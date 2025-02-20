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
: Cart<RacingChariot, ILightHorse>, IRacingChariot
: CustomEditBase<C>, ICart
: EditBase<T>
*/
namespace HorseBarn.lib.Cart
{
    public interface IRacingChariotFactory
    {
        Task<IRacingChariot> Create();
    }

    [Factory<IRacingChariot>]
    internal class RacingChariotFactory : FactoryEditBase<RacingChariot>, IRacingChariotFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IRacingChariot> CreateDelegate();
        protected CreateDelegate CreateProperty { get; }

        public RacingChariotFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateProperty = LocalCreate;
        }

        public RacingChariotFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateProperty = RemoteCreate;
        }

        public Task<IRacingChariot> Create()
        {
            return CreateProperty();
        }

        [Local<CreateDelegate>]
        protected async Task<IRacingChariot> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<RacingChariot>();
            var horsePortal = ServiceProvider.GetService<HorseListFactory>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () => target.Create(horsePortal));
            return target;
        }

        protected async Task<IRacingChariot?> RemoteCreate()
        {
            return (IRacingChariot? )await DoRemoteRequest(typeof(CreateDelegate), []);
        }
    }
}