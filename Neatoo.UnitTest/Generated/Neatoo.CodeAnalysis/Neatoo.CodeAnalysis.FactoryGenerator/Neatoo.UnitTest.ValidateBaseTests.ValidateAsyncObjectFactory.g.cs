using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Core;
using Neatoo.UnitTest.PersonObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

/*
Debugging Messages:
: PersonValidateBase<ValidateAsyncObject>, IValidateAsyncObject
: ValidateBase<T>, IPersonBase
No DataMapperMethod attribute for FillFromDto
*/
namespace Neatoo.UnitTest.ValidateBaseTests
{
    public interface IValidateAsyncObjectFactory
    {
        Task<IValidateAsyncObject> Fetch(PersonDto person);
    }

    internal class ValidateAsyncObjectFactory : FactoryBase, IValidateAsyncObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public ValidateAsyncObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public ValidateAsyncObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public virtual Task<IValidateAsyncObject> Fetch(PersonDto person)
        {
            return LocalFetch(person);
        }

        public Task<IValidateAsyncObject> LocalFetch(PersonDto person)
        {
            var target = ServiceProvider.GetRequiredService<ValidateAsyncObject>();
            var portal = ServiceProvider.GetService<ValidateAsyncObjectFactory>();
            var personTable = ServiceProvider.GetService<IReadOnlyList<PersonDto>>();
            return DoMapperMethodCallAsync<IValidateAsyncObject>(target, DataMapperMethod.Fetch, () => target.Fetch(person, portal, personTable));
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<ValidateAsyncObject>();
            services.AddScoped<ValidateAsyncObjectFactory>();
            services.AddScoped<IValidateAsyncObjectFactory, ValidateAsyncObjectFactory>();
            services.AddTransient<IValidateAsyncObject, ValidateAsyncObject>();
        }
    }
}