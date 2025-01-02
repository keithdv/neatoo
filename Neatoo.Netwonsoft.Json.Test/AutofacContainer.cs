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

namespace Neatoo.Netwonsoft.Json.Test
{

    public static class AutofacContainer
    {

        private static IContainer Container;

        public static ILifetimeScope GetLifetimeScope()
        {

            if (Container == null)
            {
                var builder = new ContainerBuilder();

                // Run first - some of these definition need to be modified
                builder.RegisterModule(new NeatooCoreModule(Autofac.Portal.Local));

                builder.AutoRegisterAssemblyTypes(Assembly.GetExecutingAssembly());

                // Newtonsoft.Json
                builder.RegisterType<FatClientContractResolver>();
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

                Container = builder.Build();
            }

            return Container.BeginLifetimeScope(Guid.NewGuid());

        }

    }
}
