using Microsoft.Extensions.DependencyInjection;
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
using Neatoo.Rules;
using Neatoo.UnitTest.Portal;
using System.Reflection;
using Neatoo.UnitTest.EditBaseTests;
using Neatoo.UnitTest.Objects;

namespace Neatoo.UnitTest
{

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

                    IServiceProvider CreateContainer(PortalServer? portal)
                    {
                        var services = new ServiceCollection();

                        services.AddNeatooServices(PortalServer.Local);

                        if (portal == null)
                        {
                            services.AddScoped(typeof(IReadPortal<>), typeof(MockReadPortal<>));
                            services.AddScoped(typeof(IReadPortalChild<>), typeof(MockReadPortalChild<>));
                            services.AddScoped(typeof(IReadWritePortal<>), typeof(MockReadWritePortal<>));
                            services.AddScoped(typeof(IReadWritePortalChild<>), typeof(MockReadWritePortalChild<>));

                        }

                        services.AutoRegisterAssemblyTypes(Assembly.GetExecutingAssembly());

                        // Unit Test Library
                        services.AddScoped<BaseTests.Authorization.IAuthorizationGrantedRule, BaseTests.Authorization.AuthorizationGrantedRule>();
                        services.AddScoped<BaseTests.Authorization.IAuthorizationGrantedAsyncRule, BaseTests.Authorization.AuthorizationGrantedAsyncRule>();
                        services.AddScoped<BaseTests.Authorization.IAuthorizationGrantedDependencyRule, BaseTests.Authorization.AuthorizationGrantedDependencyRule>();

                        services.AddTransient<Func<IDisposableDependency>>(cc => () => cc.GetRequiredService<IDisposableDependency>());

                        services.AddTransient<Objects.IDisposableDependency, Objects.DisposableDependency>();
                        services.AddScoped<Objects.DisposableDependencyList>();

                        services.AddTransient<MethodObject.Execute>(cc =>
                        {
                            var dd = cc.GetRequiredService<Func<Objects.IDisposableDependency>>();
                            return i => MethodObject.ExecuteServer(i, dd());
                        });

                        services.AddSingleton<IReadOnlyList<PersonObjects.PersonDto>>(cc => PersonObjects.PersonDto.Data());


                        return services.BuildServiceProvider();
                    }

                    Container = CreateContainer(null);
                    LocalPortalContainer = CreateContainer(PortalServer.Local);

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
}
