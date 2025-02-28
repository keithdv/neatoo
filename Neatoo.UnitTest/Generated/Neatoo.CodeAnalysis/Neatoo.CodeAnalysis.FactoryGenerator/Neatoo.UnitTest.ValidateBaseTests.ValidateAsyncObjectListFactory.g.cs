using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Core;
using Neatoo.UnitTest.PersonObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
Debugging Messages:
: ValidateListBase<ValidateAsyncObjectList, IValidateAsyncObject>, IValidateAsyncObjectList
*/
namespace Neatoo.UnitTest.ValidateBaseTests
{
    public interface IValidateAsyncObjectListFactory
    {
    }

    internal class ValidateAsyncObjectListFactory : FactoryBase, IValidateAsyncObjectListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public ValidateAsyncObjectListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public ValidateAsyncObjectListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<ValidateAsyncObjectList>();
            services.AddScoped<ValidateAsyncObjectListFactory>();
            services.AddScoped<IValidateAsyncObjectListFactory, ValidateAsyncObjectListFactory>();
            services.AddTransient<IValidateAsyncObjectList, ValidateAsyncObjectList>();
        }
    }
}