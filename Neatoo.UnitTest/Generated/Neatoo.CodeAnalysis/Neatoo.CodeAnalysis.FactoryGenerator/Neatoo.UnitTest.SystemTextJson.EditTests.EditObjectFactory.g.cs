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

    internal class EditObjectFactory : FactoryEditBase<EditObject>, IEditObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public EditObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public EditObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public async Task<IEditObject> Create(Guid ID, string Name)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return await DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Create, () => target.Create(ID, Name));
        }

        public virtual async Task<IEditObject?> LocalUpdate(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return await DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Update, () => target.Update());
        }

        public override async Task<IEditBase?> Save(EditObject target)
        {
            return await Task.FromResult((IEditBase? )Save(target));
        }

        public virtual async Task<IEditObject?> Save(IEditObject target)
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
                return await LocalUpdate(target);
                ;
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