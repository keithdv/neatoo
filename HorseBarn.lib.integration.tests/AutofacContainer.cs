using Neatoo.Portal;
using Neatoo.Autofac;
using Autofac;
using System.Reflection;

namespace HorseBarn.lib.integration.tests
{

    public static class AutofacContainer
    {


        private static IContainer? Container;
        private static IContainer? LocalPortalContainer;

        public static ILifetimeScope GetLifetimeScope(bool localPortal = false)
        {

            if (Container == null || LocalPortalContainer == null)
            {

                IContainer CreateContainer(Neatoo.Autofac.Portal portal)
                {
                    var builder = new ContainerBuilder();

                    builder.RegisterModule(new Neatoo.Autofac.NeatooCoreModule(portal));

                    builder.AutoRegisterAssemblyTypes(Assembly.GetExecutingAssembly());

                    builder.AutoRegisterAssemblyTypes(Assembly.GetAssembly(typeof(IHorseBarn)));
                    return builder.Build();
                }

                // 2-Tier tests
                Container = CreateContainer(Neatoo.Autofac.Portal.Local);

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
