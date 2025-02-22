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
*/
namespace HorseBarn.lib.Horse
{
    public interface IHorseListFactory
    {
        IHorseList Create();
        IHorseList Fetch(ICollection<Dal.Ef.Horse> horses);
        IHorseList? Save(IHorseList target, Dal.Ef.Cart cart);
        IHorseList? Save(IHorseList target, Dal.Ef.Pasture pasture);
        delegate IHorseList CreateDelegate();
        delegate IHorseList FetchDelegate(ICollection<Dal.Ef.Horse> horses);
        delegate IHorseList? SaveDelegate(IHorseList target, Dal.Ef.Cart cart);
        delegate IHorseList? Save1Delegate(IHorseList target, Dal.Ef.Pasture pasture);
    }

    internal class HorseListFactory : FactoryBase, IHorseListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IHorseListFactory.SaveDelegate SaveProperty { get; set; }
        public IHorseListFactory.Save1Delegate Save1Property { get; set; }

        public HorseListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            SaveProperty = LocalSave;
            Save1Property = LocalSave1;
        }

        public HorseListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public IHorseList? Save(IHorseList target, Dal.Ef.Cart cart)
        {
            return SaveProperty(target, cart);
        }

        public IHorseList? Save(IHorseList target, Dal.Ef.Pasture pasture)
        {
            return Save1Property(target, pasture);
        }

        public IHorseList Create()
        {
            var target = ServiceProvider.GetRequiredService<HorseList>();
            return DoMapperMethodCall<IHorseList>(target, DataMapperMethod.Create, () => target.Create());
        }

        public IHorseList Fetch(ICollection<Dal.Ef.Horse> horses)
        {
            var target = ServiceProvider.GetRequiredService<HorseList>();
            var lightHorsePortal = ServiceProvider.GetService<ILightHorseFactory>();
            var heavyHorsePortal = ServiceProvider.GetService<IHeavyHorseFactory>();
            return DoMapperMethodCall<IHorseList>(target, DataMapperMethod.Fetch, () => target.Fetch(horses, lightHorsePortal, heavyHorsePortal));
        }

        public virtual IHorseList? LocalUpdate(IHorseList itarget, Dal.Ef.Cart cart)
        {
            var target = (HorseList)itarget ?? throw new Exception("HorseList must implement IHorseList");
            var lightHorsePortal = ServiceProvider.GetService<ILightHorseFactory>();
            var heavyHorsePortal = ServiceProvider.GetService<IHeavyHorseFactory>();
            return DoMapperMethodCall<IHorseList>(target, DataMapperMethod.Update, () => target.Update(cart, lightHorsePortal, heavyHorsePortal));
        }

        public virtual IHorseList? LocalUpdate1(IHorseList itarget, Dal.Ef.Pasture pasture)
        {
            var target = (HorseList)itarget ?? throw new Exception("HorseList must implement IHorseList");
            var lightHorsePortal = ServiceProvider.GetService<ILightHorseFactory>();
            var heavyHorsePortal = ServiceProvider.GetService<IHeavyHorseFactory>();
            return DoMapperMethodCall<IHorseList>(target, DataMapperMethod.Update, () => target.Update(pasture, lightHorsePortal, heavyHorsePortal));
        }

        public virtual IHorseList? LocalSave(IHorseList target, Dal.Ef.Cart cart)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException("HorseListFactory.Update()");
            }
            else if (target.IsNew)
            {
                throw new NotImplementedException("HorseListFactory.Update()");
            }
            else
            {
                return LocalUpdate(target, cart);
            }
        }

        public virtual IHorseList? LocalSave1(IHorseList target, Dal.Ef.Pasture pasture)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException("HorseListFactory.Update()");
            }
            else if (target.IsNew)
            {
                throw new NotImplementedException("HorseListFactory.Update()");
            }
            else
            {
                return LocalUpdate1(target, pasture);
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<HorseList>();
            services.AddTransient<IHorseList, HorseList>();
            services.AddScoped<HorseListFactory>();
            services.AddScoped<IHorseListFactory, HorseListFactory>();
            services.AddScoped<IHorseListFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseListFactory>();
                return () => factory.Create();
            });
            services.AddScoped<IHorseListFactory.FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseListFactory>();
                return (ICollection<Dal.Ef.Horse> horses) => factory.Fetch(horses);
            });
        }
    }
}