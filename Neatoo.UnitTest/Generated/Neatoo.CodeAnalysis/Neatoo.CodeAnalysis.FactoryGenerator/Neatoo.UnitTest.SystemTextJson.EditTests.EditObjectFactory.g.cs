using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using System;

/*
Debugging Messages:
: EditBase<EditObject>, IEditObject
*/
namespace Neatoo.UnitTest.SystemTextJson.EditTests
{
    public interface IEditObjectFactory
    {
        Task<IEditObject> Create(Guid ID, string Name);
        Task<IEditObject?> Save(IEditObject target);
        delegate Task<IEditObject> CreateDelegate(Guid ID, string Name);
        delegate Task<IEditObject?> SaveDelegate(IEditObject target);
    }

    internal class EditObjectFactory : FactoryEditBase<EditObject>, IEditObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IEditObjectFactory.SaveDelegate SaveProperty { get; set; }

        public EditObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            SaveProperty = LocalSave;
        }

        public EditObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            SaveProperty = RemoteSave;
        }

        public override async Task<IEditBase?> Save(EditObject target)
        {
            return (IEditBase? )(await SaveProperty(target));
        }

        public Task<IEditObject?> Save(IEditObject target)
        {
            return SaveProperty(target);
        }

        public Task<IEditObject> Create(Guid ID, string Name)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Create, () => target.Create(ID, Name));
        }

        public virtual Task<IEditObject?> LocalUpdate(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Update, () => target.Update());
        }

        public virtual Task<IEditObject?> LocalUpdate1(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Insert, () => target.Update());
        }

        public virtual async Task<IEditObject?> LocalSave(IEditObject target)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException("EditObjectFactory.Update()");
            }
            else if (target.IsNew)
            {
                return await LocalUpdate1(target);
            }
            else
            {
                return await LocalUpdate(target);
            }
        }

        public async Task<IEditObject?> RemoteSave(IEditObject target)
        {
            return await DoRemoteRequest.ForDelegate<EditObject?>(typeof(IEditObjectFactory.SaveDelegate), [target, ]);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<EditObject>();
            services.AddScoped<EditObjectFactory>();
            services.AddScoped<IEditObjectFactory, EditObjectFactory>();
            services.AddScoped<IEditObject, EditObject>();
            services.AddScoped<IEditObjectFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (Guid ID, string Name) => factory.Create(ID, Name);
            });
            services.AddScoped<IEditObjectFactory.SaveDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (target) => factory.LocalSave(target);
            });
            services.AddScoped<IFactoryEditBase<EditObject>, EditObjectFactory>();
        }
    }
}