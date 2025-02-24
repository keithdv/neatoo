using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.AuthorizationRules;
using Neatoo.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
Debugging Messages:
: IAuthorizationClass
*/
namespace Neatoo.UnitTest.Portal
{
    public interface IAuthorizationClassFactory
    {
        IAuthorizationClass CanRead();
        Task<IAuthorizationClass> CanReadAsync();
        Task<IAuthorizationClass> CanReadRemote(int param);
        IAuthorizationClass CanWrite();
    }

    internal class AuthorizationClassFactory : FactoryBase, IAuthorizationClassFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public CanReadRemoteDelegate CanReadRemoteProperty { get; }

        public delegate IAuthorizationClass CanReadDelegate();
        public delegate Task<IAuthorizationClass> CanReadAsyncDelegate();
        public delegate Task<IAuthorizationClass> CanReadRemoteDelegate(int param);
        public delegate IAuthorizationClass CanWriteDelegate();
        public AuthorizationClassFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CanReadRemoteProperty = LocalCanReadRemote;
        }

        public AuthorizationClassFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CanReadRemoteProperty = RemoteCanReadRemote;
        }

        public virtual Task<IAuthorizationClass> CanReadRemote(int param)
        {
            return CanReadRemoteProperty(param);
        }

        public IAuthorizationClass CanRead()
        {
            var target = ServiceProvider.GetRequiredService<AuthorizationClass>();
            var disposableDependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IAuthorizationClass>(target, DataMapperMethod.Authorize, () => target.CanRead(disposableDependency));
        }

        public Task<IAuthorizationClass> CanReadAsync()
        {
            var target = ServiceProvider.GetRequiredService<AuthorizationClass>();
            return DoMapperMethodCallAsync<IAuthorizationClass>(target, DataMapperMethod.Authorize, () => target.CanReadAsync());
        }

        public Task<IAuthorizationClass> LocalCanReadRemote(int param)
        {
            var target = ServiceProvider.GetRequiredService<AuthorizationClass>();
            return DoMapperMethodCallAsync<IAuthorizationClass>(target, DataMapperMethod.Authorize, () => target.CanReadRemote(param));
        }

        public IAuthorizationClass CanWrite()
        {
            var target = ServiceProvider.GetRequiredService<AuthorizationClass>();
            return DoMapperMethodCall<IAuthorizationClass>(target, DataMapperMethod.Authorize, () => target.CanWrite());
        }

        public virtual async Task<IAuthorizationClass> RemoteCanReadRemote(int param)
        {
            return await DoRemoteRequest.ForDelegate<AuthorizationClass>(typeof(CanReadRemoteDelegate), [param]);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<AuthorizationClass>();
            services.AddScoped<AuthorizationClassFactory>();
            services.AddScoped<IAuthorizationClassFactory, AuthorizationClassFactory>();
            services.AddTransient<IAuthorizationClass, AuthorizationClass>();
            services.AddScoped<CanReadDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizationClassFactory>();
                return () => factory.CanRead();
            });
            services.AddScoped<CanReadAsyncDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizationClassFactory>();
                return () => factory.CanReadAsync();
            });
            services.AddScoped<CanReadRemoteDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizationClassFactory>();
                return (int param) => factory.LocalCanReadRemote(param);
            });
            services.AddScoped<CanWriteDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizationClassFactory>();
                return () => factory.CanWrite();
            });
        }
    }
}