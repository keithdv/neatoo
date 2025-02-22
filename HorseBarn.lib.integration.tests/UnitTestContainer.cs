using HorseBarn.Dal.Ef;
using HorseBarn.lib.Horse;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

                // TODO: Autoregister
                builder.AddTransient<IsHorseNameUniqueServer>();

                builder.AddTransient<IsHorseNameUnique>(cc =>
                {
                    return async (name) =>
                    {
                        await Task.Delay(5);
                        return true;
                    };
                });

                return builder.BuildServiceProvider();
            }

            // 2-Tier tests
            Container = CreateContainer(NeatooHost.Local);

        }

        return Container.CreateScope();

    }

}
