using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules;
using System;
using System.Collections.Generic;

/*
Debugging Messages:
: ValidateListBase<ValidateObjectList, IValidateObject>, IValidateObjectList
*/
namespace Neatoo.UnitTest.SystemTextJson
{
    public interface IValidateObjectListFactory
    {
    }

    internal class ValidateObjectListFactory : FactoryBase, IValidateObjectListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
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