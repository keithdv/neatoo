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
using HorseBarn.Dal.Ef;

/*
Debugging Messages:
: Cart<RacingChariot, ILightHorse>, IRacingChariot
: CustomEditBase<C>, ICart
No DataMapperMethod attribute for RemoveHorse
No DataMapperMethod attribute for AddHorse
No DataMapperMethod attribute for CanAddHorse
: EditBase<T>
No DataMapperMethod attribute for HandleIdPropertyChanged
*/
namespace HorseBarn.lib.Cart
{
    public interface IRacingChariotFactory
    {
        Task<IRacingChariot> Create();
        IRacingChariot Fetch(Dal.Ef.Cart cart);
        IRacingChariot? Save(IRacingChariot target, Dal.Ef.HorseBarn horseBarn);
    }

    internal class RacingChariotFactory : FactoryEditBase<RacingChariot>, IFactoryEditBase<RacingChariot>, IRacingChariotFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public RacingChariotFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public RacingChariotFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public virtual Task<IRacingChariot> Create()
        {
            return LocalCreate();
        }

        public Task<IRacingChariot> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<RacingChariot>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            var allRequiredRulesExecutedFactory = ServiceProvider.GetService<IAllRequiredRulesExecuted.Factory>();
            return DoMapperMethodCallAsync<IRacingChariot>(target, DataMapperMethod.Create, () => target.Create(horsePortal, allRequiredRulesExecutedFactory));
        }

        public virtual IRacingChariot Fetch(Dal.Ef.Cart cart)
        {
            return LocalFetch(cart);
        }

        public IRacingChariot LocalFetch(Dal.Ef.Cart cart)
        {
            var target = ServiceProvider.GetRequiredService<RacingChariot>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IRacingChariot>(target, DataMapperMethod.Fetch, () => target.Fetch(cart, horsePortal));
        }

        public virtual IRacingChariot? LocalInsert(IRacingChariot target, Dal.Ef.HorseBarn horseBarn)
        {
            var cTarget = (RacingChariot)target ?? throw new Exception("RacingChariot must implement IRacingChariot");
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IRacingChariot>(cTarget, DataMapperMethod.Insert, () => cTarget.Insert(horseBarn, horsePortal));
        }

        public virtual IRacingChariot? LocalUpdate(IRacingChariot target, Dal.Ef.HorseBarn horseBarn)
        {
            var cTarget = (RacingChariot)target ?? throw new Exception("RacingChariot must implement IRacingChariot");
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IRacingChariot>(cTarget, DataMapperMethod.Update, () => cTarget.Update(horseBarn, horsePortal));
        }

        public virtual IRacingChariot? Save(IRacingChariot target, Dal.Ef.HorseBarn horseBarn)
        {
            return LocalSave(target, horseBarn);
        }

        public virtual IRacingChariot? LocalSave(IRacingChariot target, Dal.Ef.HorseBarn horseBarn)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return LocalInsert(target, horseBarn);
            }
            else
            {
                return LocalUpdate(target, horseBarn);
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<RacingChariot>();
            services.AddScoped<RacingChariotFactory>();
            services.AddScoped<IRacingChariotFactory, RacingChariotFactory>();
            services.AddTransient<IRacingChariot, RacingChariot>();
            services.AddScoped<IFactoryEditBase<RacingChariot>, RacingChariotFactory>();
        }
    }
}