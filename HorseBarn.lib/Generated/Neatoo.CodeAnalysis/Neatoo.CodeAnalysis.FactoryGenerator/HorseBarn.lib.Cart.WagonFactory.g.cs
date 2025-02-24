using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.lib.Horse;
using Neatoo.Rules.Rules;
using Neatoo.Rules;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.ComponentModel;
using HorseBarn.Dal.Ef;

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
        IWagon Fetch(Dal.Ef.Cart cart);
        IWagon? Save(IWagon target, Dal.Ef.HorseBarn horseBarn);
    }

    internal class WagonFactory : FactoryEditBase<Wagon>, IWagonFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public SaveDelegate SaveProperty { get; set; }

        public delegate Task<IWagon> CreateDelegate();
        public delegate IWagon FetchDelegate(Dal.Ef.Cart cart);
        public delegate IWagon? SaveDelegate(IWagon target, Dal.Ef.HorseBarn horseBarn);
        public WagonFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            SaveProperty = LocalSave;
        }

        public WagonFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public IWagon? Save(IWagon target, Dal.Ef.HorseBarn horseBarn)
        {
            return SaveProperty(target, horseBarn);
        }

        public Task<IWagon> Create()
        {
            var target = ServiceProvider.GetRequiredService<Wagon>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            var allRequiredRulesExecutedFactory = ServiceProvider.GetService<IAllRequiredRulesExecuted.Factory>();
            return DoMapperMethodCallAsync<IWagon>(target, DataMapperMethod.Create, () => target.Create(horsePortal, allRequiredRulesExecutedFactory));
        }

        public IWagon Fetch(Dal.Ef.Cart cart)
        {
            var target = ServiceProvider.GetRequiredService<Wagon>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IWagon>(target, DataMapperMethod.Fetch, () => target.Fetch(cart, horsePortal));
        }

        public virtual Task<IWagon?> LocalInsert(IWagon itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (Wagon)itarget ?? throw new Exception("Wagon must implement IWagon");
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCallAsync<IWagon>(target, DataMapperMethod.Insert, () => target.Insert(horseBarn, horsePortal));
        }

        public virtual Task<IWagon?> LocalUpdate(IWagon itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (Wagon)itarget ?? throw new Exception("Wagon must implement IWagon");
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCallAsync<IWagon>(target, DataMapperMethod.Update, () => target.Update(horseBarn, horsePortal));
        }

        public virtual IWagon? LocalSave(IWagon target, Dal.Ef.HorseBarn horseBarn)
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
                return LocalInsert(target, horseBarn);
            }
            else
            {
                return LocalUpdate(target, horseBarn);
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<Wagon>();
            services.AddScoped<WagonFactory>();
            services.AddScoped<IWagonFactory, WagonFactory>();
            services.AddTransient<IWagon, Wagon>();
            services.AddScoped<CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<WagonFactory>();
                return () => factory.Create();
            });
            services.AddScoped<FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<WagonFactory>();
                return (Dal.Ef.Cart cart) => factory.Fetch(cart);
            });
            services.AddScoped<IFactoryEditBase<Wagon>, WagonFactory>();
        }
    }
}