using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.ComponentModel;

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
    }

    [Factory<IWagon>]
    internal class WagonFactory : FactoryEditBase<Wagon>, IWagonFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IWagon> CreateDelegate();
        protected CreateDelegate CreateProperty { get; }

        public WagonFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateProperty = LocalCreate;
        }

        public WagonFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateProperty = RemoteCreate;
        }

        public Task<IWagon> Create()
        {
            return CreateProperty();
        }

        [Local<CreateDelegate>]
        protected async Task<IWagon> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<Wagon>();
            var horsePortal = ServiceProvider.GetService<HorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () => target.Create(horsePortal));
            return target;
        }

        protected async Task<IWagon?> RemoteCreate()
        {
            return (IWagon? )await DoRemoteRequest(typeof(CreateDelegate), []);
        }
    }
}