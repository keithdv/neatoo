using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Neatoo.UnitTest.Portal;
using System.Reflection;
using Neatoo.UnitTest.Objects;
using Neatoo.Portal;

namespace Neatoo.UnitTest;


public static class UnitTestServices
{
    private static IServiceProvider Container;
    private static IServiceProvider LocalPortalContainer;
    private static object lockContainer = new object();

    public static IServiceScope GetLifetimeScope(bool localPortal = false)
    {

        lock (lockContainer)
        {
            if (Container == null)
            {

                IServiceProvider CreateContainer(NeatooHost? portal)
                {
                    var services = new ServiceCollection();

                    services.AddNeatooServices(NeatooHost.Local, Assembly.GetExecutingAssembly());

                    if (portal == null)
                    {
                        services.AddScoped(typeof(INeatooPortal<>), typeof(MockDataMapper<>));
                    }

                    // Unit Test Library
                    services.AddScoped<BaseTests.Authorization.IAuthorizationGrantedRule, BaseTests.Authorization.AuthorizationGrantedRule>();
                    services.AddScoped<BaseTests.Authorization.IAuthorizationGrantedAsyncRule, BaseTests.Authorization.AuthorizationGrantedAsyncRule>();
                    services.AddScoped<BaseTests.Authorization.IAuthorizationGrantedDependencyRule, BaseTests.Authorization.AuthorizationGrantedDependencyRule>();

                    services.AddTransient<Func<IDisposableDependency>>(cc => () => cc.GetRequiredService<IDisposableDependency>());

                    services.AddTransient<Objects.IDisposableDependency, Objects.DisposableDependency>();
                    services.AddScoped<Objects.DisposableDependencyList>();

                    services.AddSingleton<IReadOnlyList<PersonObjects.PersonDto>>(cc => PersonObjects.PersonDto.Data());

                    return services.BuildServiceProvider();
                }

                Container = CreateContainer(null);
                LocalPortalContainer = CreateContainer(NeatooHost.Local);

            }

            if (!localPortal)
            {
                return Container.CreateScope();
            }
            else
            {
                return LocalPortalContainer.CreateScope();
            }
        }
    }
}

public static class  ServiceScopeProviderExtension
{
    public static T GetRequiredService<T>(this IServiceScope service)
    {
        return service.ServiceProvider.GetRequiredService<T>();
    }
}
