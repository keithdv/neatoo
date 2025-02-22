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
*/
namespace Neatoo.UnitTest.ValidateBaseTests
{
    public interface IValidateAsyncObjectFactory
    {
        Task<IValidateAsyncObject> Fetch(PersonDto person);
        delegate Task<IValidateAsyncObject> FetchDelegate(PersonDto person);
    }

    internal class ValidateAsyncObjectFactory : FactoryBase, IValidateAsyncObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IValidateAsyncObjectFactory.FetchDelegate FetchProperty { get; }

        public ValidateAsyncObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            FetchProperty = LocalFetch;
        }

        public ValidateAsyncObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            FetchProperty = RemoteFetch;
        }

        public virtual Task<IValidateAsyncObject> Fetch(PersonDto person)
        {
            return FetchProperty(person);
        }

        public Task<IValidateAsyncObject> LocalFetch(PersonDto person)
        {
            var target = ServiceProvider.GetRequiredService<ValidateAsyncObject>();
            var portal = ServiceProvider.GetService<ValidateAsyncObjectFactory>();
            var personTable = ServiceProvider.GetService<IReadOnlyList<PersonDto>>();
            return DoMapperMethodCallAsync<IValidateAsyncObject>(target, DataMapperMethod.Fetch, () => target.Fetch(person, portal, personTable));
        }

        public virtual async Task<IValidateAsyncObject?> RemoteFetch(PersonDto person)
        {
            return await DoRemoteRequest.ForDelegate<ValidateAsyncObject?>(typeof(IValidateAsyncObjectFactory.FetchDelegate), [person]);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<ValidateAsyncObject>();
            services.AddTransient<IValidateAsyncObject, ValidateAsyncObject>();
            services.AddScoped<ValidateAsyncObjectFactory>();
            services.AddScoped<IValidateAsyncObjectFactory, ValidateAsyncObjectFactory>();
            services.AddScoped<IValidateAsyncObjectFactory.FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<ValidateAsyncObjectFactory>();
                return (PersonDto person) => factory.LocalFetch(person);
            });
        }
    }
}