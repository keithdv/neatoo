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

    internal class EditObjectFactory : FactoryEditBase<EditObject>, IEditObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public CreateRemoteDelegate CreateRemoteProperty { get; }
        public FetchRemoteDelegate FetchRemoteProperty { get; }
        public FetchFailDependencyDelegate FetchFailDependencyProperty { get; }
        public FetchFailAsyncDependencyDelegate FetchFailAsyncDependencyProperty { get; }

        public delegate Task<IEditObject> CreateRemoteDelegate(Guid criteria);
        public delegate Task<IEditObject> FetchRemoteDelegate(Guid criteria);
        public delegate Task<IEditObject?> FetchFailDependencyDelegate();
        public delegate Task<IEditObject?> FetchFailAsyncDependencyDelegate();
        public EditObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            CreateRemoteProperty = LocalCreateRemote;
            FetchRemoteProperty = LocalFetchRemote;
            FetchFailDependencyProperty = LocalFetchFailDependency;
            FetchFailAsyncDependencyProperty = LocalFetchFailAsyncDependency;
        }

        public EditObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            CreateRemoteProperty = RemoteCreateRemote;
            FetchRemoteProperty = RemoteFetchRemote;
            FetchFailDependencyProperty = RemoteFetchFailDependency;
            FetchFailAsyncDependencyProperty = RemoteFetchFailAsyncDependency;
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

        public IEditObject Create()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Create, () => target.Create());
        }

        public async Task<IEditObject> CreateAsync(int criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return await DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Create, () => target.CreateAsync(criteria));
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

        public async Task<IEditObject> Fetch(int criteria)
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return await DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Fetch, () => target.Fetch(criteria));
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
            return DoMapperMethodCallBool<IEditObject>(target, DataMapperMethod.Fetch, () => target.FetchFail());
        }

        public async Task<IEditObject?> FetchFailAsync()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            return await DoMapperMethodCallBoolAsync<IEditObject>(target, DataMapperMethod.Fetch, () => target.FetchFailAsync());
        }

        public Task<IEditObject?> LocalFetchFailDependency()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCallBoolAsync<IEditObject>(target, DataMapperMethod.Fetch, () => target.FetchFailDependency(dependency));
        }

        public async Task<IEditObject?> LocalFetchFailAsyncDependency()
        {
            var target = ServiceProvider.GetRequiredService<EditObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return await DoMapperMethodCallBoolAsync<IEditObject>(target, DataMapperMethod.Fetch, () => target.FetchFailAsyncDependency(dependency));
        }

        public virtual IEditObject? LocalInsert(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Insert, () => target.Insert());
        }

        public virtual async Task<IEditObject?> LocalInsert1(IEditObject itarget, int criteriaA)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return await DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Insert, () => target.Insert(criteriaA));
        }

        public virtual IEditObject? LocalInsert2(IEditObject itarget, Guid criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Insert, () => target.Insert(criteria, dependency));
        }

        public virtual async Task<IEditObject?> LocalUpdate(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return await DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Update, () => target.Update());
        }

        public virtual async Task<IEditObject?> LocalUpdate1(IEditObject itarget, int criteriaB)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return await DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Update, () => target.Update(criteriaB));
        }

        public virtual async Task<IEditObject?> LocalUpdate2(IEditObject itarget, Guid criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return await DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Update, () => target.Update(criteria, dependency));
        }

        public virtual IEditObject? LocalDelete(IEditObject itarget)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Delete, () => target.Delete());
        }

        public virtual async Task<IEditObject?> LocalDelete1(IEditObject itarget, int criteriaC)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            return await DoMapperMethodCallAsync<IEditObject>(target, DataMapperMethod.Delete, () => target.Delete(criteriaC));
        }

        public virtual IEditObject? LocalDelete2(IEditObject itarget, Guid criteria)
        {
            var target = (EditObject)itarget ?? throw new Exception("EditObject must implement IEditObject");
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IEditObject>(target, DataMapperMethod.Delete, () => target.Delete(criteria, dependency));
        }

        public virtual async Task<IEditObject> RemoteCreateRemote(Guid criteria)
        {
            return await DoRemoteRequest.ForDelegate<EditObject>(typeof(CreateRemoteDelegate), [criteria]);
        }

        public virtual async Task<IEditObject> RemoteFetchRemote(Guid criteria)
        {
            return await DoRemoteRequest.ForDelegate<EditObject>(typeof(FetchRemoteDelegate), [criteria]);
        }

        public virtual async Task<IEditObject?> RemoteFetchFailDependency()
        {
            return await DoRemoteRequest.ForDelegate<EditObject>(typeof(FetchFailDependencyDelegate), []);
        }

        public virtual async Task<IEditObject?> RemoteFetchFailAsyncDependency()
        {
            return await DoRemoteRequest.ForDelegate<EditObject>(typeof(FetchFailAsyncDependencyDelegate), []);
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

                return LocalDelete(target);
                ;
            }
            else if (target.IsNew)
            {
                return LocalInsert(target);
                ;
            }
            else
            {
                return await LocalUpdate(target);
                ;
            }
        }

        public virtual async Task<IEditObject?> Save(IEditObject target, int criteriaA)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return await LocalDelete1(target, criteriaA);
                ;
            }
            else if (target.IsNew)
            {
                return await LocalInsert1(target, criteriaA);
                ;
            }
            else
            {
                return await LocalUpdate1(target, criteriaA);
                ;
            }
        }

        public virtual async Task<IEditObject?> Save(IEditObject target, Guid criteria)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return LocalDelete2(target, criteria);
                ;
            }
            else if (target.IsNew)
            {
                return LocalInsert2(target, criteria);
                ;
            }
            else
            {
                return await LocalUpdate2(target, criteria);
                ;
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
            services.AddScoped<IFactoryEditBase<EditObject>, EditObjectFactory>();
        }
    }
}