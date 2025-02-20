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
        Task<IHeavyHorse> Create(IHorseCriteria horseCriteria);
        Task<IHeavyHorse> Fetch(Dal.Ef.Horse horse);
        Task<IHeavyHorse?> Save(IHeavyHorse target, Dal.Ef.Pasture pasture);
        Task<IHeavyHorse?> Save(IHeavyHorse target, Dal.Ef.Cart cart);
    }

    [Factory<IHeavyHorse>]
    internal class HeavyHorseFactory : FactoryEditBase<HeavyHorse>, IHeavyHorseFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IHeavyHorse?> SaveDelegate(IHeavyHorse target, Dal.Ef.Pasture pasture);
        protected internal delegate Task<IHeavyHorse?> Save1Delegate(IHeavyHorse target, Dal.Ef.Cart cart);
        protected SaveDelegate SaveProperty { get; set; }
        protected Save1Delegate Save1Property { get; set; }

        public HeavyHorseFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            SaveProperty = LocalSave;
            Save1Property = LocalSave1;
        }

        public HeavyHorseFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
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

        public async Task<IHeavyHorse> Create(IHorseCriteria horseCriteria)
        {
            var target = ServiceProvider.GetRequiredService<HeavyHorse>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create(horseCriteria);
                return Task.CompletedTask;
            });
            return target;
        }

        public async Task<IHeavyHorse> Fetch(Dal.Ef.Horse horse)
        {
            var target = ServiceProvider.GetRequiredService<HeavyHorse>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () =>
            {
                target.Fetch(horse);
                return Task.CompletedTask;
            });
            return target;
        }

        protected async Task LocalInsert(IHeavyHorse itarget, Dal.Ef.Pasture pasture)
        {
            var target = (HeavyHorse)itarget ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            await DoMapperMethodCall(target, DataMapperMethod.Insert, () =>
            {
                target.Insert(pasture);
                return Task.CompletedTask;
            });
        }

        protected async Task LocalInsert1(IHeavyHorse itarget, Dal.Ef.Cart cart)
        {
            var target = (HeavyHorse)itarget ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            await DoMapperMethodCall(target, DataMapperMethod.Insert, () =>
            {
                target.Insert(cart);
                return Task.CompletedTask;
            });
        }

        protected async Task LocalUpdate(IHeavyHorse itarget, Dal.Ef.Pasture pasture)
        {
            var target = (HeavyHorse)itarget ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(pasture, horseBarnContext));
        }

        protected async Task LocalUpdate1(IHeavyHorse itarget, Dal.Ef.Cart cart)
        {
            var target = (HeavyHorse)itarget ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(cart, horseBarnContext));
        }

        protected async Task LocalDelete(IHeavyHorse itarget, Dal.Ef.Cart cart)
        {
            var target = (HeavyHorse)itarget ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            await DoMapperMethodCall(target, DataMapperMethod.Delete, () =>
            {
                target.Delete(cart);
                return Task.CompletedTask;
            });
        }

        protected async Task LocalDelete1(IHeavyHorse itarget, Dal.Ef.Pasture pasture)
        {
            var target = (HeavyHorse)itarget ?? throw new Exception("HeavyHorse must implement IHeavyHorse");
            await DoMapperMethodCall(target, DataMapperMethod.Delete, () =>
            {
                target.Delete(pasture);
                return Task.CompletedTask;
            });
        }

        [Local<SaveDelegate>]
        protected async Task<IHeavyHorse?> LocalSave(IHeavyHorse target, Dal.Ef.Pasture pasture)
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

        protected async Task<IHeavyHorse?> RemoteSave(IHeavyHorse target, Dal.Ef.Pasture pasture)
        {
            return (IHeavyHorse? )await DoRemoteRequest(typeof(SaveDelegate), [target, pasture]);
        }

        [Local<Save1Delegate>]
        protected async Task<IHeavyHorse?> LocalSave1(IHeavyHorse target, Dal.Ef.Cart cart)
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

        protected async Task<IHeavyHorse?> RemoteSave1(IHeavyHorse target, Dal.Ef.Cart cart)
        {
            return (IHeavyHorse? )await DoRemoteRequest(typeof(Save1Delegate), [target, cart]);
        }
    }
}