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
using HorseBarn.Dal.Ef;

/*
Debugging Messages:
: Cart<RacingChariot, ILightHorse>, IRacingChariot
: CustomEditBase<C>, ICart
: EditBase<T>
*/
namespace HorseBarn.lib.Cart
{
    public interface IRacingChariotFactory
    {
        Task<IRacingChariot> Create();
        IRacingChariot Fetch(Dal.Ef.Cart cart);
        IRacingChariot? Save(IRacingChariot target, Dal.Ef.HorseBarn horseBarn);
        delegate Task<IRacingChariot> CreateDelegate();
        delegate IRacingChariot FetchDelegate(Dal.Ef.Cart cart);
        delegate IRacingChariot? SaveDelegate(IRacingChariot target, Dal.Ef.HorseBarn horseBarn);
    }

    internal class RacingChariotFactory : FactoryEditBase<RacingChariot>, IRacingChariotFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IRacingChariotFactory.CreateDelegate CreateProperty { get; }
        public IRacingChariotFactory.SaveDelegate SaveProperty { get; set; }

        public RacingChariotFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateProperty = LocalCreate;
            SaveProperty = LocalSave;
        }

        public RacingChariotFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateProperty = RemoteCreate;
        }

        public virtual Task<IRacingChariot> Create()
        {
            return CreateProperty();
        }

        public IRacingChariot? Save(IRacingChariot target, Dal.Ef.HorseBarn horseBarn)
        {
            return SaveProperty(target, horseBarn);
        }

        public Task<IRacingChariot> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<RacingChariot>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            var allRequiredRulesExecutedFactory = ServiceProvider.GetService<IAllRequiredRulesExecuted.Factory>();
            return DoMapperMethodCallAsync<IRacingChariot>(target, DataMapperMethod.Create, () => target.Create(horsePortal, allRequiredRulesExecutedFactory));
        }

        public IRacingChariot Fetch(Dal.Ef.Cart cart)
        {
            var target = ServiceProvider.GetRequiredService<RacingChariot>();
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IRacingChariot>(target, DataMapperMethod.Fetch, () => target.Fetch(cart, horsePortal));
        }

        public virtual IRacingChariot? LocalInsert(IRacingChariot itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (RacingChariot)itarget ?? throw new Exception("RacingChariot must implement IRacingChariot");
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IRacingChariot>(target, DataMapperMethod.Insert, () => target.Insert(horseBarn, horsePortal));
        }

        public virtual IRacingChariot? LocalUpdate(IRacingChariot itarget, Dal.Ef.HorseBarn horseBarn)
        {
            var target = (RacingChariot)itarget ?? throw new Exception("RacingChariot must implement IRacingChariot");
            var horsePortal = ServiceProvider.GetService<IHorseListFactory>();
            return DoMapperMethodCall<IRacingChariot>(target, DataMapperMethod.Update, () => target.Update(horseBarn, horsePortal));
        }

        public virtual async Task<IRacingChariot?> RemoteCreate()
        {
            return await DoRemoteRequest.ForDelegate<RacingChariot?>(typeof(IRacingChariotFactory.CreateDelegate), []);
        }

        public virtual IRacingChariot? LocalSave(IRacingChariot target, Dal.Ef.HorseBarn horseBarn)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException("RacingChariotFactory.Update()");
            }
            else if (target.IsNew)
            {
                return LocalInsert(target, horseBarn);
            }
            else
            {
                return LocalUpdate(target, horseBarn);
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<RacingChariot>();
            services.AddTransient<IRacingChariot, RacingChariot>();
            services.AddScoped<RacingChariotFactory>();
            services.AddScoped<IRacingChariotFactory, RacingChariotFactory>();
            services.AddScoped<IRacingChariotFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<RacingChariotFactory>();
                return () => factory.LocalCreate();
            });
            services.AddScoped<IRacingChariotFactory.FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<RacingChariotFactory>();
                return (Dal.Ef.Cart cart) => factory.Fetch(cart);
            });
            services.AddScoped<IFactoryEditBase<RacingChariot>, RacingChariotFactory>();
        }
    }
}