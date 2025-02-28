using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.Dal.Ef;
using HorseBarn.lib.Cart;
using System.Collections.Specialized;

/*
Debugging Messages:
: EditListBase<HorseList, IHorse>, IHorseList
No DataMapperMethod attribute for RemoveHorse
*/
namespace HorseBarn.lib.Horse
{
    public interface IHorseListFactory
    {
        IHorseList Create();
        IHorseList Fetch(ICollection<Dal.Ef.Horse> horses);
        IHorseList? Save(IHorseList target, Dal.Ef.Cart cart);
        IHorseList? Save(IHorseList target, Dal.Ef.Pasture pasture);
    }

    internal class HorseListFactory : FactoryEditBase<HorseList>, IFactoryEditBase<HorseList>, IHorseListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public HorseListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public HorseListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public virtual IHorseList Create()
        {
            return LocalCreate();
        }

        public IHorseList LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<HorseList>();
            return DoMapperMethodCall<IHorseList>(target, DataMapperMethod.Create, () => target.Create());
        }

        public virtual IHorseList Fetch(ICollection<Dal.Ef.Horse> horses)
        {
            return LocalFetch(horses);
        }

        public IHorseList LocalFetch(ICollection<Dal.Ef.Horse> horses)
        {
            var target = ServiceProvider.GetRequiredService<HorseList>();
            var lightHorsePortal = ServiceProvider.GetService<ILightHorseFactory>();
            var heavyHorsePortal = ServiceProvider.GetService<IHeavyHorseFactory>();
            return DoMapperMethodCall<IHorseList>(target, DataMapperMethod.Fetch, () => target.Fetch(horses, lightHorsePortal, heavyHorsePortal));
        }

        public virtual IHorseList? LocalUpdate(IHorseList target, Dal.Ef.Cart cart)
        {
            var cTarget = (HorseList)target ?? throw new Exception("HorseList must implement IHorseList");
            var lightHorsePortal = ServiceProvider.GetService<ILightHorseFactory>();
            var heavyHorsePortal = ServiceProvider.GetService<IHeavyHorseFactory>();
            return DoMapperMethodCall<IHorseList>(cTarget, DataMapperMethod.Update, () => cTarget.Update(cart, lightHorsePortal, heavyHorsePortal));
        }

        public virtual IHorseList? LocalUpdate1(IHorseList target, Dal.Ef.Pasture pasture)
        {
            var cTarget = (HorseList)target ?? throw new Exception("HorseList must implement IHorseList");
            var lightHorsePortal = ServiceProvider.GetService<ILightHorseFactory>();
            var heavyHorsePortal = ServiceProvider.GetService<IHeavyHorseFactory>();
            return DoMapperMethodCall<IHorseList>(cTarget, DataMapperMethod.Update, () => cTarget.Update(pasture, lightHorsePortal, heavyHorsePortal));
        }

        public virtual IHorseList? Save(IHorseList target, Dal.Ef.Cart cart)
        {
            return LocalSave(target, cart);
        }

        public virtual IHorseList? LocalSave(IHorseList target, Dal.Ef.Cart cart)
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
                return LocalUpdate(target, cart);
            }
        }

        public virtual IHorseList? Save(IHorseList target, Dal.Ef.Pasture pasture)
        {
            return LocalSave1(target, pasture);
        }

        public virtual IHorseList? LocalSave1(IHorseList target, Dal.Ef.Pasture pasture)
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
                return LocalUpdate1(target, pasture);
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<HorseList>();
            services.AddScoped<HorseListFactory>();
            services.AddScoped<IHorseListFactory, HorseListFactory>();
            services.AddTransient<IHorseList, HorseList>();
            services.AddScoped<IFactoryEditBase<HorseList>, HorseListFactory>();
        }
    }
}