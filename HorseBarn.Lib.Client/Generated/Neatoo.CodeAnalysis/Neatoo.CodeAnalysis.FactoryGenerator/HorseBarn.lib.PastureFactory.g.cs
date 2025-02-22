using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.lib.Horse;
using System.Diagnostics;
using System.ComponentModel;

/*
Debugging Messages:
: CustomEditBase<Pasture>, IPasture
: EditBase<T>
*/
namespace HorseBarn.lib
{
    public interface IPastureFactory
    {
    }

    internal class PastureFactory : FactoryEditBase<Pasture>, IPastureFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public PastureFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public PastureFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<Pasture>();
            services.AddTransient<IPasture, Pasture>();
            services.AddScoped<PastureFactory>();
            services.AddScoped<IPastureFactory, PastureFactory>();
            services.AddScoped<IFactoryEditBase<Pasture>, PastureFactory>();
        }
    }
}