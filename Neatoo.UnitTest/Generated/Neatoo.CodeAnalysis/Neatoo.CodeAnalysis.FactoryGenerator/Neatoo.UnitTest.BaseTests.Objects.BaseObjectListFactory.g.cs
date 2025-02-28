using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using System;

/*
Debugging Messages:
: ListBase<BaseObjectList, IBaseObject>, IBaseObjectList
*/
namespace Neatoo.UnitTest.BaseTests.Objects
{
    public interface IBaseObjectListFactory
    {
    }

    internal class BaseObjectListFactory : FactoryBase, IBaseObjectListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public BaseObjectListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public BaseObjectListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<BaseObjectList>();
            services.AddScoped<BaseObjectListFactory>();
            services.AddScoped<IBaseObjectListFactory, BaseObjectListFactory>();
            services.AddTransient<IBaseObjectList, BaseObjectList>();
        }
    }
}