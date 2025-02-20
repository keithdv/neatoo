using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo.Portal;
using Neatoo.Rules;
using System;
using System.Collections.Generic;

/*
Debugging Messages:
: ValidateBase<ValidateObject>, IValidateObject
*/
namespace Neatoo.UnitTest.SystemTextJson
{
    public interface IValidateObjectFactory
    {
        Task<IValidateObject> Create(Guid ID, string Name);
    }

    [Factory<IValidateObject>]
    internal class ValidateObjectFactory : FactoryBase<ValidateObject>, IValidateObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        public ValidateObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public ValidateObjectFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public async Task<IValidateObject> Create(Guid ID, string Name)
        {
            var target = ServiceProvider.GetRequiredService<ValidateObject>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create(ID, Name);
                return Task.CompletedTask;
            });
            return target;
        }
    }
}