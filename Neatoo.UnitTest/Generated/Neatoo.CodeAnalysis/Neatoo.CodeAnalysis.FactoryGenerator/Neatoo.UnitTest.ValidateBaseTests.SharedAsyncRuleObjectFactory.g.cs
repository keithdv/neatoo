using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Rules;
using Neatoo.UnitTest.PersonObjects;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

/*
Debugging Messages:
: PersonValidateBase<SharedAsyncRuleObject>, ISharedAsyncRuleObject, ISharedShortNameRuleTarget
: ValidateBase<T>, IPersonBase
No DataMapperMethod attribute for FillFromDto
*/
namespace Neatoo.UnitTest.ValidateBaseTests
{
    public interface ISharedAsyncRuleObjectFactory
    {
    }

    internal class SharedAsyncRuleObjectFactory : FactoryBase, ISharedAsyncRuleObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public SharedAsyncRuleObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public SharedAsyncRuleObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<SharedAsyncRuleObject>();
            services.AddScoped<SharedAsyncRuleObjectFactory>();
            services.AddScoped<ISharedAsyncRuleObjectFactory, SharedAsyncRuleObjectFactory>();
            services.AddTransient<ISharedAsyncRuleObject, SharedAsyncRuleObject>();
        }
    }
}