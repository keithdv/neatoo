using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using System;

/*
Debugging Messages:
: Base<BaseObject>, IBaseObject
No DataMapperMethod attribute for LoadPropertyTest
*/
namespace Neatoo.UnitTest.BaseTests.Objects
{
    public interface IBaseObjectFactory
    {
    }

    internal class BaseObjectFactory : FactoryBase, IBaseObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public BaseObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public BaseObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<BaseObject>();
            services.AddScoped<BaseObjectFactory>();
            services.AddScoped<IBaseObjectFactory, BaseObjectFactory>();
            services.AddTransient<IBaseObject, BaseObject>();
        }
    }
}