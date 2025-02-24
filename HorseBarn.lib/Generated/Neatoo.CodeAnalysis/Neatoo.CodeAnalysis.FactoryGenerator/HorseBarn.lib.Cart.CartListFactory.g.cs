using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.Dal.Ef;
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
        ICartList Fetch(ICollection<Dal.Ef.Cart> carts);
        ICartList? Save(ICartList target, Dal.Ef.HorseBarn horseBarn);
    }

    internal class CartListFactory : FactoryBase, ICartListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public SaveDelegate SaveProperty { get; set; }

        public delegate ICartList CreateDelegate();
        public delegate ICartList FetchDelegate(ICollection<Dal.Ef.Cart> carts);
        public delegate ICartList? SaveDelegate(ICartList target, Dal.Ef.HorseBarn horseBarn);
        public CartListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            SaveProperty = LocalSave;
        }

        public CartListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public ICartList? Save(ICartList target, Dal.Ef.HorseBarn horseBarn)
        {
            return SaveProperty(target, horseBarn);
        }

        public ICartList Create()
        {
            var target = ServiceProvider.GetRequiredService<CartList>();
            return DoMapperMethodCall<ICartList>(target, DataMapperMethod.Create, () => target.Create());
        }

        public ICartList Fetch(ICollection<Dal.Ef.Cart> carts)
        {
            var target = ServiceProvider.GetRequiredService<CartList>();
            var racingChariotPortal = ServiceProvider.GetService<RacingChariotFactory>();
            var wagonPortal = ServiceProvider.GetService<WagonFactory>();
            return DoMapperMethodCall<ICartList>(target, DataMapperMethod.Fetch, () => target.Fetch(carts, racingChariotPortal, wagonPortal));
        }

        public virtual Task<ICartList?> LocalUpdate(ICartList itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (CartList)itarget ?? throw new Exception("CartList must implement ICartList");
            var racingChariotPortal = ServiceProvider.GetService<RacingChariotFactory>();
            var wagonPortal = ServiceProvider.GetService<WagonFactory>();
            return DoMapperMethodCallAsync<ICartList>(target, DataMapperMethod.Update, () => target.Update(horseBarn, racingChariotPortal, wagonPortal));
        }

        public virtual ICartList? LocalSave(ICartList target, Dal.Ef.HorseBarn horseBarn)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                throw new NotImplementedException();
            }
            else
            {
                return LocalUpdate(target, horseBarn);
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<CartList>();
            services.AddScoped<CartListFactory>();
            services.AddScoped<ICartListFactory, CartListFactory>();
            services.AddTransient<ICartList, CartList>();
            services.AddScoped<CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<CartListFactory>();
                return () => factory.Create();
            });
            services.AddScoped<FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<CartListFactory>();
                return (ICollection<Dal.Ef.Cart> carts) => factory.Fetch(carts);
            });
        }
    }
}