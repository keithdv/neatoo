using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using HorseBarn.Dal.Ef;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;

/*
Debugging Messages:
: EditListBase<CartList, ICart>, ICartList
*/
namespace HorseBarn.lib.Cart
{
    public interface ICartListFactory
    {
        Task<ICartList> Create();
        Task<ICartList> Fetch(ICollection<Dal.Ef.Cart> carts);
        Task<ICartList?> Save(ICartList target, Dal.Ef.HorseBarn horseBarn);
    }

    [Factory<ICartList>]
    internal class CartListFactory : FactoryBase<CartList>, ICartListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<ICartList> FetchDelegate(ICollection<Dal.Ef.Cart> carts);
        protected internal delegate Task<ICartList?> SaveDelegate(ICartList target, Dal.Ef.HorseBarn horseBarn);
        protected FetchDelegate FetchProperty { get; }
        protected SaveDelegate SaveProperty { get; set; }

        public CartListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            FetchProperty = LocalFetch;
            SaveProperty = LocalSave;
        }

        public CartListFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            FetchProperty = RemoteFetch;
            SaveProperty = RemoteSave;
        }

        public Task<ICartList> Fetch(ICollection<Dal.Ef.Cart> carts)
        {
            return FetchProperty(carts);
        }

        public Task<ICartList?> Save(ICartList target, Dal.Ef.HorseBarn horseBarn)
        {
            return SaveProperty(target, horseBarn);
        }

        public async Task<ICartList> Create()
        {
            var target = ServiceProvider.GetRequiredService<CartList>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create();
                return Task.CompletedTask;
            });
            return target;
        }

        [Local<FetchDelegate>]
        protected async Task<ICartList> LocalFetch(ICollection<Dal.Ef.Cart> carts)
        {
            var target = ServiceProvider.GetRequiredService<CartList>();
            var racingChariotPortal = ServiceProvider.GetService<RacingChariotFactory>();
            var wagonPortal = ServiceProvider.GetService<WagonFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () => target.Fetch(carts, racingChariotPortal, wagonPortal));
            return target;
        }

        protected async Task LocalUpdate(ICartList itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (CartList)itarget ?? throw new Exception("CartList must implement ICartList");
            var racingChariotPortal = ServiceProvider.GetService<RacingChariotFactory>();
            var wagonPortal = ServiceProvider.GetService<WagonFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(horseBarn, racingChariotPortal, wagonPortal));
        }

        protected async Task<ICartList?> RemoteFetch(ICollection<Dal.Ef.Cart> carts)
        {
            return (ICartList? )await DoRemoteRequest(typeof(FetchDelegate), [carts]);
        }

        [Local<SaveDelegate>]
        protected async Task<ICartList?> LocalSave(ICartList target, Dal.Ef.HorseBarn horseBarn)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException("CartListFactory.Update()");
            }
            else if (target.IsNew)
            {
                throw new NotImplementedException("CartListFactory.Update()");
            }
            else
            {
                await LocalUpdate(target, horseBarn);
            }

            return target;
        }

        protected async Task<ICartList?> RemoteSave(ICartList target, Dal.Ef.HorseBarn horseBarn)
        {
            return (ICartList? )await DoRemoteRequest(typeof(SaveDelegate), [target, horseBarn]);
        }
    }
}