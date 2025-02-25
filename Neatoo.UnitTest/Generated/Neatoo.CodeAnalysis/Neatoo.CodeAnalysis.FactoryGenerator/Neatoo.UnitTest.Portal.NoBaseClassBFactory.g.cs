using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.AuthorizationRules;
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
    }

    internal class NoBaseClassBFactory : FactoryBase, INoBaseClassBFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public CreateRemoteDelegate CreateRemoteProperty { get; }

        public delegate Task<INoBaseClassB> CreateRemoteDelegate(string name);
        public NoBaseClassBFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateRemoteProperty = LocalCreateRemote;
        }

        public NoBaseClassBFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
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
            return Task.FromResult(DoMapperMethodCall<INoBaseClassB>(target, DataMapperMethod.Create, () => target.CreateRemote(name)));
        }

        public virtual async Task<INoBaseClassB> RemoteCreateRemote(string name)
        {
            return await DoRemoteRequest.ForDelegate<INoBaseClassB>(typeof(CreateRemoteDelegate), [name]);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<NoBaseClassB>();
            services.AddScoped<NoBaseClassBFactory>();
            services.AddScoped<INoBaseClassBFactory, NoBaseClassBFactory>();
            services.AddTransient<INoBaseClassB, NoBaseClassB>();
            services.AddScoped<CreateRemoteDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<NoBaseClassBFactory>();
                return (string name) => factory.LocalCreateRemote(name);
            });
        }
    }
}