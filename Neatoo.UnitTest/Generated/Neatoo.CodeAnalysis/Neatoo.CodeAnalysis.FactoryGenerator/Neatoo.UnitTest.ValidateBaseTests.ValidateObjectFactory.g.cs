using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
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
    }

    [Factory<IValidateObject>]
    internal class ValidateObjectFactory : FactoryBase<ValidateObject>, IValidateObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IValidateObject> FetchDelegate(PersonDto person);
        protected FetchDelegate FetchProperty { get; }

        public ValidateObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            FetchProperty = LocalFetch;
        }

        public ValidateObjectFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            FetchProperty = RemoteFetch;
        }

        public Task<IValidateObject> Fetch(PersonDto person)
        {
            return FetchProperty(person);
        }

        [Local<FetchDelegate>]
        protected async Task<IValidateObject> LocalFetch(PersonDto person)
        {
            var target = ServiceProvider.GetRequiredService<ValidateObject>();
            var portal = ServiceProvider.GetService<ValidateObjectFactory>();
            var personTable = ServiceProvider.GetService<IReadOnlyList<PersonDto>>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () => target.Fetch(person, portal, personTable));
            return target;
        }

        protected async Task<IValidateObject?> RemoteFetch(PersonDto person)
        {
            return (IValidateObject? )await DoRemoteRequest(typeof(FetchDelegate), [person]);
        }
    }
}