using Autofac;
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
using Neatoo.Netwonsoft.Json;
using Neatoo.Newtonsoft.Json;
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

            // Tools - More or less Static classes - but now they can be changed! (For better or worse...)
            builder.RegisterType<Core.ValuesDiffer>().As<IValuesDiffer>().SingleInstance();

            // Scope Wrapper
            builder.RegisterType<ServiceScope>().As<IServiceScope>().InstancePerLifetimeScope();

            // Meta Data about the properties and methods of Classes
            // This will not change during runtime
            // So SingleInstance
            builder.RegisterGeneric(typeof(RegisteredPropertyManager<>)).As(typeof(IRegisteredPropertyManager<>)).SingleInstance();


            // This was single instance; but now it resolves the Authorization Rules 
            // When single instance it receives the root scopewhich is no good
            builder.RegisterGeneric(typeof(PortalOperationManager<>)).As(typeof(IPortalOperationManager<>)).InstancePerLifetimeScope();

            builder.RegisterType<FatClientContractResolver>().AsSelf().As<IPortalJsonSerializer>();
            builder.RegisterType<ListBaseCollectionConverter>().AsSelf();

            // Should not be singleinstance because AuthorizationRules can have constructor dependencies
            builder.RegisterGeneric(typeof(AuthorizationRuleManager<>)).As(typeof(IAuthorizationRuleManager<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RuleManager<>)).As(typeof(IRuleManager<>)).AsSelf();
            builder.RegisterType<RuleResultList>().As<IRuleResultList>();
            builder.RegisterType<RequiredRule>().As<IRequiredRule>();
            builder.RegisterType<AttributeToRule>().As<IAttributeToRule>().SingleInstance(); // SingleInstance is safe as long as it only resolves Func<>s
            builder.Register<CreateRequiredRule>(cc =>
            {
                return (string propertyName) => new RequiredRule(propertyName);
            });

            builder.RegisterGeneric(typeof(RegisteredProperty<>)).As(typeof(IRegisteredProperty<>));
            builder.Register<CreateRegisteredProperty>(cc =>
            {
                var scope = cc.Resolve<Func<ILifetimeScope>>();
                return (propertyInfo) =>
                {
                    return (IRegisteredProperty)scope().Resolve(typeof(IRegisteredProperty<>).MakeGenericType(propertyInfo.PropertyType), new TypedParameter(typeof(System.Reflection.PropertyInfo), propertyInfo));
                };
            });

            // Stored values for each Domain Object instance
            // MUST BE per instance
            builder.RegisterGeneric(typeof(PropertyValueManager<>)).As(typeof(IPropertyValueManager<>)).AsSelf();
            builder.RegisterGeneric(typeof(ValidatePropertyValueManager<>)).As(typeof(IValidatePropertyValueManager<>)).AsSelf();
            builder.RegisterGeneric(typeof(EditPropertyValueManager<>)).As(typeof(IEditPropertyValueManager<>)).AsSelf();

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
            builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>));
            builder.RegisterGeneric(typeof(ListBaseServices<,>)).As(typeof(IListBaseServices<,>));
            builder.RegisterGeneric(typeof(ValidateBaseServices<>)).As(typeof(IValidateBaseServices<>));
            builder.RegisterGeneric(typeof(ValidateListBaseServices<,>)).As(typeof(IValidateListBaseServices<,>));
            builder.RegisterGeneric(typeof(EditBaseServices<>)).As(typeof(IEditBaseServices<>));
            builder.RegisterGeneric(typeof(EditListBaseServices<,>)).As(typeof(IEditListBaseServices<,>));

            builder.Register<RequestFromServerDelegate>(cc => {

                var httpClient = cc.Resolve<HttpClient>();
                var portalJsonSerializer = cc.Resolve<IPortalJsonSerializer>();

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
