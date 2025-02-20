using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
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
        Task<IHorseBarn> Fetch();
        Task<IHorseBarn> Create();
        Task<IHorseBarn?> Save(IHorseBarn target);
    }

    [Factory<IHorseBarn>]
    internal class HorseBarnFactory : FactoryEditBase<HorseBarn>, IHorseBarnFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IHorseBarn> FetchDelegate();
        protected internal delegate Task<IHorseBarn> CreateDelegate();
        protected internal delegate Task<IHorseBarn?> SaveDelegate(IHorseBarn target);
        protected FetchDelegate FetchProperty { get; }
        protected CreateDelegate CreateProperty { get; }
        protected SaveDelegate SaveProperty { get; set; }

        public HorseBarnFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            FetchProperty = LocalFetch;
            CreateProperty = LocalCreate;
            SaveProperty = LocalSave;
        }

        public HorseBarnFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            FetchProperty = RemoteFetch;
            CreateProperty = RemoteCreate;
            SaveProperty = RemoteSave;
        }

        public Task<IHorseBarn> Fetch()
        {
            return FetchProperty();
        }

        public Task<IHorseBarn> Create()
        {
            return CreateProperty();
        }

        public override async Task<IEditBase?> Save(HorseBarn target)
        {
            return await SaveProperty(target);
        }

        public Task<IHorseBarn?> Save(IHorseBarn target)
        {
            return SaveProperty(target);
        }

        [Local<FetchDelegate>]
        protected async Task<IHorseBarn> LocalFetch()
        {
            var target = ServiceProvider.GetRequiredService<HorseBarn>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () =>
            {
                target.Fetch();
                return Task.CompletedTask;
            });
            return target;
        }

        [Local<CreateDelegate>]
        protected async Task<IHorseBarn> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<HorseBarn>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create();
                return Task.CompletedTask;
            });
            return target;
        }

        protected async Task LocalUpdate(IHorseBarn itarget)
        {
            var target = (HorseBarn)itarget ?? throw new Exception("HorseBarn must implement IHorseBarn");
            await DoMapperMethodCall(target, DataMapperMethod.Update, () =>
            {
                target.Update();
                return Task.CompletedTask;
            });
        }

        protected async Task<IHorseBarn?> RemoteFetch()
        {
            return (IHorseBarn? )await DoRemoteRequest(typeof(FetchDelegate), []);
        }

        protected async Task<IHorseBarn?> RemoteCreate()
        {
            return (IHorseBarn? )await DoRemoteRequest(typeof(CreateDelegate), []);
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
                throw new NotImplementedException("HorseBarnFactory.Update()");
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