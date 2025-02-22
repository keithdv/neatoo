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
: EditListBase<EditObjectList, IEditObject>, IEditObjectList
*/
namespace Neatoo.UnitTest.ObjectPortal
{
    public interface IEditObjectListFactory
    {
    }

    internal class EditObjectListFactory : FactoryBase, IEditObjectListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public EditObjectListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public EditObjectListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<EditObjectList>();
            services.AddScoped<EditObjectListFactory>();
            services.AddScoped<IEditObjectListFactory, EditObjectListFactory>();
            services.AddScoped<IEditObjectList, EditObjectList>();
        }
    }
}