﻿using Microsoft.Extensions.DependencyInjection;
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
: List<INoBaseClass>, INoBaseClassList
*/
namespace Neatoo.UnitTest.Portal
{
    public interface INoBaseClassListFactory
    {
        INoBaseClassList Create();
        Task<INoBaseClassList> CreateRemote();
    }

    internal class NoBaseClassListFactory : FactoryBase, INoBaseClassListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public CreateRemoteDelegate CreateRemoteProperty { get; }

        public delegate Task<INoBaseClassList> CreateRemoteDelegate();
        public NoBaseClassListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateRemoteProperty = LocalCreateRemote;
        }

        public NoBaseClassListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateRemoteProperty = RemoteCreateRemote;
        }

        public virtual Task<INoBaseClassList> CreateRemote()
        {
            return CreateRemoteProperty();
        }

        public INoBaseClassList Create()
        {
            var target = ServiceProvider.GetRequiredService<NoBaseClassList>();
            var factoryA = ServiceProvider.GetService<INoBaseClassAFactory>();
            var factoryB = ServiceProvider.GetService<INoBaseClassBFactory>();
            return DoMapperMethodCall<INoBaseClassList>(target, DataMapperMethod.Create, () => target.Create(factoryA, factoryB));
        }

        public Task<INoBaseClassList> LocalCreateRemote()
        {
            var target = ServiceProvider.GetRequiredService<NoBaseClassList>();
            var factoryA = ServiceProvider.GetService<INoBaseClassAFactory>();
            var factoryB = ServiceProvider.GetService<INoBaseClassBFactory>();
            return DoMapperMethodCallAsync<INoBaseClassList>(target, DataMapperMethod.Create, () => target.CreateRemote(factoryA, factoryB));
        }

        public virtual async Task<INoBaseClassList> RemoteCreateRemote()
        {
            return await DoRemoteRequest.ForDelegate<NoBaseClassList>(typeof(CreateRemoteDelegate), []);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<NoBaseClassList>();
            services.AddScoped<NoBaseClassListFactory>();
            services.AddScoped<INoBaseClassListFactory, NoBaseClassListFactory>();
            services.AddTransient<INoBaseClassList, NoBaseClassList>();
            services.AddScoped<CreateRemoteDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<NoBaseClassListFactory>();
                return () => factory.LocalCreateRemote();
            });
        }
    }
}