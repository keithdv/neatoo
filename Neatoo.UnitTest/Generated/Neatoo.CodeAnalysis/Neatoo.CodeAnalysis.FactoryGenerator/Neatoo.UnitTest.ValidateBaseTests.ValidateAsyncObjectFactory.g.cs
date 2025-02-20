using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo.Core;
using Neatoo.Portal;
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
    }

    [Factory<IValidateAsyncObject>]
    internal class ValidateAsyncObjectFactory : FactoryBase<ValidateAsyncObject>, IValidateAsyncObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IValidateAsyncObject> FetchDelegate(PersonDto person);
        protected FetchDelegate FetchProperty { get; }

        public ValidateAsyncObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            FetchProperty = LocalFetch;
        }

        public ValidateAsyncObjectFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            FetchProperty = RemoteFetch;
        }

        public Task<IValidateAsyncObject> Fetch(PersonDto person)
        {
            return FetchProperty(person);
        }

        [Local<FetchDelegate>]
        protected async Task<IValidateAsyncObject> LocalFetch(PersonDto person)
        {
            var target = ServiceProvider.GetRequiredService<ValidateAsyncObject>();
            var portal = ServiceProvider.GetService<ValidateAsyncObjectFactory>();
            var personTable = ServiceProvider.GetService<IReadOnlyList<PersonDto>>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () => target.Fetch(person, portal, personTable));
            return target;
        }

        protected async Task<IValidateAsyncObject?> RemoteFetch(PersonDto person)
        {
            return (IValidateAsyncObject? )await DoRemoteRequest(typeof(FetchDelegate), [person]);
        }
    }
}