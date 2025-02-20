using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.ComponentModel;
using HorseBarn.Dal.Ef;

/*
Debugging Messages:
: Cart<Wagon, IHeavyHorse>, IWagon
: CustomEditBase<C>, ICart
: EditBase<T>
*/
namespace HorseBarn.lib.Cart
{
    public interface IWagonFactory
    {
        Task<IWagon> Create();
        Task<IWagon> Fetch(Dal.Ef.Cart cart);
        Task<IWagon?> Save(IWagon target, Dal.Ef.HorseBarn horseBarn);
    }

    [Factory<IWagon>]
    internal class WagonFactory : FactoryEditBase<Wagon>, IWagonFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IWagon> CreateDelegate();
        protected internal delegate Task<IWagon> FetchDelegate(Dal.Ef.Cart cart);
        protected internal delegate Task<IWagon?> SaveDelegate(IWagon target, Dal.Ef.HorseBarn horseBarn);
        protected CreateDelegate CreateProperty { get; }
        protected FetchDelegate FetchProperty { get; }
        protected SaveDelegate SaveProperty { get; set; }

        public WagonFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateProperty = LocalCreate;
            FetchProperty = LocalFetch;
            SaveProperty = LocalSave;
        }

        public WagonFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateProperty = RemoteCreate;
            FetchProperty = RemoteFetch;
            SaveProperty = RemoteSave;
        }

        public Task<IWagon> Create()
        {
            return CreateProperty();
        }

        public Task<IWagon> Fetch(Dal.Ef.Cart cart)
        {
            return FetchProperty(cart);
        }

        public Task<IWagon?> Save(IWagon target, Dal.Ef.HorseBarn horseBarn)
        {
            return SaveProperty(target, horseBarn);
        }

        [Local<CreateDelegate>]
        protected async Task<IWagon> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<Wagon>();
            var horsePortal = ServiceProvider.GetService<HorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () => target.Create(horsePortal));
            return target;
        }

        [Local<FetchDelegate>]
        protected async Task<IWagon> LocalFetch(Dal.Ef.Cart cart)
        {
            var target = ServiceProvider.GetRequiredService<Wagon>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () => target.Fetch(cart, horsePortal));
            return target;
        }

        protected async Task LocalInsert(IWagon itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (Wagon)itarget ?? throw new Exception("Wagon must implement IWagon");
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Insert, () => target.Insert(horseBarn, horsePortal));
        }

        protected async Task LocalUpdate(IWagon itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (Wagon)itarget ?? throw new Exception("Wagon must implement IWagon");
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(horseBarn, horsePortal));
        }

        protected async Task<IWagon?> RemoteCreate()
        {
            return (IWagon? )await DoRemoteRequest(typeof(CreateDelegate), []);
        }

        protected async Task<IWagon?> RemoteFetch(Dal.Ef.Cart cart)
        {
            return (IWagon? )await DoRemoteRequest(typeof(FetchDelegate), [cart]);
        }

        [Local<SaveDelegate>]
        protected async Task<IWagon?> LocalSave(IWagon target, Dal.Ef.HorseBarn horseBarn)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException("WagonFactory.Update()");
            }
            else if (target.IsNew)
            {
                await LocalInsert(target, horseBarn);
            }
            else
            {
                await LocalUpdate(target, horseBarn);
            }

            return target;
        }

        protected async Task<IWagon?> RemoteSave(IWagon target, Dal.Ef.HorseBarn horseBarn)
        {
            return (IWagon? )await DoRemoteRequest(typeof(SaveDelegate), [target, horseBarn]);
        }
    }
}