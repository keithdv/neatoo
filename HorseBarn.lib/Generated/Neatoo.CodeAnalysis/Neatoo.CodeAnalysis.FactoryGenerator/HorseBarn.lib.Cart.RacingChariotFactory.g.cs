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
: Cart<RacingChariot, ILightHorse>, IRacingChariot
: CustomEditBase<C>, ICart
: EditBase<T>
*/
namespace HorseBarn.lib.Cart
{
    public interface IRacingChariotFactory
    {
        Task<IRacingChariot> Create();
        Task<IRacingChariot> Fetch(Dal.Ef.Cart cart);
        Task<IRacingChariot?> Save(IRacingChariot target, Dal.Ef.HorseBarn horseBarn);
    }

    [Factory<IRacingChariot>]
    internal class RacingChariotFactory : FactoryEditBase<RacingChariot>, IRacingChariotFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IRacingChariot> CreateDelegate();
        protected internal delegate Task<IRacingChariot> FetchDelegate(Dal.Ef.Cart cart);
        protected internal delegate Task<IRacingChariot?> SaveDelegate(IRacingChariot target, Dal.Ef.HorseBarn horseBarn);
        protected CreateDelegate CreateProperty { get; }
        protected FetchDelegate FetchProperty { get; }
        protected SaveDelegate SaveProperty { get; set; }

        public RacingChariotFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateProperty = LocalCreate;
            FetchProperty = LocalFetch;
            SaveProperty = LocalSave;
        }

        public RacingChariotFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateProperty = RemoteCreate;
            FetchProperty = RemoteFetch;
            SaveProperty = RemoteSave;
        }

        public Task<IRacingChariot> Create()
        {
            return CreateProperty();
        }

        public Task<IRacingChariot> Fetch(Dal.Ef.Cart cart)
        {
            return FetchProperty(cart);
        }

        public Task<IRacingChariot?> Save(IRacingChariot target, Dal.Ef.HorseBarn horseBarn)
        {
            return SaveProperty(target, horseBarn);
        }

        [Local<CreateDelegate>]
        protected async Task<IRacingChariot> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<RacingChariot>();
            var horsePortal = ServiceProvider.GetService<HorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () => target.Create(horsePortal));
            return target;
        }

        [Local<FetchDelegate>]
        protected async Task<IRacingChariot> LocalFetch(Dal.Ef.Cart cart)
        {
            var target = ServiceProvider.GetRequiredService<RacingChariot>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () => target.Fetch(cart, horsePortal));
            return target;
        }

        protected async Task LocalInsert(IRacingChariot itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (RacingChariot)itarget ?? throw new Exception("RacingChariot must implement IRacingChariot");
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Insert, () => target.Insert(horseBarn, horsePortal));
        }

        protected async Task LocalUpdate(IRacingChariot itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (RacingChariot)itarget ?? throw new Exception("RacingChariot must implement IRacingChariot");
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(horseBarn, horsePortal));
        }

        protected async Task<IRacingChariot?> RemoteCreate()
        {
            return (IRacingChariot? )await DoRemoteRequest(typeof(CreateDelegate), []);
        }

        protected async Task<IRacingChariot?> RemoteFetch(Dal.Ef.Cart cart)
        {
            return (IRacingChariot? )await DoRemoteRequest(typeof(FetchDelegate), [cart]);
        }

        [Local<SaveDelegate>]
        protected async Task<IRacingChariot?> LocalSave(IRacingChariot target, Dal.Ef.HorseBarn horseBarn)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException("RacingChariotFactory.Update()");
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

        protected async Task<IRacingChariot?> RemoteSave(IRacingChariot target, Dal.Ef.HorseBarn horseBarn)
        {
            return (IRacingChariot? )await DoRemoteRequest(typeof(SaveDelegate), [target, horseBarn]);
        }
    }
}