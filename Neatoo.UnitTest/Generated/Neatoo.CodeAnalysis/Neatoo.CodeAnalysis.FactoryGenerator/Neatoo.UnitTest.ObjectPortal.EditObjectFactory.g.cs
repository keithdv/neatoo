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
No DataMapperMethod attribute for MarkAsChild
No DataMapperMethod attribute for MarkDeleted
No DataMapperMethod attribute for MarkNew
No DataMapperMethod attribute for MarkOld
No DataMapperMethod attribute for MarkUnmodified
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
        Task<IEditObject?> Save(IEditObject target);
        Task<IEditObject?> Save(IEditObject target, int criteriaA);
        Task<IEditObject?> Save(IEditObject target, Guid criteria);
    }

    internal class EditObjectFactory : FactoryEditBase<EditObject>, IFactoryEditBase<EditObject>, IEditObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        public delegate Task<IEditObject> CreateRemoteDelegate(Guid criteria);
        public delegate Task<IEditObject> FetchRemoteDelegate(Guid criteria);
        public delegate Task<IEditObject?> FetchFailDependencyDelegate();
        public delegate Task<IEditObject?> FetchFailAsyncDependencyDelegate();
        public delegate Task<IEditObject?> Save2Delegate(IEditObject target, Guid criteria);
        // Delegate Properties to provide Local or Remote fork in execution
        public CreateRemoteDelegate CreateRemoteProperty { get; }
        public FetchRemoteDelegate FetchRemoteProperty { get; }
        public FetchFailDependencyDelegate FetchFailDependencyProperty { get; }
        public FetchFailAsyncDependencyDelegate FetchFailAsyncDependencyProperty { get; }
        public Save2Delegate Save2Property { get; }

        public EditObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateRemoteProperty = LocalCreateRemote;
            FetchRemoteProperty = LocalFetchRemote;
            FetchFailDependencyProperty = LocalFetchFailDependency;
            FetchFailAsyncDependencyProperty = LocalFetchFailAsyncDependency;
            Save2Property = LocalSave2;
        }

        public EditObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateRemoteProperty = RemoteCreateRemote;
            FetchRemoteProperty = RemoteFetchRemote;
            FetchFailDependencyProperty = RemoteFetchFailDependency;
            FetchFailAsyncDependencyProperty = RemoteFetchFailAsyncDependency;
            Save2Property = RemoteSave2;
        }

        public virtual IEditObject Create()
        {
            return LocalCreate();
        }

        public IEditObject LocalCreate()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Create, () => target.Create());
        }

        public virtual Task<IEditObject> CreateAsync(int criteria)
        {
            return LocalCreateAsync(criteria);
        }

        public Task<IEditObject> LocalCreateAsync(int criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Create, () => target.CreateAsync(criteria));
        }

        public virtual IEditObject Create(Guid criteria)
        {
            return LocalCreate1(criteria);
        }

        public IEditObject LocalCreate1(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Create, () => target.Create(criteria, dependency));
        }

        public virtual Task<IEditObject> CreateRemote(Guid criteria)
        {
            return CreateRemoteProperty(criteria);
        }

        public virtual async Task<IEditObject> RemoteCreateRemote(Guid criteria)
        {
            return await DoRemoteRequest.ForDelegate<IEditObject>(typeof(CreateRemoteDelegate), [criteria]);
        }

        public Task<IEditObject> LocalCreateRemote(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return Task.FromResult(DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Create, () => target.CreateRemote(criteria, dependency)));
        }

        public virtual IEditObject Fetch()
        {
            return LocalFetch();
        }

        public IEditObject LocalFetch()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Fetch, () => target.Fetch());
        }

        public virtual Task<IEditObject> Fetch(int criteria)
        {
            return LocalFetch1(criteria);
        }

        public Task<IEditObject> LocalFetch1(int criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Fetch, () => target.Fetch(criteria));
        }

        public virtual IEditObject Fetch(Guid criteria)
        {
            return LocalFetch2(criteria);
        }

        public IEditObject LocalFetch2(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Fetch, () => target.Fetch(criteria, dependency));
        }

        public virtual Task<IEditObject> FetchRemote(Guid criteria)
        {
            return FetchRemoteProperty(criteria);
        }

        public virtual async Task<IEditObject> RemoteFetchRemote(Guid criteria)
        {
            return await DoRemoteRequest.ForDelegate<IEditObject>(typeof(FetchRemoteDelegate), [criteria]);
        }

        public Task<IEditObject> LocalFetchRemote(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return Task.FromResult(DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Fetch, () => target.FetchRemote(criteria, dependency)));
        }

        public virtual IEditObject? FetchFail()
        {
            return LocalFetchFail();
        }

        public IEditObject? LocalFetchFail()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCallBool<IEditObject>(target, DataMapperMethod.Fetch, () => target.FetchFail());
        }

        public virtual Task<IEditObject?> FetchFailAsync()
        {
            return LocalFetchFailAsync();
        }

        public Task<IEditObject?> LocalFetchFailAsync()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCallBoolAsync<IEditObject>(target, DataMapperMethod.Fetch, () => target.FetchFailAsync());
        }

        public virtual Task<IEditObject?> FetchFailDependency()
        {
            return FetchFailDependencyProperty();
        }

        public virtual async Task<IEditObject?> RemoteFetchFailDependency()
        {
            return await DoRemoteRequest.ForDelegate<IEditObject?>(typeof(FetchFailDependencyDelegate), []);
        }

        public Task<IEditObject?> LocalFetchFailDependency()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return Task.FromResult(DoMapperMethodCallBool<IEditObject>(target, DataMapperMethod.Fetch, () => target.FetchFailDependency(dependency)));
        }

        public virtual Task<IEditObject?> FetchFailAsyncDependency()
        {
            return FetchFailAsyncDependencyProperty();
        }

        public virtual async Task<IEditObject?> RemoteFetchFailAsyncDependency()
        {
            return await DoRemoteRequest.ForDelegate<IEditObject?>(typeof(FetchFailAsyncDependencyDelegate), []);
        }

        public Task<IEditObject?> LocalFetchFailAsyncDependency()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCallBoolAsync<IEditObject>(target, DataMapperMethod.Fetch, () => target.FetchFailAsyncDependency(dependency));
        }

        public virtual IEditObject? LocalInsert(IEditObject target)
        {
            var cTarget = (EditObject)target ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCall<IEditObject>(cTarget, DataMapperMethod.Insert, () => cTarget.Insert());
        }

        public virtual Task<IEditObject?> LocalUpdate(IEditObject target)
        {
            var cTarget = (EditObject)target ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCallAsync<IEditObject>(cTarget, DataMapperMethod.Update, () => cTarget.Update());
        }

        public virtual IEditObject? LocalDelete(IEditObject target)
        {
            var cTarget = (EditObject)target ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCall<IEditObject>(cTarget, DataMapperMethod.Delete, () => cTarget.Delete());
        }

        public virtual Task<IEditObject?> LocalInsert1(IEditObject target, int criteriaA)
        {
            var cTarget = (EditObject)target ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCallAsync<IEditObject>(cTarget, DataMapperMethod.Insert, () => cTarget.Insert(criteriaA));
        }

        public virtual Task<IEditObject?> LocalUpdate1(IEditObject target, int criteriaB)
        {
            var cTarget = (EditObject)target ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCallAsync<IEditObject>(cTarget, DataMapperMethod.Update, () => cTarget.Update(criteriaB));
        }

        public virtual Task<IEditObject?> LocalDelete1(IEditObject target, int criteriaC)
        {
            var cTarget = (EditObject)target ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCallAsync<IEditObject>(cTarget, DataMapperMethod.Delete, () => cTarget.Delete(criteriaC));
        }

        public virtual IEditObject? LocalInsert2(IEditObject target, Guid criteria)
        {
            var cTarget = (EditObject)target ?? throw new Exception("EditObject must implement IEditObject");
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IEditObject>(cTarget, DataMapperMethod.Insert, () => cTarget.Insert(criteria, dependency));
        }

        public virtual Task<IEditObject?> LocalUpdate2(IEditObject target, Guid criteria)
        {
            var cTarget = (EditObject)target ?? throw new Exception("EditObject must implement IEditObject");
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCallAsync<IEditObject>(cTarget, DataMapperMethod.Update, () => cTarget.Update(criteria, dependency));
        }

        public virtual IEditObject? LocalDelete2(IEditObject target, Guid criteria)
        {
            var cTarget = (EditObject)target ?? throw new Exception("EditObject must implement IEditObject");
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IEditObject>(cTarget, DataMapperMethod.Delete, () => cTarget.Delete(criteria, dependency));
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

                return Task.FromResult(LocalDelete(target));
            }
            else if (target.IsNew)
            {
                return Task.FromResult(LocalInsert(target));
            }
            else
            {
                return LocalUpdate(target);
            }
        }

        public virtual Task<IEditObject?> Save(IEditObject target, int criteriaA)
        {
            return LocalSave1(target, criteriaA);
        }

        public virtual Task<IEditObject?> LocalSave1(IEditObject target, int criteriaA)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return LocalDelete1(target, criteriaA);
            }
            else if (target.IsNew)
            {
                return LocalInsert1(target, criteriaA);
            }
            else
            {
                return LocalUpdate1(target, criteriaA);
            }
        }

        public virtual Task<IEditObject?> Save(IEditObject target, Guid criteria)
        {
            return Save2Property(target, criteria);
        }

        public virtual async Task<IEditObject?> RemoteSave2(IEditObject target, Guid criteria)
        {
            return await DoRemoteRequest.ForDelegate<IEditObject?>(typeof(Save2Delegate), [target, criteria]);
        }

        public virtual Task<IEditObject?> LocalSave2(IEditObject target, Guid criteria)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return Task.FromResult(LocalDelete2(target, criteria));
            }
            else if (target.IsNew)
            {
                return Task.FromResult(LocalInsert2(target, criteria));
            }
            else
            {
                return LocalUpdate2(target, criteria);
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<EditObject>();
            services.AddScoped<EditObjectFactory>();
            services.AddScoped<IEditObjectFactory, EditObjectFactory>();
            services.AddTransient<IEditObject, EditObject>();
            services.AddScoped<CreateRemoteDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (Guid criteria) => factory.LocalCreateRemote(criteria);
            });
            services.AddScoped<FetchRemoteDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (Guid criteria) => factory.LocalFetchRemote(criteria);
            });
            services.AddScoped<FetchFailDependencyDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return () => factory.LocalFetchFailDependency();
            });
            services.AddScoped<FetchFailAsyncDependencyDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return () => factory.LocalFetchFailAsyncDependency();
            });
            services.AddScoped<Save2Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditObjectFactory>();
                return (IEditObject target, Guid criteria) => factory.LocalSave2(target, criteria);
            });
            services.AddScoped<IFactoryEditBase<EditObject>, EditObjectFactory>();
        }
    }
}