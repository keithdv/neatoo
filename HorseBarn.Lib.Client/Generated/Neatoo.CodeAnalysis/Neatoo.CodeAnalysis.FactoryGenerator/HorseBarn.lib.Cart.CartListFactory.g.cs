using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
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
    }

    [Factory<ICartList>]
    internal class CartListFactory : FactoryBase<CartList>, ICartListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        public CartListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public CartListFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
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
    }
}