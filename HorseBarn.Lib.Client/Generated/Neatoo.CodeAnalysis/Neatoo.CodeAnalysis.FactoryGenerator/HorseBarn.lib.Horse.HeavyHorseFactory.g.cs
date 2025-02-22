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
: EditBase<T>
*/
namespace HorseBarn.lib.Horse
{
    public interface IHeavyHorseFactory
    {
        IHeavyHorse Create(IHorseCriteria horseCriteria);
        delegate IHeavyHorse CreateDelegate(IHorseCriteria horseCriteria);
    }

    internal class HeavyHorseFactory : FactoryEditBase<HeavyHorse>, IHeavyHorseFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public HeavyHorseFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public HeavyHorseFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public IHeavyHorse Create(IHorseCriteria horseCriteria)
        {
            var target = ServiceProvider.GetRequiredService<HeavyHorse>();
            return DoMapperMethodCall<IHeavyHorse>(target, DataMapperMethod.Create, () => target.Create(horseCriteria));
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<HeavyHorse>();
            services.AddTransient<IHeavyHorse, HeavyHorse>();
            services.AddScoped<HeavyHorseFactory>();
            services.AddScoped<IHeavyHorseFactory, HeavyHorseFactory>();
            services.AddScoped<IHeavyHorseFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HeavyHorseFactory>();
                return (IHorseCriteria horseCriteria) => factory.Create(horseCriteria);
            });
            services.AddScoped<IFactoryEditBase<HeavyHorse>, HeavyHorseFactory>();
        }
    }
}