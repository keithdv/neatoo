using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.lib.Horse;
using Neatoo.Rules.Rules;
using Neatoo.Rules;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.ComponentModel;

/*
Debugging Messages:
: Cart<Wagon, IHeavyHorse>, IWagon
: CustomEditBase<C>, ICart
No DataMapperMethod attribute for RemoveHorse
No DataMapperMethod attribute for AddHorse
No DataMapperMethod attribute for CanAddHorse
: EditBase<T>
*/
namespace HorseBarn.lib.Cart
{
    public interface IWagonFactory
    {
        Task<IWagon> Create();
    }

    internal class WagonFactory : FactoryBase, IWagonFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public WagonFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public WagonFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public virtual Task<IWagon> Create()
        {
            return LocalCreate();
        }

        public Task<IWagon> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<Wagon>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            var allRequiredRulesExecutedFactory = ServiceProvider.GetService<IAllRequiredRulesExecuted.Factory>();
            return DoMapperMethodCallAsync<IWagon>(target, DataMapperMethod.Create, () => target.Create(horsePortal, allRequiredRulesExecutedFactory));
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<Wagon>();
            services.AddScoped<WagonFactory>();
            services.AddScoped<IWagonFactory, WagonFactory>();
            services.AddTransient<IWagon, Wagon>();
        }
    }
}