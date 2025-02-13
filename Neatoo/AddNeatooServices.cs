﻿using Microsoft.Extensions.DependencyInjection;
using Neatoo.AuthorizationRules;
using Neatoo.Core;
using Neatoo.Portal;
using Neatoo.Portal.Internal;
using Neatoo.Rules;
using Neatoo.Rules.Rules;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;

namespace Neatoo;


public enum DataMapperHost
{
    Local,
    Remote
}


public delegate Type GetImplementationType(Type type);

public static class AddNeatooServicesExtension
{

    public static void AddNeatooServices(this IServiceCollection services, DataMapperHost portalServer)
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
            return async (RemoteDataMapperRequest portalRequest) =>
            {
                var t = INeatooJsonSerializer.ToType(portalRequest.Target.AssemblyType) ?? throw new Exception($"Type {portalRequest.Target.AssemblyType} not found");

                var portal = s.GetRequiredService(typeof(IDataMapper<>).MakeGenericType(t)) as IDataMapper;

                var result = await portal.HandlePortalRequest(portalRequest);

                var portalResponse = new RemoteDataMapperResponse(s.GetRequiredService<INeatooJsonSerializer>().Serialize(result), portalRequest.Target.AssemblyType);

                return portalResponse;
            };
        });


        // SingleInstance as long it is isn't modified to accept dependencies
        services.AddSingleton<IFactory, DefaultFactory>();

        // Meta Data about the properties and methods of Classes
        // This will not change during runtime
        // So SingleInstance
        services.AddSingleton(typeof(IPropertyInfoList<>), typeof(PropertyInfoList<>));


        // This was single instance; but now it resolves the Authorization Rules 
        // When single instance it receives the root scopewhich is no good
        services.AddScoped(typeof(IDataMapper<>), typeof(DataMapper<>));


        services.AddScopedSelf<INeatooJsonSerializer, NeatooJsonSerializer>();

        services.AddTransient<NeatooJsonConverterFactory>();

        services.AddTransient(typeof(NeatooBaseJsonTypeConverter<>));
        services.AddTransient(typeof(NeatooListBaseJsonTypeConverter<>));


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

        if (portalServer == DataMapperHost.Local)
        {
            services.AddScoped(typeof(IRemoteMethodPortal<>), typeof(LocalMethodPortal<>));

            services.AddScoped(typeof(INeatooPortal<>), typeof(NeatooPortalHost<>));

        }
        else if (portalServer == DataMapperHost.Remote)
        {
            services.AddScoped(typeof(INeatooPortal<>), typeof(NeatooPortalClient<>));
        }

        // Simple wrapper - Always InstancePerDependency
        services.AddTransient(typeof(IBaseServices<>), typeof(BaseServices<>));
        services.AddTransient(typeof(IListBaseServices<,>), typeof(ListBaseServices<,>));
        services.AddTransient(typeof(IValidateBaseServices<>), typeof(ValidateBaseServices<>));
        services.AddTransient(typeof(IValidateListBaseServices<,>), typeof(ValidateListBaseServices<,>));
        services.AddTransient(typeof(EditBaseServices<>), typeof(EditBaseServices<>));
        services.AddTransient(typeof(IEditListBaseServices<,>), typeof(EditListBaseServices<,>));

        services.AddTransient<RequestFromServerDelegate>(cc =>
        {
            var httpClient = cc.GetRequiredService<HttpClient>();
            var portalJsonSerializer = cc.GetRequiredService<INeatooJsonSerializer>();

            return async (portalRequest) =>
            {
                var response = await httpClient.PostAsync("portal", JsonContent.Create(portalRequest, typeof(RemoteDataMapperRequest)));

                if (!response.IsSuccessStatusCode)
                {
                    var issue = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Failed to call portal. Status code: {response.StatusCode} {issue}");
                }

                var result = await response.Content.ReadFromJsonAsync<RemoteDataMapperResponse>() ?? throw new HttpRequestException($"Successful Code but empty response.");;

                return result;
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
    public static void AutoRegisterAssemblyTypes(this IServiceCollection services, Assembly assembly)
    {

        var types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract).ToList();
        var interfaces = assembly.GetTypes().Where(t => t.IsInterface).ToDictionary(x => x.FullName);

        foreach (var t in types)
        {
            if (interfaces.TryGetValue($"{t.Namespace}.I{t.Name}", out var i))
            {
                var singleConstructor = t.GetConstructors().SingleOrDefault();
                var zeroConstructorParams = singleConstructor != null && !singleConstructor.GetParameters().Any();


                // AsSelf required for Deserialization
                services.AddTransient(i, t);
                services.AddTransient(t);


                // I forget why this didn't work...

                // If it is a RULE
                // and has zero constructor parameters
                // assume no dependencies
                // so it can be SingleInstance
                //if (typeof(IRule).IsAssignableFrom(t) && zeroConstructorParams)
                //{
                //    reg.SingleInstance();
                //}
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
