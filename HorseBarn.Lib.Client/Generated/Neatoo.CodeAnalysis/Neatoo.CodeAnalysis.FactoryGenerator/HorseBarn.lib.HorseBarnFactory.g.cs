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
No DataMapperMethod attribute for AddRacingChariot
No DataMapperMethod attribute for AddWagon
No DataMapperMethod attribute for AddNewHorse
No DataMapperMethod attribute for MoveHorseToCart
No DataMapperMethod attribute for MoveHorseToPasture
: EditBase<T>
*/
namespace HorseBarn.lib
{
    public interface IHorseBarnFactory
    {
        Task<IHorseBarn> Fetch();
        Task<IHorseBarn> Create();
        Task<IHorseBarn?> Save(IHorseBarn target);
    }

    internal class HorseBarnFactory : FactoryEditBase<HorseBarn>, IFactoryEditBase<HorseBarn>, IHorseBarnFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        public delegate Task<IHorseBarn> FetchDelegate();
        public delegate Task<IHorseBarn> CreateDelegate();
        public delegate Task<IHorseBarn?> SaveDelegate(IHorseBarn target);
        // Delegate Properties to provide Local or Remote fork in execution
        public FetchDelegate FetchProperty { get; }
        public CreateDelegate CreateProperty { get; }
        public SaveDelegate SaveProperty { get; }

        public HorseBarnFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            FetchProperty = LocalFetch;
            CreateProperty = LocalCreate;
            SaveProperty = LocalSave;
        }

        public HorseBarnFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
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

        public virtual async Task<IHorseBarn> RemoteFetch()
        {
            return await DoRemoteRequest.ForDelegate<IHorseBarn>(typeof(FetchDelegate), []);
        }

        public Task<IHorseBarn> LocalFetch()
        {
            var target = ServiceProvider.GetRequiredService<HorseBarn>();
            return Task.FromResult(DoMapperMethodCall<IHorseBarn>(target, DataMapperMethod.Fetch, () => target.Fetch()));
        }

        public virtual Task<IHorseBarn> Create()
        {
            return CreateProperty();
        }

        public virtual async Task<IHorseBarn> RemoteCreate()
        {
            return await DoRemoteRequest.ForDelegate<IHorseBarn>(typeof(CreateDelegate), []);
        }

        public Task<IHorseBarn> LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<HorseBarn>();
            return Task.FromResult(DoMapperMethodCall<IHorseBarn>(target, DataMapperMethod.Create, () => target.Create()));
        }

        public virtual IHorseBarn? LocalUpdate(IHorseBarn target)
        {
            var cTarget = (HorseBarn)target ?? throw new Exception("HorseBarn must implement IHorseBarn");
            return DoMapperMethodCall<IHorseBarn>(cTarget, DataMapperMethod.Update, () => cTarget.Update());
        }

        async Task<IEditBase?> IFactoryEditBase<HorseBarn>.Save(HorseBarn target)
        {
            return (IEditBase? )(await SaveProperty(target));
        }

        public virtual Task<IHorseBarn?> Save(IHorseBarn target)
        {
            return SaveProperty(target);
        }

        public virtual async Task<IHorseBarn?> RemoteSave(IHorseBarn target)
        {
            return await DoRemoteRequest.ForDelegate<IHorseBarn?>(typeof(SaveDelegate), [target]);
        }

        public virtual Task<IHorseBarn?> LocalSave(IHorseBarn target)
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
                return Task.FromResult(LocalUpdate(target));
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
            services.AddScoped<SaveDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<HorseBarnFactory>();
                return (IHorseBarn target) => factory.LocalSave(target);
            });
            services.AddScoped<IFactoryEditBase<HorseBarn>, HorseBarnFactory>();
        }
    }
}