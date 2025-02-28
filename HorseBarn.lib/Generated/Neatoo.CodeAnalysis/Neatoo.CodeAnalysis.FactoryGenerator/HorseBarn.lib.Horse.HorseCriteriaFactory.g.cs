using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.AuthorizationRules;
using Neatoo.Rules;
using Neatoo.Rules.Rules;
using System.ComponentModel.DataAnnotations;

/*
Debugging Messages:
For IHorseCriteriaAuthorization using IHorseCriteriaAuthorization
: ValidateBase<HorseCriteria>, IHorseCriteria
*/
namespace HorseBarn.lib.Horse
{
    public interface IHorseCriteriaFactory
    {
        IHorseCriteria Fetch();
        Authorized<IHorseCriteria> TryFetch();
        Authorized CanFetch();
        IHorseCriteria Fetch(IEnumerable<string> horseNames);
        Authorized<IHorseCriteria> TryFetch(IEnumerable<string> horseNames);
        Authorized CanFetch(IEnumerable<string> horseNames);
    }

    internal class HorseCriteriaFactory : FactoryBase, IHorseCriteriaFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public IHorseCriteriaAuthorization IHorseCriteriaAuthorization { get; }

        public HorseCriteriaFactory(IServiceProvider serviceProvider, IHorseCriteriaAuthorization ihorsecriteriaauthorization)
        {
            this.ServiceProvider = serviceProvider;
            this.IHorseCriteriaAuthorization = ihorsecriteriaauthorization;
        }

        public HorseCriteriaFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate, IHorseCriteriaAuthorization ihorsecriteriaauthorization)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            this.IHorseCriteriaAuthorization = ihorsecriteriaauthorization;
        }

        public virtual IHorseCriteria Fetch()
        {
            return (LocalFetch(false)).Result;
        }

        public virtual Authorized<IHorseCriteria> TryFetch()
        {
            return LocalFetch(false);
        }

        public virtual Authorized CanFetch()
        {
            return LocalFetch(true);
        }

        public Authorized<IHorseCriteria> LocalFetch(bool checkAuthOnly)
        {
            Authorized canfetch = IHorseCriteriaAuthorization.CanFetch();
            if (!canfetch.HasAccess)
            {
                return new Authorized<IHorseCriteria>(canfetch);
            }

            if (checkAuthOnly)
            {
                return new Authorized<IHorseCriteria>(true);
            }

            var target = ServiceProvider.GetRequiredService<HorseCriteria>();
            return new Authorized<IHorseCriteria>(DoMapperMethodCall<IHorseCriteria>(target, DataMapperMethod.Fetch, () => target.Fetch()));
        }

        public virtual IHorseCriteria Fetch(IEnumerable<string> horseNames)
        {
            return (LocalFetch1(horseNames, false)).Result;
        }

        public virtual Authorized<IHorseCriteria> TryFetch(IEnumerable<string> horseNames)
        {
            return LocalFetch1(horseNames, false);
        }

        public virtual Authorized CanFetch(IEnumerable<string> horseNames)
        {
            return LocalFetch1(horseNames, true);
        }

        public Authorized<IHorseCriteria> LocalFetch1(IEnumerable<string> horseNames, bool checkAuthOnly)
        {
            Authorized canfetch = IHorseCriteriaAuthorization.CanFetch();
            if (!canfetch.HasAccess)
            {
                return new Authorized<IHorseCriteria>(canfetch);
            }

            if (checkAuthOnly)
            {
                return new Authorized<IHorseCriteria>(true);
            }

            var target = ServiceProvider.GetRequiredService<HorseCriteria>();
            return new Authorized<IHorseCriteria>(DoMapperMethodCall<IHorseCriteria>(target, DataMapperMethod.Fetch, () => target.Fetch(horseNames)));
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<HorseCriteria>();
            services.AddScoped<HorseCriteriaFactory>();
            services.AddScoped<IHorseCriteriaFactory, HorseCriteriaFactory>();
            services.AddTransient<IHorseCriteria, HorseCriteria>();
        }
    }
}