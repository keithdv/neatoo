using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.lib.Cart;
using System.Collections.Specialized;

/*
Debugging Messages:
: EditListBase<HorseList, IHorse>, IHorseList
*/
namespace HorseBarn.lib.Horse
{
    public interface IHorseListFactory
    {
        IHorseList Create();
        delegate IHorseList CreateDelegate();
    }

    internal class HorseListFactory : FactoryBase, IHorseListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public HorseListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public HorseListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public IHorseList Create()
        {
            var target = ServiceProvider.GetRequiredService<HorseList>();
            return DoMapperMethodCall<IHorseList>(target, DataMapperMethod.Create, () => target.Create());
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<HorseList>();
            services.AddTransient<IHorseList, HorseList>();
            services.AddScoped<HorseListFactory>();
            services.AddScoped<IHorseListFactory, HorseListFactory>();
            services.AddScoped<IHorseListFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseListFactory>();
                return () => factory.Create();
            });
        }
    }
}