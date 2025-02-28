using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;

/*
Debugging Messages:
: Horse<HeavyHorse>, IHeavyHorse
: CustomEditBase<H>, IHorse
No DataMapperMethod attribute for Create
: EditBase<T>
*/
namespace HorseBarn.lib.Horse
{
    public interface IHeavyHorseFactory
    {
        IHeavyHorse Create(IHorseCriteria horseCriteria);
    }

    internal class HeavyHorseFactory : FactoryBase, IHeavyHorseFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public HeavyHorseFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public HeavyHorseFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public virtual IHeavyHorse Create(IHorseCriteria horseCriteria)
        {
            return LocalCreate(horseCriteria);
        }

        public IHeavyHorse LocalCreate(IHorseCriteria horseCriteria)
        {
            var target = ServiceProvider.GetRequiredService<HeavyHorse>();
            return DoMapperMethodCall<IHeavyHorse>(target, DataMapperMethod.Create, () => target.Create(horseCriteria));
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<HeavyHorse>();
            services.AddScoped<HeavyHorseFactory>();
            services.AddScoped<IHeavyHorseFactory, HeavyHorseFactory>();
            services.AddTransient<IHeavyHorse, HeavyHorse>();
        }
    }
}