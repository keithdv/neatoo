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
: Horse<LightHorse>, ILightHorse
: CustomEditBase<H>, IHorse
: EditBase<T>
*/
namespace HorseBarn.lib.Horse
{
    public interface ILightHorseFactory
    {
        ILightHorse Create(IHorseCriteria criteria);
    }

    internal class LightHorseFactory : FactoryEditBase<LightHorse>, ILightHorseFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public delegate ILightHorse CreateDelegate(IHorseCriteria criteria);
        public LightHorseFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public LightHorseFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public ILightHorse Create(IHorseCriteria criteria)
        {
            var target = ServiceProvider.GetRequiredService<LightHorse>();
            return DoMapperMethodCall<ILightHorse>(target, DataMapperMethod.Create, () => target.Create(criteria));
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<LightHorse>();
            services.AddScoped<LightHorseFactory>();
            services.AddScoped<ILightHorseFactory, LightHorseFactory>();
            services.AddTransient<ILightHorse, LightHorse>();
            services.AddScoped<CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<LightHorseFactory>();
                return (IHorseCriteria criteria) => factory.Create(criteria);
            });
            services.AddScoped<IFactoryEditBase<LightHorse>, LightHorseFactory>();
        }
    }
}