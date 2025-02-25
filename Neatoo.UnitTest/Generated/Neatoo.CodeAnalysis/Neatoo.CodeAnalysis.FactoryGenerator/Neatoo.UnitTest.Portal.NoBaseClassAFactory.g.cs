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
: INoBaseClassA
*/
namespace Neatoo.UnitTest.Portal
{
    public interface INoBaseClassAFactory
    {
        INoBaseClassA Create(string name);
        Task<INoBaseClassA> CreateRemote(string name);
    }

    internal class NoBaseClassAFactory : FactoryBase, INoBaseClassAFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public CreateRemoteDelegate CreateRemoteProperty { get; }

        public delegate Task<INoBaseClassA> CreateRemoteDelegate(string name);
        public NoBaseClassAFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateRemoteProperty = LocalCreateRemote;
        }

        public NoBaseClassAFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateRemoteProperty = RemoteCreateRemote;
        }

        public virtual Task<INoBaseClassA> CreateRemote(string name)
        {
            return CreateRemoteProperty(name);
        }

        public INoBaseClassA Create(string name)
        {
            var target = ServiceProvider.GetRequiredService<NoBaseClassA>();
            return DoMapperMethodCall<INoBaseClassA>(target, DataMapperMethod.Create, () => target.Create(name));
        }

        public Task<INoBaseClassA> LocalCreateRemote(string name)
        {
            var target = ServiceProvider.GetRequiredService<NoBaseClassA>();
            return Task.FromResult(DoMapperMethodCall<INoBaseClassA>(target, DataMapperMethod.Create, () => target.CreateRemote(name)));
        }

        public virtual async Task<INoBaseClassA> RemoteCreateRemote(string name)
        {
            return await DoRemoteRequest.ForDelegate<INoBaseClassA>(typeof(CreateRemoteDelegate), [name]);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<NoBaseClassA>();
            services.AddScoped<NoBaseClassAFactory>();
            services.AddScoped<INoBaseClassAFactory, NoBaseClassAFactory>();
            services.AddTransient<INoBaseClassA, NoBaseClassA>();
            services.AddScoped<CreateRemoteDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<NoBaseClassAFactory>();
                return (string name) => factory.LocalCreateRemote(name);
            });
        }
    }
}