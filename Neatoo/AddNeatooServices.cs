using Microsoft.Extensions.DependencyInjection;
using Neatoo.AuthorizationRules;
using Neatoo.Core;
using Neatoo.Portal;
using Neatoo.Portal.Internal;
using Neatoo.Rules;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Neatoo;


public enum NeatooHost
{
    Local,
    Remote
}


public delegate Type GetImplementationType(Type type);

public static class AddNeatooServicesExtension
{

    public static void AddNeatooServices(this IServiceCollection services, NeatooHost portalServer, params Assembly[] assemblies)
    {

        services.AddTransient<GetImplementationType>(s =>
        {
            return (Type type) =>
            {
                if (type.IsInterface)
                {
                    // This is why these are delegates
                    // Need access to the DI container
                    var implementationType = services
                                             .Where(d => d.ServiceType == type && d.ImplementationType != null)
                                             .Select(d => d.ImplementationType)
                                             .SingleOrDefault();

                    if (implementationType == null)
                    {
                        throw new Exception($"Type {type.FullName} not registered");
                    };

                    type = implementationType;
                }

                return type;
            };
        });

        services.AddTransient<ServerHandlePortalRequest>(s =>
        {
            var seralizer = s.GetRequiredService<INeatooJsonSerializer>();

            return async (RemoteRequestDto portalRequest) =>
            {
                var remoteRequest = seralizer.DeserializeRemoteRequest(portalRequest);

                ArgumentNullException.ThrowIfNull(remoteRequest.DelegateType, nameof(remoteRequest.DelegateType));

                Delegate method = (Delegate)s.GetRequiredService(remoteRequest.DelegateType);

                object? result = method.DynamicInvoke(remoteRequest.Parameters);

                if (result is Task task)
                {
                    await task;
                    result = task.GetType().GetProperty("Result").GetValue(task);
                }

                var portalResponse = new RemoteResponseDto(seralizer.Serialize(result));

                return portalResponse;
            };
        });

        services.AddSingleton<ILocalAssemblies>(new LocalAssemblies(assemblies));


        // SingleInstance as long it is isn't modified to accept dependencies
        services.AddSingleton<IFactory, DefaultFactory>();

        // Meta Data about the properties and methods of Classes
        // This will not change during runtime
        // So SingleInstance
        services.AddSingleton(typeof(IPropertyInfoList<>), typeof(PropertyInfoList<>));

        services.AddScopedSelf<INeatooJsonSerializer, NeatooJsonSerializer>();

        services.AddTransient<NeatooJsonConverterFactory>();

        services.AddTransient(typeof(NeatooBaseJsonTypeConverter<>));
        services.AddTransient(typeof(NeatooListBaseJsonTypeConverter<>));
        services.AddTransient(typeof(NeatooInterfaceJsonTypeConverter<>));

        services.AddSingleton<IAttributeToRule, AttributeToRule>();

        services.AddScoped(typeof(IAuthorizationRuleManager<>), typeof(AuthorizationRuleManager<>));
        services.AddTransient(typeof(IRuleManager<>), typeof(RuleManager<>));

        services.AddTransient(typeof(RuleManagerFactory<>));

        services.AddTransient<CreatePropertyInfoWrapper>(factory =>
        {
            return (propertyInfo) =>
            {
                return new PropertyInfoWrapper(propertyInfo);
            };
        });

        // Stored values for each Domain Object instance
        // MUST BE per instance

        // Use delegates to construct - including this confuses ASP.NET Core DI
        //services.AddTransient<IPropertyManager<IProperty>, PropertyManager<IProperty>>();
        //services.AddTransient<IValidatePropertyManager<IValidateProperty>, ValidatePropertyManager<IValidateProperty>>();
        //services.AddTransient<IEditPropertyManager, EditPropertyManager>();

        services.AddTransient<CreatePropertyManager>(services =>
        {
            return (IPropertyInfoList propertyInfoList) =>
            {
                return new PropertyManager<IProperty>(propertyInfoList, services.GetRequiredService<IFactory>());
            };
        });

        services.AddTransient<CreateValidatePropertyManager>(services =>
        {
            return (IPropertyInfoList propertyInfoList) =>
            {
                return new ValidatePropertyManager<IValidateProperty>(propertyInfoList, services.GetRequiredService<IFactory>());
            };
        });

        services.AddTransient<CreateEditPropertyManager>(services =>
        {
            return (IPropertyInfoList propertyInfoList) =>
            {
                return new EditPropertyManager(propertyInfoList, services.GetRequiredService<IFactory>());
            };
        });

        if (portalServer == NeatooHost.Remote)
        {
            services.AddScoped<IDoRemoteRequest, DoRemoteRequest>();
        }

        // Simple wrapper - Always InstancePerDependency
        services.AddTransient(typeof(IBaseServices<>), typeof(BaseServices<>));
        services.AddTransient(typeof(IValidateBaseServices<>), typeof(ValidateBaseServices<>));
        services.AddTransient(typeof(IEditBaseServices<>), typeof(EditBaseServices<>));

        services.AddTransient<SendRemoteRequestToServer>(cc =>
        {
            var httpClient = cc.GetRequiredService<HttpClient>();

            return async (portalRequest) =>
            {
                var response = await httpClient.PostAsync("portal", JsonContent.Create(portalRequest, typeof(RemoteRequestDto)));

                if (!response.IsSuccessStatusCode)
                {
                    var issue = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Failed to call portal. Status code: {response.StatusCode} {issue}");
                }

                var result = await response.Content.ReadFromJsonAsync<RemoteResponseDto>() ?? throw new HttpRequestException($"Successful Code but empty response."); ;

                return result;
            };
        });

        foreach (var assembly in assemblies)
        {
            services.AutoRegisterAssemblyTypes(assembly, portalServer);
        }

        services.AddTransient<IAllRequiredRulesExecuted.Factory>(cc =>
        {
            return (IEnumerable<IRequiredRule> rules) =>
            {
                return new AllRequiredRulesExecuted(rules);
            };
        });
    }


    /// <summary>
    /// Auto register every type that has a corresponding interface linked by name in the same namespace
    /// Example MyObject will be linked to IMyObject 
    /// If it is a rule with no constructor parameters it will be registered as single instance
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="assembly"></param>
    private static void AutoRegisterAssemblyTypes(this IServiceCollection services, Assembly assembly, NeatooHost portalServer)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));

        var types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract).ToList();
        var interfaces = assembly.GetTypes().Where(t => t.IsInterface).ToDictionary(x => x.FullName);

        foreach (var t in types)
        {
            var registrationMethod = t.GetMethod("FactoryServiceRegistrar", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            if (registrationMethod != null)
            {
                registrationMethod.Invoke(null, new object[] { services });
            }
            else if (interfaces.TryGetValue($"{t.Namespace}.I{t.Name}", out var i))
            {
                //var singleConstructor = t.GetConstructors().SingleOrDefault();
                //var zeroConstructorParams = singleConstructor != null && !singleConstructor.GetParameters().Any();

                // AsSelf required for Deserialization
                services.AddTransient(i, t);
                services.AddTransient(t);
            }
        }


    }

    public static IServiceCollection AddTransientSelf<I, T>(this IServiceCollection services) where T : class, I where I : class
    {
        services.AddTransient<I, T>();
        services.AddTransient(
            provider => (T)provider.GetService<I>());
        return services;
    }

    public static IServiceCollection AddScopedSelf<I, T>(this IServiceCollection services) where T : class, I where I : class
    {
        services.AddScoped<I, T>();
        services.AddScoped(
            provider => (T)provider.GetService<I>());
        return services;
    }

}
