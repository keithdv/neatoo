using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.lib.Horse;

/*
Debugging Messages:
: EditListBase<CartList, ICart>, ICartList
*/
namespace HorseBarn.lib.Cart
{
    public interface ICartListFactory
    {
        ICartList Create();
        delegate ICartList CreateDelegate();
    }

    internal class CartListFactory : FactoryBase, ICartListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public CartListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public CartListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public ICartList Create()
        {
            var target = ServiceProvider.GetRequiredService<CartList>();
            return DoMapperMethodCall<ICartList>(target, DataMapperMethod.Create, () => target.Create());
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<CartList>();
            services.AddTransient<ICartList, CartList>();
            services.AddScoped<CartListFactory>();
            services.AddScoped<ICartListFactory, CartListFactory>();
            services.AddScoped<ICartListFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<CartListFactory>();
                return () => factory.Create();
            });
        }
    }
}