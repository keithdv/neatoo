using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.UnitTest.Objects;
using Neatoo.UnitTest.PersonObjects;
using System;
using System.Linq;

/*
Debugging Messages:
: PersonValidateBase<ValidateDependencyRules>, IValidateDependencyRules
: ValidateBase<T>, IPersonBase
No DataMapperMethod attribute for FillFromDto
*/
namespace Neatoo.UnitTest.ValidateBaseTests
{
    public interface IValidateDependencyRulesFactory
    {
    }

    internal class ValidateDependencyRulesFactory : FactoryBase, IValidateDependencyRulesFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public ValidateDependencyRulesFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public ValidateDependencyRulesFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<ValidateDependencyRules>();
            services.AddScoped<ValidateDependencyRulesFactory>();
            services.AddScoped<IValidateDependencyRulesFactory, ValidateDependencyRulesFactory>();
            services.AddTransient<IValidateDependencyRules, ValidateDependencyRules>();
        }
    }
}