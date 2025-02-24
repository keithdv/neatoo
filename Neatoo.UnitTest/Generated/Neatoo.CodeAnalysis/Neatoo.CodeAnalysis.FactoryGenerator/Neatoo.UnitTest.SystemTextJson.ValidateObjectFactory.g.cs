using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules;
using System;
using System.Collections.Generic;

/*
Debugging Messages:
: ValidateBase<ValidateObject>, IValidateObject
No DataMapperMethod attribute for MarkInvalid
*/
namespace Neatoo.UnitTest.SystemTextJson
{
    public interface IValidateObjectFactory
    {
        IValidateObject Create(Guid ID, string Name);
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

        public IValidateObject Create(Guid ID, string Name)
        {
            var target = ServiceProvider.GetRequiredService<ValidateObject>();
            return DoMapperMethodCall<IValidateObject>(target, DataMapperMethod.Create, () => target.Create(ID, Name));
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