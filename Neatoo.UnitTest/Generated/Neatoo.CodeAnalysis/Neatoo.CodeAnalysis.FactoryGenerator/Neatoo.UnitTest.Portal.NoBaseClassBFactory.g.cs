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
: INoBaseClassB
*/
namespace Neatoo.UnitTest.Portal
{
    public interface INoBaseClassBFactory
    {
        INoBaseClassB Create(string name);
        Task<INoBaseClassB> CreateRemote(string name);
        delegate INoBaseClassB CreateDelegate(string name);
        delegate Task<INoBaseClassB> CreateRemoteDelegate(string name);
    }

    internal class NoBaseClassBFactory : FactoryBase, INoBaseClassBFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public INoBaseClassBFactory.CreateRemoteDelegate CreateRemoteProperty { get; }

        public NoBaseClassBFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateRemoteProperty = LocalCreateRemote;
        }

        public NoBaseClassBFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateRemoteProperty = RemoteCreateRemote;
        }

        public virtual Task<INoBaseClassB> CreateRemote(string name)
        {
            return CreateRemoteProperty(name);
        }

        public INoBaseClassB Create(string name)
        {
            var target = ServiceProvider.GetRequiredService<NoBaseClassB>();
            return DoMapperMethodCall<INoBaseClassB>(target, DataMapperMethod.Create, () => target.Create(name));
        }

        public Task<INoBaseClassB> LocalCreateRemote(string name)
        {
            var target = ServiceProvider.GetRequiredService<NoBaseClassB>();
            return DoMapperMethodCallAsync<INoBaseClassB>(target, DataMapperMethod.Create, () => target.CreateRemote(name));
        }

        public virtual async Task<INoBaseClassB?> RemoteCreateRemote(string name)
        {
            return await DoRemoteRequest.ForDelegate<NoBaseClassB?>(typeof(INoBaseClassBFactory.CreateRemoteDelegate), [name]);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<NoBaseClassB>();
            services.AddTransient<INoBaseClassB, NoBaseClassB>();
            services.AddScoped<NoBaseClassBFactory>();
            services.AddScoped<INoBaseClassBFactory, NoBaseClassBFactory>();
            services.AddScoped<INoBaseClassBFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<NoBaseClassBFactory>();
                return (string name) => factory.Create(name);
            });
            services.AddScoped<INoBaseClassBFactory.CreateRemoteDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<NoBaseClassBFactory>();
                return (string name) => factory.LocalCreateRemote(name);
            });
        }
    }
}