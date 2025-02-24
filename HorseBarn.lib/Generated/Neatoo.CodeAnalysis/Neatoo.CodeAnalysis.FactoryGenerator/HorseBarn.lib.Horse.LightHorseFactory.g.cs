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
: EditBase<T>
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

    internal class LightHorseFactory : FactoryEditBase<LightHorse>, ILightHorseFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public SaveDelegate SaveProperty { get; set; }
        public Save1Delegate Save1Property { get; set; }

        public delegate ILightHorse CreateDelegate(IHorseCriteria criteria);
        public delegate ILightHorse FetchDelegate(Dal.Ef.Horse horse);
        public delegate Task<ILightHorse?> SaveDelegate(ILightHorse target, Dal.Ef.Pasture pasture);
        public delegate Task<ILightHorse?> Save1Delegate(ILightHorse target, Dal.Ef.Cart cart);
        public LightHorseFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            SaveProperty = LocalSave;
            Save1Property = LocalSave1;
        }

        public LightHorseFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            SaveProperty = RemoteSave;
            Save1Property = RemoteSave1;
        }

        public Task<ILightHorse?> Save(ILightHorse target, Dal.Ef.Pasture pasture)
        {
            return SaveProperty(target, pasture);
        }

        public Task<ILightHorse?> Save(ILightHorse target, Dal.Ef.Cart cart)
        {
            return Save1Property(target, cart);
        }

        public ILightHorse Create(IHorseCriteria criteria)
        {
            var target = ServiceProvider.GetRequiredService<LightHorse>();
            return DoMapperMethodCall<ILightHorse>(target, DataMapperMethod.Create, () => target.Create(criteria));
        }

        public ILightHorse Fetch(Dal.Ef.Horse horse)
        {
            var target = ServiceProvider.GetRequiredService<LightHorse>();
            return DoMapperMethodCall<ILightHorse>(target, DataMapperMethod.Fetch, () => target.Fetch(horse));
        }

        public virtual Task<ILightHorse?> LocalInsert(ILightHorse itarget, Dal.Ef.Pasture pasture)
        {
            var target = (LightHorse)itarget ?? throw new Exception("LightHorse must implement ILightHorse");
            return DoMapperMethodCallAsync<ILightHorse>(target, DataMapperMethod.Insert, () => target.Insert(pasture));
        }

        public virtual Task<ILightHorse?> LocalInsert1(ILightHorse itarget, Dal.Ef.Cart cart)
        {
            var target = (LightHorse)itarget ?? throw new Exception("LightHorse must implement ILightHorse");
            return DoMapperMethodCallAsync<ILightHorse>(target, DataMapperMethod.Insert, () => target.Insert(cart));
        }

        public virtual Task<ILightHorse?> LocalUpdate(ILightHorse itarget, Dal.Ef.Pasture pasture)
        {
            var target = (LightHorse)itarget ?? throw new Exception("LightHorse must implement ILightHorse");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            return DoMapperMethodCallAsync<ILightHorse>(target, DataMapperMethod.Update, () => target.Update(pasture, horseBarnContext));
        }

        public virtual Task<ILightHorse?> LocalUpdate1(ILightHorse itarget, Dal.Ef.Cart cart)
        {
            var target = (LightHorse)itarget ?? throw new Exception("LightHorse must implement ILightHorse");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            return DoMapperMethodCallAsync<ILightHorse>(target, DataMapperMethod.Update, () => target.Update(cart, horseBarnContext));
        }

        public virtual Task<ILightHorse?> LocalDelete(ILightHorse itarget, Dal.Ef.Cart cart)
        {
            var target = (LightHorse)itarget ?? throw new Exception("LightHorse must implement ILightHorse");
            return DoMapperMethodCallAsync<ILightHorse>(target, DataMapperMethod.Delete, () => target.Delete(cart));
        }

        public virtual Task<ILightHorse?> LocalDelete1(ILightHorse itarget, Dal.Ef.Pasture pasture)
        {
            var target = (LightHorse)itarget ?? throw new Exception("LightHorse must implement ILightHorse");
            return DoMapperMethodCallAsync<ILightHorse>(target, DataMapperMethod.Delete, () => target.Delete(pasture));
        }

        public virtual async Task<ILightHorse?> LocalSave(ILightHorse target, Dal.Ef.Pasture pasture)
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

        public async Task<ILightHorse?> RemoteSave(ILightHorse target, Dal.Ef.Pasture pasture)
        {
            return await DoRemoteRequest.ForDelegate<LightHorse?>(typeof(SaveDelegate), [target, pasture]);
        }

        public virtual async Task<ILightHorse?> LocalSave1(ILightHorse target, Dal.Ef.Cart cart)
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

        public async Task<ILightHorse?> RemoteSave1(ILightHorse target, Dal.Ef.Cart cart)
        {
            return await DoRemoteRequest.ForDelegate<LightHorse?>(typeof(Save1Delegate), [target, cart]);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<LightHorse>();
            services.AddScoped<LightHorseFactory>();
            services.AddScoped<ILightHorseFactory, LightHorseFactory>();
            services.AddTransient<ILightHorse, LightHorse>();
            services.AddScoped<CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<LightHorseFactory>();
                return (IHorseCriteria criteria) => factory.Create(criteria);
            });
            services.AddScoped<FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<LightHorseFactory>();
                return (Dal.Ef.Horse horse) => factory.Fetch(horse);
            });
            services.AddScoped<SaveDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<LightHorseFactory>();
                return (target, pasture) => factory.LocalSave(target, pasture);
            });
            services.AddScoped<Save1Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<LightHorseFactory>();
                return (target, cart) => factory.LocalSave1(target, cart);
            });
            services.AddScoped<IFactoryEditBase<LightHorse>, LightHorseFactory>();
        }
    }
}