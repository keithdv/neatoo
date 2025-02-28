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
: Cart<Wagon, IHeavyHorse>, IWagon
: CustomEditBase<C>, ICart
No DataMapperMethod attribute for RemoveHorse
No DataMapperMethod attribute for AddHorse
No DataMapperMethod attribute for CanAddHorse
: EditBase<T>
No DataMapperMethod attribute for HandleIdPropertyChanged
*/
namespace HorseBarn.lib.Cart
{
    public interface IWagonFactory
    {
        Task<IWagon> Create();
        IWagon Fetch(Dal.Ef.Cart cart);
        IWagon? Save(IWagon target, Dal.Ef.HorseBarn horseBarn);
    }

    internal class WagonFactory : FactoryEditBase<Wagon>, IFactoryEditBase<Wagon>, IWagonFactory
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

        public virtual IWagon Fetch(Dal.Ef.Cart cart)
        {
            return LocalFetch(cart);
        }

        public IWagon LocalFetch(Dal.Ef.Cart cart)
        {
            var target = ServiceProvider.GetRequiredService<Wagon>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IWagon>(target, DataMapperMethod.Fetch, () => target.Fetch(cart, horsePortal));
        }

        public virtual IWagon? LocalInsert(IWagon target, Dal.Ef.HorseBarn horseBarn)
        {
            var cTarget = (Wagon)target ?? throw new Exception("Wagon must implement IWagon");
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IWagon>(cTarget, DataMapperMethod.Insert, () => cTarget.Insert(horseBarn, horsePortal));
        }

        public virtual IWagon? LocalUpdate(IWagon target, Dal.Ef.HorseBarn horseBarn)
        {
            var cTarget = (Wagon)target ?? throw new Exception("Wagon must implement IWagon");
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IWagon>(cTarget, DataMapperMethod.Update, () => cTarget.Update(horseBarn, horsePortal));
        }

        public virtual IWagon? Save(IWagon target, Dal.Ef.HorseBarn horseBarn)
        {
            return LocalSave(target, horseBarn);
        }

        public virtual IWagon? LocalSave(IWagon target, Dal.Ef.HorseBarn horseBarn)
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
            services.AddTransient<Wagon>();
            services.AddScoped<WagonFactory>();
            services.AddScoped<IWagonFactory, WagonFactory>();
            services.AddTransient<IWagon, Wagon>();
            services.AddScoped<IFactoryEditBase<Wagon>, WagonFactory>();
        }
    }
}