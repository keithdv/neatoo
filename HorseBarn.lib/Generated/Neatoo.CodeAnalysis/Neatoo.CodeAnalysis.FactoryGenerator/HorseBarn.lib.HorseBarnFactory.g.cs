using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using Microsoft.EntityFrameworkCore;
using HorseBarn.Dal.Ef;
using System.ComponentModel;
using System.Diagnostics;

/*
Debugging Messages:
: CustomEditBase<HorseBarn>, IHorseBarn
: EditBase<T>
*/
namespace HorseBarn.lib
{
    public interface IHorseBarnFactory
    {
        Task<IHorseBarn> Create();
        Task<IHorseBarn> Fetch();
        Task<IHorseBarn?> Save(IHorseBarn target);
    }

    [Factory<IHorseBarn>]
    internal class HorseBarnFactory : FactoryEditBase<HorseBarn>, IHorseBarnFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IHorseBarn> CreateDelegate();
        protected internal delegate Task<IHorseBarn> FetchDelegate();
        protected internal delegate Task<IHorseBarn?> SaveDelegate(IHorseBarn target);
        protected CreateDelegate CreateProperty { get; }
        protected FetchDelegate FetchProperty { get; }
        protected SaveDelegate SaveProperty { get; set; }

        public HorseBarnFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateProperty = LocalCreate;
            FetchProperty = LocalFetch;
            SaveProperty = LocalSave;
        }

        public HorseBarnFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateProperty = RemoteCreate;
            FetchProperty = RemoteFetch;
            SaveProperty = RemoteSave;
        }

        public Task<IHorseBarn> Create()
        {
            return CreateProperty();
        }

        public Task<IHorseBarn> Fetch()
        {
            return FetchProperty();
        }

        public override async Task<IEditBase?> Save(HorseBarn target)
        {
            return await SaveProperty(target);
        }

        public Task<IHorseBarn?> Save(IHorseBarn target)
        {
            return SaveProperty(target);
        }

        [Local<CreateDelegate>]
        protected async Task<IHorseBarn> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<HorseBarn>();
            var pasturePortal = ServiceProvider.GetService<IPastureFactory>();
            var cartListPortal = ServiceProvider.GetService<ICartListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () => target.Create(pasturePortal, cartListPortal));
            return target;
        }

        [Local<FetchDelegate>]
        protected async Task<IHorseBarn> LocalFetch()
        {
            var target = ServiceProvider.GetRequiredService<HorseBarn>();
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            var pasturePortal = ServiceProvider.GetService<IPastureFactory>();
            var cartPortal = ServiceProvider.GetService<ICartListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () => target.Fetch(horseBarnContext, pasturePortal, cartPortal));
            return target;
        }

        protected async Task LocalInsert(IHorseBarn itarget)
        {
            var target = (HorseBarn)itarget ?? throw new Exception("HorseBarn must implement IHorseBarn");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            var pasturePortal = ServiceProvider.GetService<IPastureFactory>();
            var cartPortal = ServiceProvider.GetService<ICartListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Insert, () => target.Insert(horseBarnContext, pasturePortal, cartPortal));
        }

        protected async Task LocalUpdate(IHorseBarn itarget)
        {
            var target = (HorseBarn)itarget ?? throw new Exception("HorseBarn must implement IHorseBarn");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            var pasturePortal = ServiceProvider.GetService<IPastureFactory>();
            var cartPortal = ServiceProvider.GetService<ICartListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(horseBarnContext, pasturePortal, cartPortal));
        }

        protected async Task<IHorseBarn?> RemoteCreate()
        {
            return (IHorseBarn? )await DoRemoteRequest(typeof(CreateDelegate), []);
        }

        protected async Task<IHorseBarn?> RemoteFetch()
        {
            return (IHorseBarn? )await DoRemoteRequest(typeof(FetchDelegate), []);
        }

        [Local<SaveDelegate>]
        protected async Task<IHorseBarn?> LocalSave(IHorseBarn target)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException("HorseBarnFactory.Update()");
            }
            else if (target.IsNew)
            {
                await LocalInsert(target);
            }
            else
            {
                await LocalUpdate(target);
            }

            return target;
        }

        protected async Task<IHorseBarn?> RemoteSave(IHorseBarn target)
        {
            return (IHorseBarn? )await DoRemoteRequest(typeof(SaveDelegate), [target, ]);
        }
    }
}