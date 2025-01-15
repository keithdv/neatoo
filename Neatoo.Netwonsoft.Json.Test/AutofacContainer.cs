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
using Neatoo.Netwonsoft.Json.Test.BaseTests;
using Neatoo.Netwonsoft.Json.Test.EditTests;
using Neatoo.Netwonsoft.Json.Test.ValidateTests;
using Neatoo.Newtonsoft.Json;
using Neatoo.Autofac;
using System.Reflection;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Autofac.Core.Lifetime;
using System.Diagnostics;

namespace Neatoo.Netwonsoft.Json.Test
{

    public static class AutofacContainer
    {

        private static Dictionary<Neatoo.Autofac.Portal, IContainer> containers = new Dictionary<Autofac.Portal, IContainer>();

        public static ILifetimeScope GetLifetimeScope(Neatoo.Autofac.Portal portal = Autofac.Portal.Local)
        {

            if (!containers.ContainsKey(portal))
            {
                var builder = new ContainerBuilder();

                // Run first - some of these definition need to be modified
                builder.RegisterModule(new NeatooCoreModule(portal));

                builder.AutoRegisterAssemblyTypes(Assembly.GetExecutingAssembly());

                // Newtonsoft.Json
                builder.RegisterType<FatClientContractResolver>().AsSelf().As<IPortalJsonSerializer>();
                builder.RegisterType<ListBaseCollectionConverter>();

                builder.RegisterType<DisposableDependencyList>();
                builder.RegisterType<DisposableDependency>().As<IDisposableDependency>().InstancePerLifetimeScope();

                builder.Register<MethodObject.CommandMethod>(cc =>
                {
                    var dd = cc.Resolve<Func<IDisposableDependency>>();
                    return i => MethodObject.CommendMethod_(i, dd());
                });

                builder.RegisterGeneric(typeof(RemoteMethodCall<,,>)).As(typeof(IRemoteMethod<,,>)).AsSelf();
                builder.RegisterType<MethodObject>();

                builder.Register<RequestFromServerDelegate>(cc =>
                {

                    var portalJsonSerializer = cc.Resolve<IPortalJsonSerializer>();
                    var scope = cc.Resolve<IServiceScope>();

                    return async (portalRequest) =>
                    {
                        // Simulate making a server call
                        // TODO: Duplicate Code -> Probably want to make this a delegate and share with the controller

                        var serialized = portalJsonSerializer.Serialize(portalRequest);

                        portalRequest = portalJsonSerializer.Deserialize<PortalRequest>(serialized);

                        var t = portalRequest.Target.Type() ?? throw new Exception($"Type {portalRequest.Target.Type} not found");

                        if (t.IsInterface)
                        {
                            t = scope.ConcreteType(t);
                        }
                        else
                        {
                            Debug.WriteLine($"Type {portalRequest.Target.AssemblyType} is not an interface");
                        }

                        var portal = (IPortalOperationManager) scope.Resolve(typeof(IPortalOperationManager<>).MakeGenericType(t));

                        var result = await portal.HandlePortalRequest(portalRequest);

                        var portalResponse = new PortalResponse()
                        {
                            ObjectJson = portalJsonSerializer.Serialize(result),
                            AssemblyType = portalRequest.Target.AssemblyType
                        };

                        return portalResponse;
                    };
                });

                containers.Add(portal, builder.Build());
            }

            return containers[portal].BeginLifetimeScope(Guid.NewGuid());

        }

    }
}
