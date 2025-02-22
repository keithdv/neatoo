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
        delegate Task<IRacingChariot> CreateDelegate();
    }

    internal class RacingChariotFactory : FactoryEditBase<RacingChariot>, IRacingChariotFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IRacingChariotFactory.CreateDelegate CreateProperty { get; }

        public RacingChariotFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateProperty = LocalCreate;
        }

        public RacingChariotFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateProperty = RemoteCreate;
        }

        public virtual Task<IRacingChariot> Create()
        {
            return CreateProperty();
        }

        public Task<IRacingChariot> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<RacingChariot>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            var allRequiredRulesExecutedFactory = ServiceProvider.GetService<IAllRequiredRulesExecuted.Factory>();
            return DoMapperMethodCallAsync<IRacingChariot>(target, DataMapperMethod.Create, () => target.Create(horsePortal, allRequiredRulesExecutedFactory));
        }

        public virtual async Task<IRacingChariot?> RemoteCreate()
        {
            return await DoRemoteRequest.ForDelegate<RacingChariot?>(typeof(IRacingChariotFactory.CreateDelegate), []);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<RacingChariot>();
            services.AddTransient<IRacingChariot, RacingChariot>();
            services.AddScoped<RacingChariotFactory>();
            services.AddScoped<IRacingChariotFactory, RacingChariotFactory>();
            services.AddScoped<IRacingChariotFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<RacingChariotFactory>();
                return () => factory.LocalCreate();
            });
            services.AddScoped<IFactoryEditBase<RacingChariot>, RacingChariotFactory>();
        }
    }
}