using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;

/*
Debugging Messages:
: EditBase<EditObject>, IEditObject
*/
namespace Neatoo.UnitTest.ObjectPortal
{
    public interface IEditObjectFactory
    {
        Task<IEditObject> Create();
        Task<IEditObject> Create(int criteria);
        Task<IEditObject> Create(Guid criteria);
        Task<IEditObject> Fetch();
        Task<IEditObject> Fetch(int criteria);
        Task<IEditObject> Fetch(Guid criteria);
        Task<IEditObject?> Save(IEditObject target);
        Task<IEditObject?> Save(IEditObject target, int criteria);
        Task<IEditObject?> Save(IEditObject target, Guid criteria);
    }

    [Factory<IEditObject>]
    internal class EditObjectFactory : FactoryEditBase<EditObject>, IEditObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IEditObject> Create2Delegate(Guid criteria);
        protected internal delegate Task<IEditObject> Fetch2Delegate(Guid criteria);
        protected internal delegate Task<IEditObject?> SaveDelegate(IEditObject target);
        protected internal delegate Task<IEditObject?> Save1Delegate(IEditObject target, int criteria);
        protected internal delegate Task<IEditObject?> Save2Delegate(IEditObject target, Guid criteria);
        protected Create2Delegate Create2Property { get; }
        protected Fetch2Delegate Fetch2Property { get; }
        protected SaveDelegate SaveProperty { get; set; }
        protected Save1Delegate Save1Property { get; set; }
        protected Save2Delegate Save2Property { get; set; }

        public EditObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            Create2Property = LocalCreate2;
            Fetch2Property = LocalFetch2;
            SaveProperty = LocalSave;
            Save1Property = LocalSave1;
            Save2Property = LocalSave2;
        }

        public EditObjectFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            Create2Property = RemoteCreate2;
            Fetch2Property = RemoteFetch2;
            SaveProperty = RemoteSave;
            Save1Property = RemoteSave1;
            Save2Property = RemoteSave2;
        }

        public Task<IEditObject> Create(Guid criteria)
        {
            return Create2Property(criteria);
        }

        public Task<IEditObject> Fetch(Guid criteria)
        {
            return Fetch2Property(criteria);
        }

        public override async Task<IEditBase?> Save(EditObject target)
        {
            return await SaveProperty(target);
        }

        public Task<IEditObject?> Save(IEditObject target)
        {
            return SaveProperty(target);
        }

        public Task<IEditObject?> Save(IEditObject target, int criteria)
        {
            return Save1Property(target, criteria);
        }

        public Task<IEditObject?> Save(IEditObject target, Guid criteria)
        {
            return Save2Property(target, criteria);
        }

        public async Task<IEditObject> Create()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create();
                return Task.CompletedTask;
            });
            return target;
        }

        public async Task<IEditObject> Create(int criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create(criteria);
                return Task.CompletedTask;
            });
            return target;
        }

        [Local<Create2Delegate>]
        protected async Task<IEditObject> LocalCreate2(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create(criteria, dependency);
                return Task.CompletedTask;
            });
            return target;
        }

        public async Task<IEditObject> Fetch()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () =>
            {
                target.Fetch();
                return Task.CompletedTask;
            });
            return target;
        }

        public async Task<IEditObject> Fetch(int criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () =>
            {
                target.Fetch(criteria);
                return Task.CompletedTask;
            });
            return target;
        }

        [Local<Fetch2Delegate>]
        protected async Task<IEditObject> LocalFetch2(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () =>
            {
                target.Fetch(criteria, dependency);
                return Task.CompletedTask;
            });
            return target;
        }

        protected async Task LocalInsert(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            await DoMapperMethodCall(target, DataMapperMethod.Insert, () => target.Insert());
        }

        protected async Task LocalInsert1(IEditObject itarget, int criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            await DoMapperMethodCall(target, DataMapperMethod.Insert, () => target.Insert(criteria));
        }

        protected async Task LocalInsert2(IEditObject itarget, Guid criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            await DoMapperMethodCall(target, DataMapperMethod.Insert, () => target.Insert(criteria, dependency));
        }

        protected async Task LocalUpdate(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update());
        }

        protected async Task LocalUpdate1(IEditObject itarget, int criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(criteria));
        }

        protected async Task LocalUpdate2(IEditObject itarget, Guid criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            await DoMapperMethodCall(target, DataMapperMethod.Update, () => target.Update(criteria, dependency));
        }

        protected async Task LocalDelete(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            await DoMapperMethodCall(target, DataMapperMethod.Delete, () => target.Delete());
        }

        protected async Task LocalDelete1(IEditObject itarget, int criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            await DoMapperMethodCall(target, DataMapperMethod.Delete, () => target.Delete(criteria));
        }

        protected async Task LocalDelete2(IEditObject itarget, Guid criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            await DoMapperMethodCall(target, DataMapperMethod.Delete, () => target.Delete(criteria, dependency));
        }

        protected async Task<IEditObject?> RemoteCreate2(Guid criteria)
        {
            return (IEditObject? )await DoRemoteRequest(typeof(Create2Delegate), [criteria]);
        }

        protected async Task<IEditObject?> RemoteFetch2(Guid criteria)
        {
            return (IEditObject? )await DoRemoteRequest(typeof(Fetch2Delegate), [criteria]);
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

                await LocalDelete(target);
            }
            else if (target.IsNew)
            {
                await LocalInsert(target);
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

        [Local<Save1Delegate>]
        protected async Task<IEditObject?> LocalSave1(IEditObject target, int criteria)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                await LocalDelete1(target, criteria);
            }
            else if (target.IsNew)
            {
                await LocalInsert1(target, criteria);
            }
            else
            {
                await LocalUpdate1(target, criteria);
            }

            return target;
        }

        protected async Task<IEditObject?> RemoteSave1(IEditObject target, int criteria)
        {
            return (IEditObject? )await DoRemoteRequest(typeof(Save1Delegate), [target, criteria]);
        }

        [Local<Save2Delegate>]
        protected async Task<IEditObject?> LocalSave2(IEditObject target, Guid criteria)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                await LocalDelete2(target, criteria);
            }
            else if (target.IsNew)
            {
                await LocalInsert2(target, criteria);
            }
            else
            {
                await LocalUpdate2(target, criteria);
            }

            return target;
        }

        protected async Task<IEditObject?> RemoteSave2(IEditObject target, Guid criteria)
        {
            return (IEditObject? )await DoRemoteRequest(typeof(Save2Delegate), [target, criteria]);
        }
    }
}