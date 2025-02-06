using Microsoft.Extensions.DependencyInjection;
using Autofac.Core;
using Neatoo.AuthorizationRules;
using Neatoo.Core;
using Neatoo.Portal;
using Neatoo.Portal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Autofac.Builder;
using Neatoo.Rules;
using Neatoo.Rules.Rules;
using System.Reflection.Metadata.Ecma335;
using System.Net.Http;
using System.Net.Mime;

namespace Neatoo.Autofac
{
    public enum Portal
    {
        NoPortal, Client, Local
    }

    public class NeatooCoreModule : Module
    {
        public NeatooCoreModule(Portal portal)
        {
            Portal = portal;
        }

        public Portal Portal { get; }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // SingleInstance as long it is isn't modified to accept dependencies
            builder.RegisterType<DefaultFactory>().As<IFactory>().SingleInstance();


            // Scope Wrapper
            //builder.RegisterType<ServiceScope>().As<IServiceScope>().InstancePerLifetimeScope();

            // Meta Data about the properties and methods of Classes
            // This will not change during runtime
            // So SingleInstance
            builder.RegisterGeneric(typeof(RegisteredPropertyManager<>)).As(typeof(IRegisteredPropertyManager<>)).SingleInstance();


            // This was single instance; but now it resolves the Authorization Rules 
            // When single instance it receives the root scopewhich is no good
            builder.RegisterGeneric(typeof(PortalOperationManager<>)).As(typeof(IPortalOperationManager<>)).InstancePerLifetimeScope();

            builder.RegisterType<NeatooJsonSerializer>().AsSelf().As<IPortalJsonSerializer>();
            builder.RegisterType<NeatooJsonConverterFactory>().AsSelf();
            builder.RegisterGeneric(typeof(NeatooBaseJsonTypeConverter<>)).AsSelf();
            builder.RegisterGeneric(typeof(NeatooListBaseJsonTypeConverter<>)).AsSelf();

            // Should not be singleinstance because AuthorizationRules can have constructor dependencies
            builder.RegisterGeneric(typeof(AuthorizationRuleManager<>)).As(typeof(IAuthorizationRuleManager<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RuleManager<>)).As(typeof(IRuleManager<>)).AsSelf();
            builder.RegisterType<RequiredRule>().As<IRequiredRule>();
            builder.RegisterType<AttributeToRule>().As<IAttributeToRule>().SingleInstance(); // SingleInstance is safe as long as it only resolves Func<>s
            builder.Register<CreateRequiredRule>(cc =>
            {
                return (string propertyName) => new RequiredRule(propertyName);
            });

            builder.RegisterType<RegisteredProperty>().As<IRegisteredProperty>().AsSelf();
            builder.Register<CreateRegisteredProperty>(cc =>
            {
                var scope = cc.GetRequiredService<Func<IServiceScope>>();
                return (propertyInfo) =>
                {
                    return scope().GetRequiredService<Func<System.Reflection.PropertyInfo, IRegisteredProperty>>()(propertyInfo);
                };
            });

            // Stored values for each Domain Object instance
            // MUST BE per instance
            builder.RegisterType<PropertyManager<IProperty>>().As<IPropertyManager<IProperty>>().AsSelf();
            builder.RegisterType<ValidatePropertyManager<IValidateProperty>>().As<IValidatePropertyManager<IValidateProperty>>().AsSelf();
            builder.RegisterType<EditPropertyManager>().As<IEditPropertyManager>().AsSelf();

            builder.RegisterGeneric(typeof(LocalReadPortal<>))
                .As(typeof(ILocalReadPortal<>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(LocalReadWritePortal<>))
                .As(typeof(ILocalReadWritePortal<>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ClientReadPortal<>))
                .As(typeof(IClientReadPortal<>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ClientReadWritePortal<>))
                .As(typeof(IClientReadWritePortal<>))
                .InstancePerLifetimeScope();

            if (Portal == Portal.Local)
            {
                // Takes IServiceScope so these need to match it's lifetime
                builder.RegisterGeneric(typeof(LocalReadPortal<>))
                    .As(typeof(IReadPortal<>))
                    .As(typeof(IReadPortalChild<>))
                    .InstancePerLifetimeScope();

                builder.RegisterGeneric(typeof(LocalReadWritePortal<>))
                    .As(typeof(IReadWritePortal<>))
                    .As(typeof(IReadWritePortalChild<>))
                    .InstancePerLifetimeScope();

                builder.RegisterGeneric(typeof(LocalMethodPortal<>)).As(typeof(IRemoteMethodPortal<>)).AsSelf();

                builder.RegisterGeneric(typeof(Portal<>)).As(typeof(IPortal<>)).InstancePerLifetimeScope();
            }
            else if (Portal == Portal.Client)
            {
                builder.RegisterGeneric(typeof(ClientReadPortal<>))
                    .As(typeof(IReadPortal<>))
                    .As(typeof(IReadPortalChild<>))
                    .InstancePerLifetimeScope();

                builder.RegisterGeneric(typeof(ClientReadWritePortal<>))
                    .As(typeof(IReadWritePortal<>))
                    .As(typeof(IReadWritePortalChild<>))
                    .InstancePerLifetimeScope();
            }

            // Simple wrapper - Always InstancePerDependency
            builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>)) .AsSelf();
            builder.RegisterGeneric(typeof(ListBaseServices<,>)).As(typeof(IListBaseServices<,>)).AsSelf();
            builder.RegisterGeneric(typeof(ValidateBaseServices<>)).As(typeof(IValidateBaseServices<>)) .AsSelf();
            builder.RegisterGeneric(typeof(ValidateListBaseServices<,>)).As(typeof(IValidateListBaseServices<,>)).AsSelf();
            builder.RegisterGeneric(typeof(EditBaseServices<>)).As(typeof(IEditBaseServices<>)) .AsSelf();
            builder.RegisterGeneric(typeof(EditListBaseServices<,>)).As(typeof(IEditListBaseServices<,>)) .AsSelf();

            builder.Register<RequestFromServerDelegate>(cc => {

                var httpClient = cc.GetRequiredService<HttpClient>();
                var portalJsonSerializer = cc.GetRequiredService<IPortalJsonSerializer>();

                return async (portalRequest) =>
                {

                    var response = await httpClient.PostAsync("http://localhost:5037/portal", new StringContent(portalJsonSerializer.Serialize(portalRequest), Encoding.UTF8, MediaTypeNames.Application.Json));

                    if (!response.IsSuccessStatusCode)
                    {
                        var issue = response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Failed to call portal. Status code: {response.StatusCode} {issue}");
                    }

                    return portalJsonSerializer.Deserialize<PortalResponse>(await response.Content.ReadAsStringAsync());
                };
            });

        }
    }

}
