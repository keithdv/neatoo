using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
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
    }

    [Factory<IEditObject>]
    internal class EditObjectFactory : FactoryEditBase<EditObject>, IEditObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IEditObject?> SaveDelegate(IEditObject target);
        protected SaveDelegate SaveProperty { get; set; }

        public EditObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            SaveProperty = LocalSave;
        }

        public EditObjectFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            SaveProperty = RemoteSave;
        }

        public override async Task<IEditBase?> Save(EditObject target)
        {
            return await SaveProperty(target);
        }

        public Task<IEditObject?> Save(IEditObject target)
        {
            return SaveProperty(target);
        }

        public async Task<IEditObject> Create(Guid ID, string Name)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () => target.Create(ID, Name));
            return target;
        }

        protected async Task LocalUpdate(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update());
        }

        protected async Task LocalUpdate1(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            await DoMapperMethodCall(target, DataMapperMethod.Insert, () => target.Update());
        }

        [Local<SaveDelegate>]
        protected async Task<IEditObject?> LocalSave(IEditObject target)
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
                await LocalUpdate1(target);
            }
            else
            {
                await LocalUpdate(target);
            }

            return target;
        }

        protected async Task<IEditObject?> RemoteSave(IEditObject target)
        {
            return (IEditObject? )await DoRemoteRequest(typeof(SaveDelegate), [target, ]);
        }
    }
}