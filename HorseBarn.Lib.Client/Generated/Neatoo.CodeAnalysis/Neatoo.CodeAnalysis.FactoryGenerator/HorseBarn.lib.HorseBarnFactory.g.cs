using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
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
        Task<IHorseBarn> Fetch();
        Task<IHorseBarn> Create();
        Task<IHorseBarn?> Save(IHorseBarn target);
        delegate Task<IHorseBarn> FetchDelegate();
        delegate Task<IHorseBarn> CreateDelegate();
        delegate Task<IHorseBarn?> SaveDelegate(IHorseBarn target);
    }

    internal class HorseBarnFactory : FactoryEditBase<HorseBarn>, IHorseBarnFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IHorseBarnFactory.FetchDelegate FetchProperty { get; }
        public IHorseBarnFactory.CreateDelegate CreateProperty { get; }
        public IHorseBarnFactory.SaveDelegate SaveProperty { get; set; }

        public HorseBarnFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            FetchProperty = LocalFetch;
            CreateProperty = LocalCreate;
            SaveProperty = LocalSave;
        }

        public HorseBarnFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            FetchProperty = RemoteFetch;
            CreateProperty = RemoteCreate;
            SaveProperty = RemoteSave;
        }

        public virtual Task<IHorseBarn> Fetch()
        {
            return FetchProperty();
        }

        public virtual Task<IHorseBarn> Create()
        {
            return CreateProperty();
        }

        public override async Task<IEditBase?> Save(HorseBarn target)
        {
            return (IEditBase? )(await SaveProperty(target));
        }

        public Task<IHorseBarn?> Save(IHorseBarn target)
        {
            return SaveProperty(target);
        }

        public Task<IHorseBarn> LocalFetch()
        {
            var target = ServiceProvider.GetRequiredService<HorseBarn>();
            return DoMapperMethodCallAsync<IHorseBarn>(target, DataMapperMethod.Fetch, () => target.Fetch());
        }

        public Task<IHorseBarn> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<HorseBarn>();
            return DoMapperMethodCallAsync<IHorseBarn>(target, DataMapperMethod.Create, () => target.Create());
        }

        public virtual Task<IHorseBarn?> LocalUpdate(IHorseBarn itarget)
        {
            var target = (HorseBarn)itarget ?? throw new Exception("HorseBarn must implement IHorseBarn");
            return DoMapperMethodCallAsync<IHorseBarn>(target, DataMapperMethod.Update, () => target.Update());
        }

        public virtual async Task<IHorseBarn?> RemoteFetch()
        {
            return await DoRemoteRequest.ForDelegate<HorseBarn?>(typeof(IHorseBarnFactory.FetchDelegate), []);
        }

        public virtual async Task<IHorseBarn?> RemoteCreate()
        {
            return await DoRemoteRequest.ForDelegate<HorseBarn?>(typeof(IHorseBarnFactory.CreateDelegate), []);
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
                throw new NotImplementedException("HorseBarnFactory.Update()");
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
            services.AddScoped<IHorseBarnFactory.FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseBarnFactory>();
                return () => factory.LocalFetch();
            });
            services.AddScoped<IHorseBarnFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseBarnFactory>();
                return () => factory.LocalCreate();
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