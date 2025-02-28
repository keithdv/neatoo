using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using System;

/*
Debugging Messages:
: EditBase<EditObject>, IEditObject
No DataMapperMethod attribute for MarkAsChild
No DataMapperMethod attribute for MarkDeleted
No DataMapperMethod attribute for MarkNew
No DataMapperMethod attribute for MarkOld
No DataMapperMethod attribute for MarkUnmodified
*/
namespace Neatoo.UnitTest.SystemTextJson.EditTests
{
    public interface IEditObjectFactory
    {
        Task<IEditObject> Create(Guid ID, string Name);
        Task<IEditObject?> Save(IEditObject target);
    }

    internal class EditObjectFactory : FactoryEditBase<EditObject>, IFactoryEditBase<EditObject>, IEditObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public EditObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public EditObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public virtual Task<IEditObject> Create(Guid ID, string Name)
        {
            return LocalCreate(ID, Name);
        }

        public Task<IEditObject> LocalCreate(Guid ID, string Name)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Create, () => target.Create(ID, Name));
        }

        public virtual Task<IEditObject?> LocalUpdate(IEditObject target)
        {
            var cTarget = (EditObject)target ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCallAsync<IEditObject>(cTarget, DataMapperMethod.Update, () => cTarget.Update());
        }

        async Task<IEditBase?> IFactoryEditBase<EditObject>.Save(EditObject target)
        {
            return (IEditBase? )await Save(target);
        }

        public virtual Task<IEditObject?> Save(IEditObject target)
        {
            return LocalSave(target);
        }

        public virtual Task<IEditObject?> LocalSave(IEditObject target)
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
            services.AddTransient<EditObject>();
            services.AddScoped<EditObjectFactory>();
            services.AddScoped<IEditObjectFactory, EditObjectFactory>();
            services.AddTransient<IEditObject, EditObject>();
            services.AddScoped<IFactoryEditBase<EditObject>, EditObjectFactory>();
        }
    }
}