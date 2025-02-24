using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.UnitTest.PersonObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
Debugging Messages:
: ValidateListBase<ValidateObjectList, IValidateObject>, IValidateObjectList
*/
namespace Neatoo.UnitTest.ValidateBaseTests
{
    public interface IValidateObjectListFactory
    {
    }

    internal class ValidateObjectListFactory : FactoryBase, IValidateObjectListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public ValidateObjectListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public ValidateObjectListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<ValidateObjectList>();
            services.AddScoped<ValidateObjectListFactory>();
            services.AddScoped<IValidateObjectListFactory, ValidateObjectListFactory>();
            services.AddTransient<IValidateObjectList, ValidateObjectList>();
        }
    }
}