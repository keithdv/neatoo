using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Microsoft.EntityFrameworkCore;
using HorseBarn.Dal.Ef;
using System.ComponentModel;
using System.Diagnostics;

/*
Debugging Messages:
: CustomEditBase<HorseBarn>, IHorseBarn
: EditBase<T>
*/
namespace HorseBarn.lib
{
    public interface IHorseBarnFactory
    {
        IHorseBarn Create();
        Task<IHorseBarn> Fetch();
        Task<IHorseBarn?> Save(IHorseBarn target);
        delegate IHorseBarn CreateDelegate();
        delegate Task<IHorseBarn> FetchDelegate();
        delegate Task<IHorseBarn?> SaveDelegate(IHorseBarn target);
    }

    internal class HorseBarnFactory : FactoryEditBase<HorseBarn>, IHorseBarnFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IHorseBarnFactory.FetchDelegate FetchProperty { get; }
        public IHorseBarnFactory.SaveDelegate SaveProperty { get; set; }

        public HorseBarnFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            FetchProperty = LocalFetch;
            SaveProperty = LocalSave;
        }

        public HorseBarnFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            FetchProperty = RemoteFetch;
            SaveProperty = RemoteSave;
        }

        public virtual Task<IHorseBarn> Fetch()
        {
            return FetchProperty();
        }

        public override async Task<IEditBase?> Save(HorseBarn target)
        {
            return (IEditBase? )(await SaveProperty(target));
        }

        public Task<IHorseBarn?> Save(IHorseBarn target)
        {
            return SaveProperty(target);
        }

        public IHorseBarn Create()
        {
            var target = ServiceProvider.GetRequiredService<HorseBarn>();
            var pasturePortal = ServiceProvider.GetService<IPastureFactory>();
            var cartListPortal = ServiceProvider.GetService<ICartListFactory>();
            return DoMapperMethodCall<IHorseBarn>(target, DataMapperMethod.Create, () => target.Create(pasturePortal, cartListPortal));
        }

        public Task<IHorseBarn> LocalFetch()
        {
            var target = ServiceProvider.GetRequiredService<HorseBarn>();
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            var pasturePortal = ServiceProvider.GetService<IPastureFactory>();
            var cartPortal = ServiceProvider.GetService<ICartListFactory>();
            return DoMapperMethodCallAsync<IHorseBarn>(target, DataMapperMethod.Fetch, () => target.Fetch(horseBarnContext, pasturePortal, cartPortal));
        }

        public virtual Task<IHorseBarn?> LocalInsert(IHorseBarn itarget)
        {
            var target = (HorseBarn)itarget ?? throw new Exception("HorseBarn must implement IHorseBarn");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            var pasturePortal = ServiceProvider.GetService<IPastureFactory>();
            var cartPortal = ServiceProvider.GetService<ICartListFactory>();
            return DoMapperMethodCallAsync<IHorseBarn>(target, DataMapperMethod.Insert, () => target.Insert(horseBarnContext, pasturePortal, cartPortal));
        }

        public virtual Task<IHorseBarn?> LocalUpdate(IHorseBarn itarget)
        {
            var target = (HorseBarn)itarget ?? throw new Exception("HorseBarn must implement IHorseBarn");
            var horseBarnContext = ServiceProvider.GetService<IHorseBarnContext>();
            var pasturePortal = ServiceProvider.GetService<IPastureFactory>();
            var cartPortal = ServiceProvider.GetService<ICartListFactory>();
            return DoMapperMethodCallAsync<IHorseBarn>(target, DataMapperMethod.Update, () => target.Update(horseBarnContext, pasturePortal, cartPortal));
        }

        public virtual async Task<IHorseBarn?> RemoteFetch()
        {
            return await DoRemoteRequest.ForDelegate<HorseBarn?>(typeof(IHorseBarnFactory.FetchDelegate), []);
        }

        public virtual async Task<IHorseBarn?> LocalSave(IHorseBarn target)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException("HorseBarnFactory.Update()");
            }
            else if (target.IsNew)
            {
                return await LocalInsert(target);
            }
            else
            {
                return await LocalUpdate(target);
            }
        }

        public async Task<IHorseBarn?> RemoteSave(IHorseBarn target)
        {
            return await DoRemoteRequest.ForDelegate<HorseBarn?>(typeof(IHorseBarnFactory.SaveDelegate), [target, ]);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<HorseBarn>();
            services.AddTransient<IHorseBarn, HorseBarn>();
            services.AddScoped<HorseBarnFactory>();
            services.AddScoped<IHorseBarnFactory, HorseBarnFactory>();
            services.AddScoped<IHorseBarnFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseBarnFactory>();
                return () => factory.Create();
            });
            services.AddScoped<IHorseBarnFactory.FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseBarnFactory>();
                return () => factory.LocalFetch();
            });
            services.AddScoped<IHorseBarnFactory.SaveDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseBarnFactory>();
                return (target) => factory.LocalSave(target);
            });
            services.AddScoped<IFactoryEditBase<HorseBarn>, HorseBarnFactory>();
        }
    }
}