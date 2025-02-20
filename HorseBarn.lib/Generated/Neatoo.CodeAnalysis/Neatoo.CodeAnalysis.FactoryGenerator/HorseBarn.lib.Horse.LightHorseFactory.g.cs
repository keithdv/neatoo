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
        Task<ILightHorse> Create(IHorseCriteria criteria);
        Task<ILightHorse> Fetch(Dal.Ef.Horse horse);
        Task<ILightHorse?> Save(ILightHorse target, Dal.Ef.Pasture pasture);
        Task<ILightHorse?> Save(ILightHorse target, Dal.Ef.Cart cart);
    }

    [Factory<ILightHorse>]
    internal class LightHorseFactory : FactoryEditBase<LightHorse>, ILightHorseFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<ILightHorse?> SaveDelegate(ILightHorse target, Dal.Ef.Pasture pasture);
        protected internal delegate Task<ILightHorse?> Save1Delegate(ILightHorse target, Dal.Ef.Cart cart);
        protected SaveDelegate SaveProperty { get; set; }
        protected Save1Delegate Save1Property { get; set; }

        public LightHorseFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            SaveProperty = LocalSave;
            Save1Property = LocalSave1;
        }

        public LightHorseFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
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

        public async Task<ILightHorse> Create(IHorseCriteria criteria)
        {
            var target = ServiceProvider.GetRequiredService<LightHorse>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create(criteria);
                return Task.CompletedTask;
            });
            return target;
        }

        public async Task<ILightHorse> Fetch(Dal.Ef.Horse horse)
        {
            var target = ServiceProvider.GetRequiredService<LightHorse>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () =>
            {
                target.Fetch(horse);
                return Task.CompletedTask;
            });
            return target;
        }

        protected async Task LocalInsert(ILightHorse itarget, Dal.Ef.Pasture pasture)
        {
            var target = (LightHorse)itarget ?? throw new Exception("LightHorse must implement ILightHorse");
            await DoMapperMethodCall(target, DataMapperMethod.Insert, () =>
            {
                target.Insert(pasture);
                return Task.CompletedTask;
            });
        }

        protected async Task LocalInsert1(ILightHorse itarget, Dal.Ef.Cart cart)
        {
            var target = (LightHorse)itarget ?? throw new Exception("LightHorse must implement ILightHorse");
            await DoMapperMethodCall(target, DataMapperMethod.Insert, () =>
            {
                target.Insert(cart);
                return Task.CompletedTask;
            });
        }

        protected async Task LocalUpdate(ILightHorse itarget, Dal.Ef.Pasture pasture)
        {
            var target = (LightHorse)itarget ?? throw new Exception("LightHorse must implement ILightHorse");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(pasture, horseBarnContext));
        }

        protected async Task LocalUpdate1(ILightHorse itarget, Dal.Ef.Cart cart)
        {
            var target = (LightHorse)itarget ?? throw new Exception("LightHorse must implement ILightHorse");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(cart, horseBarnContext));
        }

        protected async Task LocalDelete(ILightHorse itarget, Dal.Ef.Cart cart)
        {
            var target = (LightHorse)itarget ?? throw new Exception("LightHorse must implement ILightHorse");
            await DoMapperMethodCall(target, DataMapperMethod.Delete, () =>
            {
                target.Delete(cart);
                return Task.CompletedTask;
            });
        }

        protected async Task LocalDelete1(ILightHorse itarget, Dal.Ef.Pasture pasture)
        {
            var target = (LightHorse)itarget ?? throw new Exception("LightHorse must implement ILightHorse");
            await DoMapperMethodCall(target, DataMapperMethod.Delete, () =>
            {
                target.Delete(pasture);
                return Task.CompletedTask;
            });
        }

        [Local<SaveDelegate>]
        protected async Task<ILightHorse?> LocalSave(ILightHorse target, Dal.Ef.Pasture pasture)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                await LocalDelete1(target, pasture);
            }
            else if (target.IsNew)
            {
                await LocalInsert(target, pasture);
            }
            else
            {
                await LocalUpdate(target, pasture);
            }

            return target;
        }

        protected async Task<ILightHorse?> RemoteSave(ILightHorse target, Dal.Ef.Pasture pasture)
        {
            return (ILightHorse? )await DoRemoteRequest(typeof(SaveDelegate), [target, pasture]);
        }

        [Local<Save1Delegate>]
        protected async Task<ILightHorse?> LocalSave1(ILightHorse target, Dal.Ef.Cart cart)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                await LocalDelete(target, cart);
            }
            else if (target.IsNew)
            {
                await LocalInsert1(target, cart);
            }
            else
            {
                await LocalUpdate1(target, cart);
            }

            return target;
        }

        protected async Task<ILightHorse?> RemoteSave1(ILightHorse target, Dal.Ef.Cart cart)
        {
            return (ILightHorse? )await DoRemoteRequest(typeof(Save1Delegate), [target, cart]);
        }
    }
}