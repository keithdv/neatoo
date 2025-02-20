using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules;
using System.ComponentModel.DataAnnotations;

/*
Debugging Messages:
: ValidateBase<HorseCriteria>, IHorseCriteria
*/
namespace HorseBarn.lib.Horse
{
    public interface IHorseCriteriaFactory
    {
        Task<IHorseCriteria> Fetch();
        Task<IHorseCriteria> Fetch(IEnumerable<string> horseNames);
    }

    [Factory<IHorseCriteria>]
    internal class HorseCriteriaFactory : FactoryBase<HorseCriteria>, IHorseCriteriaFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        public HorseCriteriaFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public HorseCriteriaFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public async Task<IHorseCriteria> Fetch()
        {
            var target = ServiceProvider.GetRequiredService<HorseCriteria>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () =>
            {
                target.Fetch();
                return Task.CompletedTask;
            });
            return target;
        }

        public async Task<IHorseCriteria> Fetch(IEnumerable<string> horseNames)
        {
            var target = ServiceProvider.GetRequiredService<HorseCriteria>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () =>
            {
                target.Fetch(horseNames);
                return Task.CompletedTask;
            });
            return target;
        }
    }
}