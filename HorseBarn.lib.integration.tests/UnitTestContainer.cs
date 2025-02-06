using HorseBarn.Dal.Ef;
using Microsoft.Extensions.DependencyInjection;
using Neatoo;
using Neatoo.Portal.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.integration.tests
{
    internal class UnitTestContainer
    {

        private static IServiceProvider? Container;

        public static IServiceScope GetLifetimeScope()
        {

            if (Container == null)
            {

                IServiceProvider CreateContainer(Neatoo.PortalServer portal)
                {
                    var builder = new ServiceCollection();

                    builder.AddNeatooServices(portal);

                    builder.AutoRegisterAssemblyTypes(Assembly.GetExecutingAssembly());

                    builder.AutoRegisterAssemblyTypes(Assembly.GetAssembly(typeof(IHorseBarn)));


                    builder.AddScopedSelf<IHorseBarnContext, HorseBarnContext>();

                    return builder.BuildServiceProvider();
                }

                // 2-Tier tests
                Container = CreateContainer(PortalServer.Local);

            }

            return Container.CreateScope();

        }

    }

}
