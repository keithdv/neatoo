using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using HorseBarn.Dal.Ef;
using HorseBarn.lib.Cart;
using Neatoo;
using Neatoo.Portal;
using System.Collections.Specialized;

/*
Debugging Messages:
: EditListBase<HorseList, IHorse>, IHorseList
*/
namespace HorseBarn.lib.Horse
{
    public interface IHorseListFactory
    {
        Task<IHorseList> Create();
        Task<IHorseList> Fetch(ICollection<Dal.Ef.Horse> horses);
        Task<IHorseList?> Save(IHorseList target, Dal.Ef.Cart cart);
        Task<IHorseList?> Save(IHorseList target, Dal.Ef.Pasture pasture);
    }

    [Factory<IHorseList>]
    internal class HorseListFactory : FactoryBase<HorseList>, IHorseListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IHorseList> FetchDelegate(ICollection<Dal.Ef.Horse> horses);
        protected internal delegate Task<IHorseList?> SaveDelegate(IHorseList target, Dal.Ef.Cart cart);
        protected internal delegate Task<IHorseList?> Save1Delegate(IHorseList target, Dal.Ef.Pasture pasture);
        protected FetchDelegate FetchProperty { get; }
        protected SaveDelegate SaveProperty { get; set; }
        protected Save1Delegate Save1Property { get; set; }

        public HorseListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            FetchProperty = LocalFetch;
            SaveProperty = LocalSave;
            Save1Property = LocalSave1;
        }

        public HorseListFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            FetchProperty = RemoteFetch;
            SaveProperty = RemoteSave;
            Save1Property = RemoteSave1;
        }

        public Task<IHorseList> Fetch(ICollection<Dal.Ef.Horse> horses)
        {
            return FetchProperty(horses);
        }

        public Task<IHorseList?> Save(IHorseList target, Dal.Ef.Cart cart)
        {
            return SaveProperty(target, cart);
        }

        public Task<IHorseList?> Save(IHorseList target, Dal.Ef.Pasture pasture)
        {
            return Save1Property(target, pasture);
        }

        public async Task<IHorseList> Create()
        {
            var target = ServiceProvider.GetRequiredService<HorseList>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create();
                return Task.CompletedTask;
            });
            return target;
        }

        [Local<FetchDelegate>]
        protected async Task<IHorseList> LocalFetch(ICollection<Dal.Ef.Horse> horses)
        {
            var target = ServiceProvider.GetRequiredService<HorseList>();
            var lightHorsePortal = ServiceProvider.GetService<LightHorseFactory>();
            var heavyHorsePortal = ServiceProvider.GetService<HeavyHorseFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () => target.Fetch(horses, lightHorsePortal, heavyHorsePortal));
            return target;
        }

        protected async Task LocalUpdate(IHorseList itarget, Dal.Ef.Cart cart)
        {
            var target = (HorseList)itarget ?? throw new Exception("HorseList must implement IHorseList");
            var lightHorsePortal = ServiceProvider.GetService<LightHorseFactory>();
            var heavyHorsePortal = ServiceProvider.GetService<HeavyHorseFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(cart, lightHorsePortal, heavyHorsePortal));
        }

        protected async Task LocalUpdate1(IHorseList itarget, Dal.Ef.Pasture pasture)
        {
            var target = (HorseList)itarget ?? throw new Exception("HorseList must implement IHorseList");
            var lightHorsePortal = ServiceProvider.GetService<LightHorseFactory>();
            var heavyHorsePortal = ServiceProvider.GetService<HeavyHorseFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(pasture, lightHorsePortal, heavyHorsePortal));
        }

        protected async Task<IHorseList?> RemoteFetch(ICollection<Dal.Ef.Horse> horses)
        {
            return (IHorseList? )await DoRemoteRequest(typeof(FetchDelegate), [horses]);
        }

        [Local<SaveDelegate>]
        protected async Task<IHorseList?> LocalSave(IHorseList target, Dal.Ef.Cart cart)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException("HorseListFactory.Update()");
            }
            else if (target.IsNew)
            {
                throw new NotImplementedException("HorseListFactory.Update()");
            }
            else
            {
                await LocalUpdate(target, cart);
            }

            return target;
        }

        protected async Task<IHorseList?> RemoteSave(IHorseList target, Dal.Ef.Cart cart)
        {
            return (IHorseList? )await DoRemoteRequest(typeof(SaveDelegate), [target, cart]);
        }

        [Local<Save1Delegate>]
        protected async Task<IHorseList?> LocalSave1(IHorseList target, Dal.Ef.Pasture pasture)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException("HorseListFactory.Update()");
            }
            else if (target.IsNew)
            {
                throw new NotImplementedException("HorseListFactory.Update()");
            }
            else
            {
                await LocalUpdate1(target, pasture);
            }

            return target;
        }

        protected async Task<IHorseList?> RemoteSave1(IHorseList target, Dal.Ef.Pasture pasture)
        {
            return (IHorseList? )await DoRemoteRequest(typeof(Save1Delegate), [target, pasture]);
        }
    }
}