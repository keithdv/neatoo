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
        Task<IHeavyHorse> Create(IHorseCriteria horseCriteria);
    }

    [Factory<IHeavyHorse>]
    internal class HeavyHorseFactory : FactoryEditBase<HeavyHorse>, IHeavyHorseFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        public HeavyHorseFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public HeavyHorseFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public async Task<IHeavyHorse> Create(IHorseCriteria horseCriteria)
        {
            var target = ServiceProvider.GetRequiredService<HeavyHorse>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create(horseCriteria);
                return Task.CompletedTask;
            });
            return target;
        }
    }
}