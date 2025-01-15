using Neatoo.Portal;
using Neatoo.Autofac;
using Autofac;
using System.Reflection;
using HorseBarn.Dal.Ef;

namespace HorseBarn.lib.integration.tests
{

    public static class AutofacContainer
    {


        private static IContainer? Container;

        public static ILifetimeScope GetLifetimeScope()
        {

            if (Container == null)
            {

                IContainer CreateContainer(Neatoo.Autofac.Portal portal)
                {
                    var builder = new ContainerBuilder();

                    builder.RegisterModule(new Neatoo.Autofac.NeatooCoreModule(portal));

                    builder.AutoRegisterAssemblyTypes(Assembly.GetExecutingAssembly());

                    builder.AutoRegisterAssemblyTypes(Assembly.GetAssembly(typeof(IHorseBarn)));

                    builder.RegisterType<HorseBarnContext>().As<IHorseBarnContext>().AsSelf().InstancePerLifetimeScope();

                    return builder.Build();
                }

                // 2-Tier tests
                Container = CreateContainer(Neatoo.Autofac.Portal.Local);

            }

            return Container.BeginLifetimeScope();

        }

    }
}
