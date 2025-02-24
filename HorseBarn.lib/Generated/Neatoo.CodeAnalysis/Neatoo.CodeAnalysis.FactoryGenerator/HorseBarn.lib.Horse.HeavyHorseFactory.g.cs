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
: EditBase<T>
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

    internal class HeavyHorseFactory : FactoryEditBase<HeavyHorse>, IHeavyHorseFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public SaveDelegate SaveProperty { get; set; }
        public Save1Delegate Save1Property { get; set; }

        public delegate IHeavyHorse CreateDelegate(IHorseCriteria horseCriteria);
        public delegate IHeavyHorse FetchDelegate(Dal.Ef.Horse horse);
        public delegate Task<IHeavyHorse?> SaveDelegate(IHeavyHorse target, Dal.Ef.Pasture pasture);
        public delegate Task<IHeavyHorse?> Save1Delegate(IHeavyHorse target, Dal.Ef.Cart cart);
        public HeavyHorseFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            SaveProperty = LocalSave;
            Save1Property = LocalSave1;
        }

        public HeavyHorseFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            SaveProperty = RemoteSave;
            Save1Property = RemoteSave1;
        }

        public Task<IHeavyHorse?> Save(IHeavyHorse target, Dal.Ef.Pasture pasture)
        {
            return SaveProperty(target, pasture);
        }

        public Task<IHeavyHorse?> Save(IHeavyHorse target, Dal.Ef.Cart cart)
        {
            return Save1Property(target, cart);
        }

        public IHeavyHorse Create(IHorseCriteria horseCriteria)
        {
            var target = ServiceProvider.GetRequiredService<HeavyHorse>();
            return DoMapperMethodCall<IHeavyHorse>(target, DataMapperMethod.Create, () => target.Create(horseCriteria));
        }

        public IHeavyHorse Fetch(Dal.Ef.Horse horse)
        {
            var target = ServiceProvider.GetRequiredService<HeavyHorse>();
            return DoMapperMethodCall<IHeavyHorse>(target, DataMapperMethod.Fetch, () => target.Fetch(horse));
        }

        public virtual Task<IHeavyHorse?> LocalInsert(IHeavyHorse itarget, Dal.Ef.Pasture pasture)
        {
            var target = (HeavyHorse)itarget ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            return DoMapperMethodCallAsync<IHeavyHorse>(target, DataMapperMethod.Insert, () => target.Insert(pasture));
        }

        public virtual Task<IHeavyHorse?> LocalInsert1(IHeavyHorse itarget, Dal.Ef.Cart cart)
        {
            var target = (HeavyHorse)itarget ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            return DoMapperMethodCallAsync<IHeavyHorse>(target, DataMapperMethod.Insert, () => target.Insert(cart));
        }

        public virtual Task<IHeavyHorse?> LocalUpdate(IHeavyHorse itarget, Dal.Ef.Pasture pasture)
        {
            var target = (HeavyHorse)itarget ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            return DoMapperMethodCallAsync<IHeavyHorse>(target, DataMapperMethod.Update, () => target.Update(pasture, horseBarnContext));
        }

        public virtual Task<IHeavyHorse?> LocalUpdate1(IHeavyHorse itarget, Dal.Ef.Cart cart)
        {
            var target = (HeavyHorse)itarget ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            return DoMapperMethodCallAsync<IHeavyHorse>(target, DataMapperMethod.Update, () => target.Update(cart, horseBarnContext));
        }

        public virtual Task<IHeavyHorse?> LocalDelete(IHeavyHorse itarget, Dal.Ef.Cart cart)
        {
            var target = (HeavyHorse)itarget ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            return DoMapperMethodCallAsync<IHeavyHorse>(target, DataMapperMethod.Delete, () => target.Delete(cart));
        }

        public virtual Task<IHeavyHorse?> LocalDelete1(IHeavyHorse itarget, Dal.Ef.Pasture pasture)
        {
            var target = (HeavyHorse)itarget ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            return DoMapperMethodCallAsync<IHeavyHorse>(target, DataMapperMethod.Delete, () => target.Delete(pasture));
        }

        public virtual async Task<IHeavyHorse?> LocalSave(IHeavyHorse target, Dal.Ef.Pasture pasture)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return LocalDelete1(target, pasture);
            }
            else if (target.IsNew)
            {
                return LocalInsert(target, pasture);
            }
            else
            {
                return await LocalUpdate(target, pasture);
            }
        }

        public async Task<IHeavyHorse?> RemoteSave(IHeavyHorse target, Dal.Ef.Pasture pasture)
        {
            return await DoRemoteRequest.ForDelegate<HeavyHorse?>(typeof(SaveDelegate), [target, pasture]);
        }

        public virtual async Task<IHeavyHorse?> LocalSave1(IHeavyHorse target, Dal.Ef.Cart cart)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return LocalDelete(target, cart);
            }
            else if (target.IsNew)
            {
                return LocalInsert1(target, cart);
            }
            else
            {
                return await LocalUpdate1(target, cart);
            }
        }

        public async Task<IHeavyHorse?> RemoteSave1(IHeavyHorse target, Dal.Ef.Cart cart)
        {
            return await DoRemoteRequest.ForDelegate<HeavyHorse?>(typeof(Save1Delegate), [target, cart]);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<HeavyHorse>();
            services.AddScoped<HeavyHorseFactory>();
            services.AddScoped<IHeavyHorseFactory, HeavyHorseFactory>();
            services.AddTransient<IHeavyHorse, HeavyHorse>();
            services.AddScoped<CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HeavyHorseFactory>();
                return (IHorseCriteria horseCriteria) => factory.Create(horseCriteria);
            });
            services.AddScoped<FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HeavyHorseFactory>();
                return (Dal.Ef.Horse horse) => factory.Fetch(horse);
            });
            services.AddScoped<SaveDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HeavyHorseFactory>();
                return (target, pasture) => factory.LocalSave(target, pasture);
            });
            services.AddScoped<Save1Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<HeavyHorseFactory>();
                return (target, cart) => factory.LocalSave1(target, cart);
            });
            services.AddScoped<IFactoryEditBase<HeavyHorse>, HeavyHorseFactory>();
        }
    }
}