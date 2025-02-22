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
: ValidateBase<T>, IPersonBase
*/
namespace Neatoo.UnitTest.ValidateBaseTests
{
    public interface IValidateObjectFactory
    {
        Task<IValidateObject> Fetch(PersonDto person);
        delegate Task<IValidateObject> FetchDelegate(PersonDto person);
    }

    internal class ValidateObjectFactory : FactoryBase, IValidateObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IValidateObjectFactory.FetchDelegate FetchProperty { get; }

        public ValidateObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            FetchProperty = LocalFetch;
        }

        public ValidateObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            FetchProperty = RemoteFetch;
        }

        public virtual Task<IValidateObject> Fetch(PersonDto person)
        {
            return FetchProperty(person);
        }

        public Task<IValidateObject> LocalFetch(PersonDto person)
        {
            var target = ServiceProvider.GetRequiredService<ValidateObject>();
            var portal = ServiceProvider.GetService<ValidateObjectFactory>();
            var personTable = ServiceProvider.GetService<IReadOnlyList<PersonDto>>();
            return DoMapperMethodCallAsync<IValidateObject>(target, DataMapperMethod.Fetch, () => target.Fetch(person, portal, personTable));
        }

        public virtual async Task<IValidateObject?> RemoteFetch(PersonDto person)
        {
            return await DoRemoteRequest.ForDelegate<ValidateObject?>(typeof(IValidateObjectFactory.FetchDelegate), [person]);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<ValidateObject>();
            services.AddTransient<IValidateObject, ValidateObject>();
            services.AddScoped<ValidateObjectFactory>();
            services.AddScoped<IValidateObjectFactory, ValidateObjectFactory>();
            services.AddScoped<IValidateObjectFactory.FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<ValidateObjectFactory>();
                return (PersonDto person) => factory.LocalFetch(person);
            });
        }
    }
}