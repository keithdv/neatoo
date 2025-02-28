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
No DataMapperMethod attribute for RemoveHorse
: EditBase<T>
No DataMapperMethod attribute for HandleIdPropertyChanged
*/
namespace HorseBarn.lib
{
    public interface IPastureFactory
    {
        IPasture Create();
        IPasture Fetch(Dal.Ef.Pasture pasture);
        IPasture? Save(IPasture target, Dal.Ef.HorseBarn horseBarn);
    }

    internal class PastureFactory : FactoryEditBase<Pasture>, IFactoryEditBase<Pasture>, IPastureFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public PastureFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public PastureFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public virtual IPasture Create()
        {
            return LocalCreate();
        }

        public IPasture LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<Pasture>();
            var horseListPortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IPasture>(target, DataMapperMethod.Create, () => target.Create(horseListPortal));
        }

        public virtual IPasture Fetch(Dal.Ef.Pasture pasture)
        {
            return LocalFetch(pasture);
        }

        public IPasture LocalFetch(Dal.Ef.Pasture pasture)
        {
            var target = ServiceProvider.GetRequiredService<Pasture>();
            var horseListPortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IPasture>(target, DataMapperMethod.Fetch, () => target.Fetch(pasture, horseListPortal));
        }

        public virtual IPasture? LocalInsert(IPasture target, Dal.Ef.HorseBarn horseBarn)
        {
            var cTarget = (Pasture)target ?? throw new Exception("Pasture must implement IPasture");
            var horseListPortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IPasture>(cTarget, DataMapperMethod.Insert, () => cTarget.Insert(horseBarn, horseListPortal));
        }

        public virtual IPasture? LocalUpdate(IPasture target, Dal.Ef.HorseBarn horseBarn)
        {
            var cTarget = (Pasture)target ?? throw new Exception("Pasture must implement IPasture");
            var horseListPortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IPasture>(cTarget, DataMapperMethod.Update, () => cTarget.Update(horseBarn, horseListPortal));
        }

        public virtual IPasture? Save(IPasture target, Dal.Ef.HorseBarn horseBarn)
        {
            return LocalSave(target, horseBarn);
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
            services.AddScoped<IFactoryEditBase<Pasture>, PastureFactory>();
        }
    }
}