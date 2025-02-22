using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules;
using Neatoo.Rules.Rules;
using System.ComponentModel.DataAnnotations;

/*
Debugging Messages:
: ValidateBase<HorseCriteria>, IHorseCriteria
*/
namespace HorseBarn.lib.Horse
{
    public interface IHorseCriteriaFactory
    {
        IHorseCriteria Fetch();
        IHorseCriteria Fetch(IEnumerable<string> horseNames);
        delegate IHorseCriteria FetchDelegate();
        delegate IHorseCriteria Fetch1Delegate(IEnumerable<string> horseNames);
    }

    internal class HorseCriteriaFactory : FactoryBase, IHorseCriteriaFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public HorseCriteriaFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public HorseCriteriaFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public IHorseCriteria Fetch()
        {
            var target = ServiceProvider.GetRequiredService<HorseCriteria>();
            return DoMapperMethodCall<IHorseCriteria>(target, DataMapperMethod.Fetch, () => target.Fetch());
        }

        public IHorseCriteria Fetch(IEnumerable<string> horseNames)
        {
            var target = ServiceProvider.GetRequiredService<HorseCriteria>();
            return DoMapperMethodCall<IHorseCriteria>(target, DataMapperMethod.Fetch, () => target.Fetch(horseNames));
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<HorseCriteria>();
            services.AddTransient<IHorseCriteria, HorseCriteria>();
            services.AddScoped<HorseCriteriaFactory>();
            services.AddScoped<IHorseCriteriaFactory, HorseCriteriaFactory>();
            services.AddScoped<IHorseCriteriaFactory.FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseCriteriaFactory>();
                return () => factory.Fetch();
            });
            services.AddScoped<IHorseCriteriaFactory.Fetch1Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseCriteriaFactory>();
                return (IEnumerable<string> horseNames) => factory.Fetch(horseNames);
            });
        }
    }
}