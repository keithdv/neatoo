using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.lib.Horse;
using System.Diagnostics;
using HorseBarn.Dal.Ef;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

/*
Debugging Messages:
: CustomEditBase<Pasture>, IPasture
: EditBase<T>
*/
namespace HorseBarn.lib
{
    public interface IPastureFactory
    {
        IPasture Create();
        IPasture Fetch(Dal.Ef.Pasture pasture);
        IPasture? Save(IPasture target, Dal.Ef.HorseBarn horseBarn);
    }

    internal class PastureFactory : FactoryEditBase<Pasture>, IPastureFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public SaveDelegate SaveProperty { get; set; }

        public delegate IPasture CreateDelegate();
        public delegate IPasture FetchDelegate(Dal.Ef.Pasture pasture);
        public delegate IPasture? SaveDelegate(IPasture target, Dal.Ef.HorseBarn horseBarn);
        public PastureFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            SaveProperty = LocalSave;
        }

        public PastureFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public IPasture? Save(IPasture target, Dal.Ef.HorseBarn horseBarn)
        {
            return SaveProperty(target, horseBarn);
        }

        public IPasture Create()
        {
            var target = ServiceProvider.GetRequiredService<Pasture>();
            var horseListPortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IPasture>(target, DataMapperMethod.Create, () => target.Create(horseListPortal));
        }

        public IPasture Fetch(Dal.Ef.Pasture pasture)
        {
            var target = ServiceProvider.GetRequiredService<Pasture>();
            var horseListPortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IPasture>(target, DataMapperMethod.Fetch, () => target.Fetch(pasture, horseListPortal));
        }

        public virtual Task<IPasture?> LocalInsert(IPasture itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (Pasture)itarget ?? throw new Exception("Pasture must implement IPasture");
            var horseListPortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCallAsync<IPasture>(target, DataMapperMethod.Insert, () => target.Insert(horseBarn, horseListPortal));
        }

        public virtual Task<IPasture?> LocalUpdate(IPasture itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (Pasture)itarget ?? throw new Exception("Pasture must implement IPasture");
            var horseListPortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCallAsync<IPasture>(target, DataMapperMethod.Update, () => target.Update(horseBarn, horseListPortal));
        }

        public virtual IPasture? LocalSave(IPasture target, Dal.Ef.HorseBarn horseBarn)
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
            services.AddTransient<Pasture>();
            services.AddScoped<PastureFactory>();
            services.AddScoped<IPastureFactory, PastureFactory>();
            services.AddTransient<IPasture, Pasture>();
            services.AddScoped<CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<PastureFactory>();
                return () => factory.Create();
            });
            services.AddScoped<FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<PastureFactory>();
                return (Dal.Ef.Pasture pasture) => factory.Fetch(pasture);
            });
            services.AddScoped<IFactoryEditBase<Pasture>, PastureFactory>();
        }
    }
}