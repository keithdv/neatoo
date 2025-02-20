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
        Task<ILightHorse> Create(IHorseCriteria criteria);
    }

    [Factory<ILightHorse>]
    internal class LightHorseFactory : FactoryEditBase<LightHorse>, ILightHorseFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        public LightHorseFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public LightHorseFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public async Task<ILightHorse> Create(IHorseCriteria criteria)
        {
            var target = ServiceProvider.GetRequiredService<LightHorse>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create(criteria);
                return Task.CompletedTask;
            });
            return target;
        }
    }
}