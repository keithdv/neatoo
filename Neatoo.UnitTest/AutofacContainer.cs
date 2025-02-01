using Autofac;
using Autofac.Core;
using Neatoo.AuthorizationRules;
using Neatoo.Core;
using Neatoo.Portal;
using Neatoo.Portal.Core;
using Neatoo.UnitTest.ObjectPortal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Autofac.Builder;
using Neatoo.Rules;
using Neatoo.UnitTest.Portal;
using Neatoo.Autofac;
using System.Reflection;
using Neatoo.UnitTest.EditBaseTests;

namespace Neatoo.UnitTest
{

    public static class AutofacContainer
    {


        private static IContainer Container;
        private static IContainer LocalPortalContainer;
        private static object lockContainer = new object();

        public static ILifetimeScope GetLifetimeScope(bool localPortal = false)
        {

            lock (lockContainer)
            {
                if (Container == null)
                {

                    IContainer CreateContainer(Autofac.Portal portal)
                    {
                        var builder = new ContainerBuilder();

                        builder.RegisterModule(new Neatoo.Autofac.NeatooCoreModule(portal));

                        if (portal == Autofac.Portal.NoPortal)
                        {
                            builder.RegisterGeneric(typeof(MockReadPortal<>)).As(typeof(IReadPortal<>)).AsSelf().InstancePerLifetimeScope();
                            builder.RegisterGeneric(typeof(MockReadPortalChild<>)).As(typeof(IReadPortalChild<>)).AsSelf().InstancePerLifetimeScope();
                            builder.RegisterGeneric(typeof(MockReadWritePortal<>)).As(typeof(IReadWritePortal<>)).AsSelf().InstancePerLifetimeScope();
                            builder.RegisterGeneric(typeof(MockReadWritePortalChild<>)).As(typeof(IReadWritePortalChild<>)).AsSelf().InstancePerLifetimeScope();
                        }


                        builder.AutoRegisterAssemblyTypes(Assembly.GetExecutingAssembly());

                        // Unit Test Library
                        builder.RegisterType<BaseTests.Authorization.AuthorizationGrantedRule>().As<BaseTests.Authorization.IAuthorizationGrantedRule>().InstancePerLifetimeScope(); // Not normal - Lifetimescope so the results can be validated
                        builder.RegisterType<BaseTests.Authorization.AuthorizationGrantedAsyncRule>().As<BaseTests.Authorization.IAuthorizationGrantedAsyncRule>().InstancePerLifetimeScope(); // Not normal - Lifetimescope so the results can be validated
                        builder.RegisterType<BaseTests.Authorization.AuthorizationGrantedDependencyRule>().As<BaseTests.Authorization.IAuthorizationGrantedDependencyRule>().InstancePerLifetimeScope(); // Not normal - Lifetimescope so the results can be validated

                        builder.RegisterType<Objects.DisposableDependency>().As<Objects.IDisposableDependency>();
                        builder.RegisterType<Objects.DisposableDependencyList>().InstancePerLifetimeScope();

                        builder.Register<MethodObject.Execute>(cc =>
                        {
                            var dd = cc.Resolve<Func<Objects.IDisposableDependency>>();
                            return i => MethodObject.ExecuteServer(i, dd());
                        });

                        builder.Register<IReadOnlyList<PersonObjects.PersonDto>>(cc =>
                        {
                            return PersonObjects.PersonDto.Data();
                        }).SingleInstance();

                        return builder.Build();
                    }

                    Container = CreateContainer(Autofac.Portal.NoPortal);
                    LocalPortalContainer = CreateContainer(Autofac.Portal.Local);

                }

                if (!localPortal)
                {
                    return Container.BeginLifetimeScope();
                }
                else
                {
                    return LocalPortalContainer.BeginLifetimeScope();
                }
            }
        }

    }
}
