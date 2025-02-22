using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.lib.Horse;
using Neatoo.Rules.Rules;
using Neatoo.Rules;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.ComponentModel;

/*
Debugging Messages:
: Cart<Wagon, IHeavyHorse>, IWagon
: CustomEditBase<C>, ICart
: EditBase<T>
*/
namespace HorseBarn.lib.Cart
{
    public interface IWagonFactory
    {
        Task<IWagon> Create();
        delegate Task<IWagon> CreateDelegate();
    }

    internal class WagonFactory : FactoryEditBase<Wagon>, IWagonFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IWagonFactory.CreateDelegate CreateProperty { get; }

        public WagonFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateProperty = LocalCreate;
        }

        public WagonFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateProperty = RemoteCreate;
        }

        public virtual Task<IWagon> Create()
        {
            return CreateProperty();
        }

        public Task<IWagon> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<Wagon>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            var allRequiredRulesExecutedFactory = ServiceProvider.GetService<IAllRequiredRulesExecuted.Factory>();
            return DoMapperMethodCallAsync<IWagon>(target, DataMapperMethod.Create, () => target.Create(horsePortal, allRequiredRulesExecutedFactory));
        }

        public virtual async Task<IWagon?> RemoteCreate()
        {
            return await DoRemoteRequest.ForDelegate<Wagon?>(typeof(IWagonFactory.CreateDelegate), []);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<Wagon>();
            services.AddTransient<IWagon, Wagon>();
            services.AddScoped<WagonFactory>();
            services.AddScoped<IWagonFactory, WagonFactory>();
            services.AddScoped<IWagonFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<WagonFactory>();
                return () => factory.LocalCreate();
            });
            services.AddScoped<IFactoryEditBase<Wagon>, WagonFactory>();
        }
    }
}