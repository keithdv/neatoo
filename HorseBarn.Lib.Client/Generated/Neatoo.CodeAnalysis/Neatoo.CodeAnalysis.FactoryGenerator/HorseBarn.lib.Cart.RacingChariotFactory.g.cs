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
: Cart<RacingChariot, ILightHorse>, IRacingChariot
: CustomEditBase<C>, ICart
: EditBase<T>
*/
namespace HorseBarn.lib.Cart
{
    public interface IRacingChariotFactory
    {
        Task<IRacingChariot> Create();
    }

    internal class RacingChariotFactory : FactoryEditBase<RacingChariot>, IRacingChariotFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public delegate Task<IRacingChariot> CreateDelegate();
        public RacingChariotFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public RacingChariotFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public Task<IRacingChariot> Create()
        {
            var target = ServiceProvider.GetRequiredService<RacingChariot>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            var allRequiredRulesExecutedFactory = ServiceProvider.GetService<IAllRequiredRulesExecuted.Factory>();
            return DoMapperMethodCallAsync<IRacingChariot>(target, DataMapperMethod.Create, () => target.Create(horsePortal, allRequiredRulesExecutedFactory));
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<RacingChariot>();
            services.AddScoped<RacingChariotFactory>();
            services.AddScoped<IRacingChariotFactory, RacingChariotFactory>();
            services.AddTransient<IRacingChariot, RacingChariot>();
            services.AddScoped<CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<RacingChariotFactory>();
                return () => factory.Create();
            });
            services.AddScoped<IFactoryEditBase<RacingChariot>, RacingChariotFactory>();
        }
    }
}