using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules;
using System.ComponentModel.DataAnnotations;
using HorseBarn.Dal.Ef;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;

/*
Debugging Messages:
: Horse<HeavyHorse>, IHeavyHorse
: CustomEditBase<H>, IHorse
No DataMapperMethod attribute for Create
: EditBase<T>
No DataMapperMethod attribute for HandleIdPropertyChanged
*/
namespace HorseBarn.lib.Horse
{
    public interface IHeavyHorseFactory
    {
        IHeavyHorse Create(IHorseCriteria horseCriteria);
        IHeavyHorse Fetch(Dal.Ef.Horse horse);
        Task<IHeavyHorse?> Save(IHeavyHorse target, Dal.Ef.Pasture pasture);
        Task<IHeavyHorse?> Save(IHeavyHorse target, Dal.Ef.Cart cart);
    }

    internal class HeavyHorseFactory : FactoryEditBase<HeavyHorse>, IFactoryEditBase<HeavyHorse>, IHeavyHorseFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public HeavyHorseFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public HeavyHorseFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public virtual IHeavyHorse Create(IHorseCriteria horseCriteria)
        {
            return LocalCreate(horseCriteria);
        }

        public IHeavyHorse LocalCreate(IHorseCriteria horseCriteria)
        {
            var target = ServiceProvider.GetRequiredService<HeavyHorse>();
            return DoMapperMethodCall<IHeavyHorse>(target, DataMapperMethod.Create, () => target.Create(horseCriteria));
        }

        public virtual IHeavyHorse Fetch(Dal.Ef.Horse horse)
        {
            return LocalFetch(horse);
        }

        public IHeavyHorse LocalFetch(Dal.Ef.Horse horse)
        {
            var target = ServiceProvider.GetRequiredService<HeavyHorse>();
            return DoMapperMethodCall<IHeavyHorse>(target, DataMapperMethod.Fetch, () => target.Fetch(horse));
        }

        public virtual IHeavyHorse? LocalInsert(IHeavyHorse target, Dal.Ef.Pasture pasture)
        {
            var cTarget = (HeavyHorse)target ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            return DoMapperMethodCall<IHeavyHorse>(cTarget, DataMapperMethod.Insert, () => cTarget.Insert(pasture));
        }

        public virtual Task<IHeavyHorse?> LocalUpdate(IHeavyHorse target, Dal.Ef.Pasture pasture)
        {
            var cTarget = (HeavyHorse)target ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            return DoMapperMethodCallAsync<IHeavyHorse>(cTarget, DataMapperMethod.Update, () => cTarget.Update(pasture, horseBarnContext));
        }

        public virtual IHeavyHorse? LocalDelete1(IHeavyHorse target, Dal.Ef.Pasture pasture)
        {
            var cTarget = (HeavyHorse)target ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            return DoMapperMethodCall<IHeavyHorse>(cTarget, DataMapperMethod.Delete, () => cTarget.Delete(pasture));
        }

        public virtual IHeavyHorse? LocalInsert1(IHeavyHorse target, Dal.Ef.Cart cart)
        {
            var cTarget = (HeavyHorse)target ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            return DoMapperMethodCall<IHeavyHorse>(cTarget, DataMapperMethod.Insert, () => cTarget.Insert(cart));
        }

        public virtual Task<IHeavyHorse?> LocalUpdate1(IHeavyHorse target, Dal.Ef.Cart cart)
        {
            var cTarget = (HeavyHorse)target ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            return DoMapperMethodCallAsync<IHeavyHorse>(cTarget, DataMapperMethod.Update, () => cTarget.Update(cart, horseBarnContext));
        }

        public virtual IHeavyHorse? LocalDelete(IHeavyHorse target, Dal.Ef.Cart cart)
        {
            var cTarget = (HeavyHorse)target ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            return DoMapperMethodCall<IHeavyHorse>(cTarget, DataMapperMethod.Delete, () => cTarget.Delete(cart));
        }

        public virtual Task<IHeavyHorse?> Save(IHeavyHorse target, Dal.Ef.Pasture pasture)
        {
            return LocalSave(target, pasture);
        }

        public virtual Task<IHeavyHorse?> LocalSave(IHeavyHorse target, Dal.Ef.Pasture pasture)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return Task.FromResult(LocalDelete1(target, pasture));
            }
            else if (target.IsNew)
            {
                return Task.FromResult(LocalInsert(target, pasture));
            }
            else
            {
                return LocalUpdate(target, pasture);
            }
        }

        public virtual Task<IHeavyHorse?> Save(IHeavyHorse target, Dal.Ef.Cart cart)
        {
            return LocalSave1(target, cart);
        }

        public virtual Task<IHeavyHorse?> LocalSave1(IHeavyHorse target, Dal.Ef.Cart cart)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return Task.FromResult(LocalDelete(target, cart));
            }
            else if (target.IsNew)
            {
                return Task.FromResult(LocalInsert1(target, cart));
            }
            else
            {
                return LocalUpdate1(target, cart);
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<HeavyHorse>();
            services.AddScoped<HeavyHorseFactory>();
            services.AddScoped<IHeavyHorseFactory, HeavyHorseFactory>();
            services.AddTransient<IHeavyHorse, HeavyHorse>();
            services.AddScoped<IFactoryEditBase<HeavyHorse>, HeavyHorseFactory>();
        }
    }
}