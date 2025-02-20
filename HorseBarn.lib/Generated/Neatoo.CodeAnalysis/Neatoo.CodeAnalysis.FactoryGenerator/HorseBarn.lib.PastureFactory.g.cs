using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using System.Diagnostics;
using HorseBarn.Dal.Ef;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

/*
Debugging Messages:
: CustomEditBase<Pasture>, IPasture
: EditBase<T>
*/
namespace HorseBarn.lib
{
    public interface IPastureFactory
    {
        Task<IPasture> Create();
        Task<IPasture> Fetch(Dal.Ef.Pasture pasture);
        Task<IPasture?> Save(IPasture target, Dal.Ef.HorseBarn horseBarn);
    }

    [Factory<IPasture>]
    internal class PastureFactory : FactoryEditBase<Pasture>, IPastureFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IPasture> CreateDelegate();
        protected internal delegate Task<IPasture> FetchDelegate(Dal.Ef.Pasture pasture);
        protected internal delegate Task<IPasture?> SaveDelegate(IPasture target, Dal.Ef.HorseBarn horseBarn);
        protected CreateDelegate CreateProperty { get; }
        protected FetchDelegate FetchProperty { get; }
        protected SaveDelegate SaveProperty { get; set; }

        public PastureFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateProperty = LocalCreate;
            FetchProperty = LocalFetch;
            SaveProperty = LocalSave;
        }

        public PastureFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateProperty = RemoteCreate;
            FetchProperty = RemoteFetch;
            SaveProperty = RemoteSave;
        }

        public Task<IPasture> Create()
        {
            return CreateProperty();
        }

        public Task<IPasture> Fetch(Dal.Ef.Pasture pasture)
        {
            return FetchProperty(pasture);
        }

        public Task<IPasture?> Save(IPasture target, Dal.Ef.HorseBarn horseBarn)
        {
            return SaveProperty(target, horseBarn);
        }

        [Local<CreateDelegate>]
        protected async Task<IPasture> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<Pasture>();
            var horseListPortal = ServiceProvider.GetService<HorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () => target.Create(horseListPortal));
            return target;
        }

        [Local<FetchDelegate>]
        protected async Task<IPasture> LocalFetch(Dal.Ef.Pasture pasture)
        {
            var target = ServiceProvider.GetRequiredService<Pasture>();
            var horseListPortal = ServiceProvider.GetService<HorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () => target.Fetch(pasture, horseListPortal));
            return target;
        }

        protected async Task LocalInsert(IPasture itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (Pasture)itarget ?? throw new Exception("Pasture must implement IPasture");
            var horseListPortal = ServiceProvider.GetService<HorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Insert, () => target.Insert(horseBarn, horseListPortal));
        }

        protected async Task LocalUpdate(IPasture itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (Pasture)itarget ?? throw new Exception("Pasture must implement IPasture");
            var horseListPortal = ServiceProvider.GetService<HorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(horseBarn, horseListPortal));
        }

        protected async Task<IPasture?> RemoteCreate()
        {
            return (IPasture? )await DoRemoteRequest(typeof(CreateDelegate), []);
        }

        protected async Task<IPasture?> RemoteFetch(Dal.Ef.Pasture pasture)
        {
            return (IPasture? )await DoRemoteRequest(typeof(FetchDelegate), [pasture]);
        }

        [Local<SaveDelegate>]
        protected async Task<IPasture?> LocalSave(IPasture target, Dal.Ef.HorseBarn horseBarn)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException("PastureFactory.Update()");
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

        protected async Task<IPasture?> RemoteSave(IPasture target, Dal.Ef.HorseBarn horseBarn)
        {
            return (IPasture? )await DoRemoteRequest(typeof(SaveDelegate), [target, horseBarn]);
        }
    }
}