using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
Debugging Messages:
: EditListBase<EditPersonList, IEditPerson>, IEditPersonList
*/
namespace Neatoo.UnitTest.EditBaseTests
{
    public interface IEditPersonListFactory
    {
    }

    internal class EditPersonListFactory : FactoryBase, IEditPersonListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public EditPersonListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public EditPersonListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<EditPersonList>();
            services.AddScoped<EditPersonListFactory>();
            services.AddScoped<IEditPersonListFactory, EditPersonListFactory>();
            services.AddTransient<IEditPersonList, EditPersonList>();
        }
    }
}