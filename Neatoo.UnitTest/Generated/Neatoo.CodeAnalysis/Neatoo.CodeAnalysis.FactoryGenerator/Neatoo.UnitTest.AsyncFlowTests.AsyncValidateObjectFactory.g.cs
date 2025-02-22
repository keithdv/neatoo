using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Core;
using Neatoo.Rules;
using System;
using System.Threading;
using System.Threading.Tasks;

/*
Debugging Messages:
: ValidateBase<AsyncValidateObject>
*/
namespace Neatoo.UnitTest.AsyncFlowTests
{
    public interface IAsyncValidateObjectFactory
    {
    }

    internal class AsyncValidateObjectFactory : FactoryBase, IAsyncValidateObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public AsyncValidateObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public AsyncValidateObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<AsyncValidateObject>();
            services.AddScoped<AsyncValidateObjectFactory>();
            services.AddScoped<IAsyncValidateObjectFactory, AsyncValidateObjectFactory>();
        }
    }
}