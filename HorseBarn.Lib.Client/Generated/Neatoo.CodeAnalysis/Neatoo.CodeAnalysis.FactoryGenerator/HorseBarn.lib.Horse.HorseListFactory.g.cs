using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.lib.Cart;
using System.Collections.Specialized;

/*
Debugging Messages:
: EditListBase<HorseList, IHorse>, IHorseList
No DataMapperMethod attribute for RemoveHorse
*/
namespace HorseBarn.lib.Horse
{
    public interface IHorseListFactory
    {
        IHorseList Create();
    }

    internal class HorseListFactory : FactoryBase, IHorseListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public HorseListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public HorseListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public virtual IHorseList Create()
        {
            return LocalCreate();
        }

        public IHorseList LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<HorseList>();
            return DoMapperMethodCall<IHorseList>(target, DataMapperMethod.Create, () => target.Create());
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<HorseList>();
            services.AddScoped<HorseListFactory>();
            services.AddScoped<IHorseListFactory, HorseListFactory>();
            services.AddTransient<IHorseList, HorseList>();
        }
    }
}