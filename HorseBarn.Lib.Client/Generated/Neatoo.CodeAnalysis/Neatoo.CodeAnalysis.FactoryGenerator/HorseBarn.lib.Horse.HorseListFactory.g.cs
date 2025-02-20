using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using HorseBarn.lib.Cart;
using Neatoo;
using Neatoo.Portal;
using System.Collections.Specialized;

/*
Debugging Messages:
: EditListBase<HorseList, IHorse>, IHorseList
*/
namespace HorseBarn.lib.Horse
{
    public interface IHorseListFactory
    {
        Task<IHorseList> Create();
    }

    [Factory<IHorseList>]
    internal class HorseListFactory : FactoryBase<HorseList>, IHorseListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        public HorseListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public HorseListFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public async Task<IHorseList> Create()
        {
            var target = ServiceProvider.GetRequiredService<HorseList>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create();
                return Task.CompletedTask;
            });
            return target;
        }
    }
}