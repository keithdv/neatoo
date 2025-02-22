using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        IEditObject Create();
        Task<IEditObject> CreateAsync(int criteria);
        IEditObject Create(Guid criteria);
        Task<IEditObject> CreateRemote(Guid criteria);
        IEditObject Fetch();
        Task<IEditObject> Fetch(int criteria);
        IEditObject Fetch(Guid criteria);
        Task<IEditObject> FetchRemote(Guid criteria);
        IEditObject? FetchFail();
        Task<IEditObject?> FetchFailAsync();
        Task<IEditObject?> FetchFailDependency();
        Task<IEditObject?> FetchFailAsyncDependency();
        IEditObject? Save(IEditObject target);
        Task<IEditObject?> Save(IEditObject target, int criteria);
        Task<IEditObject?> Save(IEditObject target, Guid criteria);
        delegate IEditObject CreateDelegate();
        delegate Task<IEditObject> CreateAsyncDelegate(int criteria);
        delegate IEditObject Create1Delegate(Guid criteria);
        delegate Task<IEditObject> CreateRemoteDelegate(Guid criteria);
        delegate IEditObject FetchDelegate();
        delegate Task<IEditObject> Fetch1Delegate(int criteria);
        delegate IEditObject Fetch2Delegate(Guid criteria);
        delegate Task<IEditObject> FetchRemoteDelegate(Guid criteria);
        delegate IEditObject? FetchFailDelegate();
        delegate Task<IEditObject?> FetchFailAsyncDelegate();
        delegate Task<IEditObject?> FetchFailDependencyDelegate();
        delegate Task<IEditObject?> FetchFailAsyncDependencyDelegate();
        delegate IEditObject? SaveDelegate(IEditObject target);
        delegate Task<IEditObject?> Save1Delegate(IEditObject target, int criteria);
        delegate Task<IEditObject?> Save2Delegate(IEditObject target, Guid criteria);
    }

    internal class EditObjectFactory : FactoryEditBase<EditObject>, IEditObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IEditObjectFactory.CreateRemoteDelegate CreateRemoteProperty { get; }
        public IEditObjectFactory.FetchRemoteDelegate FetchRemoteProperty { get; }
        public IEditObjectFactory.FetchFailDependencyDelegate FetchFailDependencyProperty { get; }
        public IEditObjectFactory.FetchFailAsyncDependencyDelegate FetchFailAsyncDependencyProperty { get; }
        public IEditObjectFactory.SaveDelegate SaveProperty { get; set; }
        public IEditObjectFactory.Save1Delegate Save1Property { get; set; }
        public IEditObjectFactory.Save2Delegate Save2Property { get; set; }

        public EditObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateRemoteProperty = LocalCreateRemote;
            FetchRemoteProperty = LocalFetchRemote;
            FetchFailDependencyProperty = LocalFetchFailDependency;
            FetchFailAsyncDependencyProperty = LocalFetchFailAsyncDependency;
            SaveProperty = LocalSave;
            Save1Property = LocalSave1;
            Save2Property = LocalSave2;
        }

        public EditObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateRemoteProperty = RemoteCreateRemote;
            FetchRemoteProperty = RemoteFetchRemote;
            FetchFailDependencyProperty = RemoteFetchFailDependency;
            FetchFailAsyncDependencyProperty = RemoteFetchFailAsyncDependency;
            Save1Property = RemoteSave1;
            Save2Property = RemoteSave2;
        }

        public virtual Task<IEditObject> CreateRemote(Guid criteria)
        {
            return CreateRemoteProperty(criteria);
        }

        public virtual Task<IEditObject> FetchRemote(Guid criteria)
        {
            return FetchRemoteProperty(criteria);
        }

        public virtual Task<IEditObject?> FetchFailDependency()
        {
            return FetchFailDependencyProperty();
        }

        public virtual Task<IEditObject?> FetchFailAsyncDependency()
        {
            return FetchFailAsyncDependencyProperty();
        }

        public override Task<IEditBase?> Save(EditObject target)
        {
            return Task.FromResult<IEditBase?>(SaveProperty(target));
        }

        public IEditObject? Save(IEditObject target)
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

        public IEditObject Create()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Create, () => target.Create());
        }

        public Task<IEditObject> CreateAsync(int criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Create, () => target.CreateAsync(criteria));
        }

        public IEditObject Create(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Create, () => target.Create(criteria, dependency));
        }

        public Task<IEditObject> LocalCreateRemote(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Create, () => target.CreateRemote(criteria, dependency));
        }

        public IEditObject Fetch()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Fetch, () => target.Fetch());
        }

        public Task<IEditObject> Fetch(int criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Fetch, () => target.Fetch(criteria));
        }

        public IEditObject Fetch(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Fetch, () => target.Fetch(criteria, dependency));
        }

        public Task<IEditObject> LocalFetchRemote(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Fetch, () => target.FetchRemote(criteria, dependency));
        }

        public IEditObject? FetchFail()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCallBool<IEditObject?>(target, DataMapperMethod.Fetch, () => target.FetchFail());
        }

        public Task<IEditObject?> FetchFailAsync()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCallBoolAsync<IEditObject?>(target, DataMapperMethod.Fetch, () => target.FetchFailAsync());
        }

        public Task<IEditObject?> LocalFetchFailDependency()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCallBoolAsync<IEditObject?>(target, DataMapperMethod.Fetch, () => target.FetchFailDependency(dependency));
        }

        public Task<IEditObject?> LocalFetchFailAsyncDependency()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCallBoolAsync<IEditObject?>(target, DataMapperMethod.Fetch, () => target.FetchFailAsyncDependency(dependency));
        }

        public virtual IEditObject? LocalInsert(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Insert, () => target.Insert());
        }

        public virtual Task<IEditObject?> LocalInsert1(IEditObject itarget, int criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Insert, () => target.Insert(criteria));
        }

        public virtual IEditObject? LocalInsert2(IEditObject itarget, Guid criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Insert, () => target.Insert(criteria, dependency));
        }

        public virtual IEditObject? LocalUpdate(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Update, () => target.Update());
        }

        public virtual Task<IEditObject?> LocalUpdate1(IEditObject itarget, int criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Update, () => target.Update(criteria));
        }

        public virtual Task<IEditObject?> LocalUpdate2(IEditObject itarget, Guid criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Update, () => target.Update(criteria, dependency));
        }

        public virtual IEditObject? LocalDelete(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Delete, () => target.Delete());
        }

        public virtual Task<IEditObject?> LocalDelete1(IEditObject itarget, int criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Delete, () => target.Delete(criteria));
        }

        public virtual IEditObject? LocalDelete2(IEditObject itarget, Guid criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Delete, () => target.Delete(criteria, dependency));
        }

        public virtual async Task<IEditObject?> RemoteCreateRemote(Guid criteria)
        {
            return await DoRemoteRequest.ForDelegate<EditObject?>(typeof(IEditObjectFactory.CreateRemoteDelegate), [criteria]);
        }

        public virtual async Task<IEditObject?> RemoteFetchRemote(Guid criteria)
        {
            return await DoRemoteRequest.ForDelegate<EditObject?>(typeof(IEditObjectFactory.FetchRemoteDelegate), [criteria]);
        }

        public virtual async Task<IEditObject?> RemoteFetchFailDependency()
        {
            return await DoRemoteRequest.ForDelegate<EditObject?>(typeof(IEditObjectFactory.FetchFailDependencyDelegate), []);
        }

        public virtual async Task<IEditObject?> RemoteFetchFailAsyncDependency()
        {
            return await DoRemoteRequest.ForDelegate<EditObject?>(typeof(IEditObjectFactory.FetchFailAsyncDependencyDelegate), []);
        }

        public virtual IEditObject? LocalSave(IEditObject target)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return LocalDelete(target);
            }
            else if (target.IsNew)
            {
                return LocalInsert(target);
            }
            else
            {
                return LocalUpdate(target);
            }
        }

        public virtual async Task<IEditObject?> LocalSave1(IEditObject target, int criteria)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return await LocalDelete1(target, criteria);
            }
            else if (target.IsNew)
            {
                return await LocalInsert1(target, criteria);
            }
            else
            {
                return await LocalUpdate1(target, criteria);
            }
        }

        public async Task<IEditObject?> RemoteSave1(IEditObject target, int criteria)
        {
            return await DoRemoteRequest.ForDelegate<EditObject?>(typeof(IEditObjectFactory.Save1Delegate), [target, criteria]);
        }

        public virtual async Task<IEditObject?> LocalSave2(IEditObject target, Guid criteria)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return LocalDelete2(target, criteria);
            }
            else if (target.IsNew)
            {
                return LocalInsert2(target, criteria);
            }
            else
            {
                return await LocalUpdate2(target, criteria);
            }
        }

        public async Task<IEditObject?> RemoteSave2(IEditObject target, Guid criteria)
        {
            return await DoRemoteRequest.ForDelegate<EditObject?>(typeof(IEditObjectFactory.Save2Delegate), [target, criteria]);
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
                return () => factory.Create();
            });
            services.AddScoped<IEditObjectFactory.CreateAsyncDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (int criteria) => factory.CreateAsync(criteria);
            });
            services.AddScoped<IEditObjectFactory.Create1Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (Guid criteria) => factory.Create(criteria);
            });
            services.AddScoped<IEditObjectFactory.CreateRemoteDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (Guid criteria) => factory.LocalCreateRemote(criteria);
            });
            services.AddScoped<IEditObjectFactory.FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return () => factory.Fetch();
            });
            services.AddScoped<IEditObjectFactory.Fetch1Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (int criteria) => factory.Fetch(criteria);
            });
            services.AddScoped<IEditObjectFactory.Fetch2Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (Guid criteria) => factory.Fetch(criteria);
            });
            services.AddScoped<IEditObjectFactory.FetchRemoteDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (Guid criteria) => factory.LocalFetchRemote(criteria);
            });
            services.AddScoped<IEditObjectFactory.FetchFailDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return () => factory.FetchFail();
            });
            services.AddScoped<IEditObjectFactory.FetchFailAsyncDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return () => factory.FetchFailAsync();
            });
            services.AddScoped<IEditObjectFactory.FetchFailDependencyDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return () => factory.LocalFetchFailDependency();
            });
            services.AddScoped<IEditObjectFactory.FetchFailAsyncDependencyDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return () => factory.LocalFetchFailAsyncDependency();
            });
            services.AddScoped<IEditObjectFactory.Save1Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (target, criteria) => factory.LocalSave1(target, criteria);
            });
            services.AddScoped<IEditObjectFactory.Save2Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (target, criteria) => factory.LocalSave2(target, criteria);
            });
            services.AddScoped<IFactoryEditBase<EditObject>, EditObjectFactory>();
        }
    }
}