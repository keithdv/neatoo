using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
Debugging Messages:
: INoBaseClass
*/
namespace Neatoo.UnitTest.Portal
{
    public interface INoBaseClassFactory
    {
        INoBaseClass Create();
        Task<INoBaseClass> CreateRemote();
        delegate INoBaseClass CreateDelegate();
        delegate Task<INoBaseClass> CreateRemoteDelegate();
    }

    internal class NoBaseClassFactory : FactoryBase, INoBaseClassFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public INoBaseClassFactory.CreateRemoteDelegate CreateRemoteProperty { get; }

        public NoBaseClassFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateRemoteProperty = LocalCreateRemote;
        }

        public NoBaseClassFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateRemoteProperty = RemoteCreateRemote;
        }

        public virtual Task<INoBaseClass> CreateRemote()
        {
            return CreateRemoteProperty();
        }

        public INoBaseClass Create()
        {
            var target = ServiceProvider.GetRequiredService<NoBaseClass>();
            return DoMapperMethodCall<INoBaseClass>(target, DataMapperMethod.Create, () => target.Create());
        }

        public Task<INoBaseClass> LocalCreateRemote()
        {
            var target = ServiceProvider.GetRequiredService<NoBaseClass>();
            return DoMapperMethodCallAsync<INoBaseClass>(target, DataMapperMethod.Create, () => target.CreateRemote());
        }

        public virtual async Task<INoBaseClass?> RemoteCreateRemote()
        {
            return await DoRemoteRequest.ForDelegate<NoBaseClass?>(typeof(INoBaseClassFactory.CreateRemoteDelegate), []);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<NoBaseClass>();
            services.AddTransient<INoBaseClass, NoBaseClass>();
            services.AddScoped<NoBaseClassFactory>();
            services.AddScoped<INoBaseClassFactory, NoBaseClassFactory>();
            services.AddScoped<INoBaseClassFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<NoBaseClassFactory>();
                return () => factory.Create();
            });
            services.AddScoped<INoBaseClassFactory.CreateRemoteDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<NoBaseClassFactory>();
                return () => factory.LocalCreateRemote();
            });
        }
    }
}