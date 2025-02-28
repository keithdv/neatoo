using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.Dal.Ef;
using HorseBarn.lib.Horse;

/*
Debugging Messages:
: EditListBase<CartList, ICart>, ICartList
No DataMapperMethod attribute for RemoveHorse
*/
namespace HorseBarn.lib.Cart
{
    public interface ICartListFactory
    {
        ICartList Create();
        ICartList Fetch(ICollection<Dal.Ef.Cart> carts);
        ICartList? Save(ICartList target, Dal.Ef.HorseBarn horseBarn);
    }

    internal class CartListFactory : FactoryEditBase<CartList>, IFactoryEditBase<CartList>, ICartListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public CartListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public CartListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public virtual ICartList Create()
        {
            return LocalCreate();
        }

        public ICartList LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<CartList>();
            return DoMapperMethodCall<ICartList>(target, DataMapperMethod.Create, () => target.Create());
        }

        public virtual ICartList Fetch(ICollection<Dal.Ef.Cart> carts)
        {
            return LocalFetch(carts);
        }

        public ICartList LocalFetch(ICollection<Dal.Ef.Cart> carts)
        {
            var target = ServiceProvider.GetRequiredService<CartList>();
            var racingChariotPortal = ServiceProvider.GetService<RacingChariotFactory>();
            var wagonPortal = ServiceProvider.GetService<WagonFactory>();
            return DoMapperMethodCall<ICartList>(target, DataMapperMethod.Fetch, () => target.Fetch(carts, racingChariotPortal, wagonPortal));
        }

        public virtual ICartList? LocalUpdate(ICartList target, Dal.Ef.HorseBarn horseBarn)
        {
            var cTarget = (CartList)target ?? throw new Exception("CartList must implement ICartList");
            var racingChariotPortal = ServiceProvider.GetService<RacingChariotFactory>();
            var wagonPortal = ServiceProvider.GetService<WagonFactory>();
            return DoMapperMethodCall<ICartList>(cTarget, DataMapperMethod.Update, () => cTarget.Update(horseBarn, racingChariotPortal, wagonPortal));
        }

        public virtual ICartList? Save(ICartList target, Dal.Ef.HorseBarn horseBarn)
        {
            return LocalSave(target, horseBarn);
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
            services.AddScoped<IFactoryEditBase<CartList>, CartListFactory>();
        }
    }
}