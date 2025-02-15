using HorseBarn.Dal.Ef;
using Microsoft.Extensions.DependencyInjection;
using Neatoo;
using System.Reflection;

namespace HorseBarn.lib.integration.tests;

internal class UnitTestContainer
{

    private static IServiceProvider? Container;

    public static IServiceScope GetLifetimeScope()
    {

        if (Container == null)
        {

            IServiceProvider CreateContainer(Neatoo.NeatooHost portal)
            {
                var builder = new ServiceCollection();

                builder.AddNeatooServices(portal, Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(IHorseBarn)));

                builder.AddScopedSelf<IHorseBarnContext, HorseBarnContext>();

                return builder.BuildServiceProvider();
            }

            // 2-Tier tests
            Container = CreateContainer(NeatooHost.Local);

        }

        return Container.CreateScope();

    }

}
