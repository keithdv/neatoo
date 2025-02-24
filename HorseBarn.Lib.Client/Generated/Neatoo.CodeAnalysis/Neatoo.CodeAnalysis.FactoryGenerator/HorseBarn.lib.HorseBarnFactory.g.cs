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
        IHorseBarn? Save(IHorseBarn target);
    }

    internal class HorseBarnFactory : FactoryEditBase<HorseBarn>, IHorseBarnFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public FetchDelegate FetchProperty { get; }
        public CreateDelegate CreateProperty { get; }
        public SaveDelegate SaveProperty { get; set; }

        public delegate Task<IHorseBarn> FetchDelegate();
        public delegate Task<IHorseBarn> CreateDelegate();
        public delegate IHorseBarn? SaveDelegate(IHorseBarn target);
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
        }

        public virtual Task<IHorseBarn> Fetch()
        {
            return FetchProperty();
        }

        public virtual Task<IHorseBarn> Create()
        {
            return CreateProperty();
        }

        public override Task<IEditBase?> Save(HorseBarn target)
        {
            return Task.FromResult<IEditBase?>(SaveProperty(target));
        }

        public IHorseBarn? Save(IHorseBarn target)
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
            return await DoRemoteRequest.ForDelegate<HorseBarn?>(typeof(FetchDelegate), []);
        }

        public virtual async Task<IHorseBarn?> RemoteCreate()
        {
            return await DoRemoteRequest.ForDelegate<HorseBarn?>(typeof(CreateDelegate), []);
        }

        public virtual IHorseBarn? LocalSave(IHorseBarn target)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                throw new NotImplementedException();
            }
            else
            {
                return LocalUpdate(target);
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<HorseBarn>();
            services.AddScoped<HorseBarnFactory>();
            services.AddScoped<IHorseBarnFactory, HorseBarnFactory>();
            services.AddTransient<IHorseBarn, HorseBarn>();
            services.AddScoped<FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseBarnFactory>();
                return () => factory.LocalFetch();
            });
            services.AddScoped<CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseBarnFactory>();
                return () => factory.LocalCreate();
            });
            services.AddScoped<IFactoryEditBase<HorseBarn>, HorseBarnFactory>();
        }
    }
}