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
: Horse<LightHorse>, ILightHorse
: CustomEditBase<H>, IHorse
No DataMapperMethod attribute for Create
: EditBase<T>
No DataMapperMethod attribute for HandleIdPropertyChanged
*/
namespace HorseBarn.lib.Horse
{
    public interface ILightHorseFactory
    {
        ILightHorse Create(IHorseCriteria criteria);
        ILightHorse Fetch(Dal.Ef.Horse horse);
        Task<ILightHorse?> Save(ILightHorse target, Dal.Ef.Pasture pasture);
        Task<ILightHorse?> Save(ILightHorse target, Dal.Ef.Cart cart);
    }

    internal class LightHorseFactory : FactoryEditBase<LightHorse>, IFactoryEditBase<LightHorse>, ILightHorseFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public LightHorseFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public LightHorseFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public virtual ILightHorse Create(IHorseCriteria criteria)
        {
            return LocalCreate(criteria);
        }

        public ILightHorse LocalCreate(IHorseCriteria criteria)
        {
            var target = ServiceProvider.GetRequiredService<LightHorse>();
            return DoMapperMethodCall<ILightHorse>(target, DataMapperMethod.Create, () => target.Create(criteria));
        }

        public virtual ILightHorse Fetch(Dal.Ef.Horse horse)
        {
            return LocalFetch(horse);
        }

        public ILightHorse LocalFetch(Dal.Ef.Horse horse)
        {
            var target = ServiceProvider.GetRequiredService<LightHorse>();
            return DoMapperMethodCall<ILightHorse>(target, DataMapperMethod.Fetch, () => target.Fetch(horse));
        }

        public virtual ILightHorse? LocalInsert(ILightHorse target, Dal.Ef.Pasture pasture)
        {
            var cTarget = (LightHorse)target ?? throw new Exception("LightHorse must implement ILightHorse");
            return DoMapperMethodCall<ILightHorse>(cTarget, DataMapperMethod.Insert, () => cTarget.Insert(pasture));
        }

        public virtual Task<ILightHorse?> LocalUpdate(ILightHorse target, Dal.Ef.Pasture pasture)
        {
            var cTarget = (LightHorse)target ?? throw new Exception("LightHorse must implement ILightHorse");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            return DoMapperMethodCallAsync<ILightHorse>(cTarget, DataMapperMethod.Update, () => cTarget.Update(pasture, horseBarnContext));
        }

        public virtual ILightHorse? LocalDelete1(ILightHorse target, Dal.Ef.Pasture pasture)
        {
            var cTarget = (LightHorse)target ?? throw new Exception("LightHorse must implement ILightHorse");
            return DoMapperMethodCall<ILightHorse>(cTarget, DataMapperMethod.Delete, () => cTarget.Delete(pasture));
        }

        public virtual ILightHorse? LocalInsert1(ILightHorse target, Dal.Ef.Cart cart)
        {
            var cTarget = (LightHorse)target ?? throw new Exception("LightHorse must implement ILightHorse");
            return DoMapperMethodCall<ILightHorse>(cTarget, DataMapperMethod.Insert, () => cTarget.Insert(cart));
        }

        public virtual Task<ILightHorse?> LocalUpdate1(ILightHorse target, Dal.Ef.Cart cart)
        {
            var cTarget = (LightHorse)target ?? throw new Exception("LightHorse must implement ILightHorse");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            return DoMapperMethodCallAsync<ILightHorse>(cTarget, DataMapperMethod.Update, () => cTarget.Update(cart, horseBarnContext));
        }

        public virtual ILightHorse? LocalDelete(ILightHorse target, Dal.Ef.Cart cart)
        {
            var cTarget = (LightHorse)target ?? throw new Exception("LightHorse must implement ILightHorse");
            return DoMapperMethodCall<ILightHorse>(cTarget, DataMapperMethod.Delete, () => cTarget.Delete(cart));
        }

        public virtual Task<ILightHorse?> Save(ILightHorse target, Dal.Ef.Pasture pasture)
        {
            return LocalSave(target, pasture);
        }

        public virtual Task<ILightHorse?> LocalSave(ILightHorse target, Dal.Ef.Pasture pasture)
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

        public virtual Task<ILightHorse?> Save(ILightHorse target, Dal.Ef.Cart cart)
        {
            return LocalSave1(target, cart);
        }

        public virtual Task<ILightHorse?> LocalSave1(ILightHorse target, Dal.Ef.Cart cart)
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
            services.AddTransient<LightHorse>();
            services.AddScoped<LightHorseFactory>();
            services.AddScoped<ILightHorseFactory, LightHorseFactory>();
            services.AddTransient<ILightHorse, LightHorse>();
            services.AddScoped<IFactoryEditBase<LightHorse>, LightHorseFactory>();
        }
    }
}