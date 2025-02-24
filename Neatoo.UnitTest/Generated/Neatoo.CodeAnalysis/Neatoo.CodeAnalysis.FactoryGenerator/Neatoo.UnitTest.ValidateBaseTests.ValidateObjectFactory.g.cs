using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.UnitTest.PersonObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

/*
Debugging Messages:
: PersonValidateBase<ValidateObject>, IValidateObject
No DataMapperMethod attribute for TestMarkInvalid
: ValidateBase<T>, IPersonBase
No DataMapperMethod attribute for FillFromDto
*/
namespace Neatoo.UnitTest.ValidateBaseTests
{
    public interface IValidateObjectFactory
    {
        Task<IValidateObject> Fetch(PersonDto person);
    }

    internal class ValidateObjectFactory : FactoryBase, IValidateObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public ValidateObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public ValidateObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public async Task<IValidateObject> Fetch(PersonDto person)
        {
            var target = ServiceProvider.GetRequiredService<ValidateObject>();
            var portal = ServiceProvider.GetService<ValidateObjectFactory>();
            var personTable = ServiceProvider.GetService<IReadOnlyList<PersonDto>>();
            return await DoMapperMethodCallAsync<IValidateObject>(target, DataMapperMethod.Fetch, () => target.Fetch(person, portal, personTable));
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<ValidateObject>();
            services.AddScoped<ValidateObjectFactory>();
            services.AddScoped<IValidateObjectFactory, ValidateObjectFactory>();
            services.AddTransient<IValidateObject, ValidateObject>();
        }
    }
}