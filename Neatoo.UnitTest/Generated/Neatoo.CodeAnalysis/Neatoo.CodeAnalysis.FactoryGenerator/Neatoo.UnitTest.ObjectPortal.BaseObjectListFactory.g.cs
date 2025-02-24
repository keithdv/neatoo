using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.UnitTest.Objects;
using System;
using System.Threading.Tasks;

/*
Debugging Messages:
: ListBase<BaseObjectList, IBaseObject>, IBaseObjectList
*/
namespace Neatoo.UnitTest.ObjectPortal
{
    public interface IBaseObjectListFactory
    {
    }

    internal class BaseObjectListFactory : FactoryBase, IBaseObjectListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
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