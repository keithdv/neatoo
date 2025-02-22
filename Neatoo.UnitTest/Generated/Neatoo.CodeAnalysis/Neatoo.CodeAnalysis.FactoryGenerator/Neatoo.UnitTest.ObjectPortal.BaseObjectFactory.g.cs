using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.UnitTest.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

/*
Debugging Messages:
: Base<BaseObject>, IBaseObject
*/
namespace Neatoo.UnitTest.ObjectPortal
{
    public interface IBaseObjectFactory
    {
        Task<IBaseObject> CreateAsync();
        IBaseObject Create(int criteria);
        IBaseObject Create(int i, string s);
        IBaseObject Create(int i, double d);
        Task<IBaseObject> Create(Guid criteria);
        IBaseObject Fetch();
        IBaseObject Fetch(int criteria);
        Task<IBaseObject> Fetch(Guid criteria);
        delegate Task<IBaseObject> CreateAsyncDelegate();
        delegate IBaseObject CreateDelegate(int criteria);
        delegate IBaseObject Create1Delegate(int i, string s);
        delegate IBaseObject Create2Delegate(int i, double d);
        delegate Task<IBaseObject> Create3Delegate(Guid criteria);
        delegate IBaseObject FetchDelegate();
        delegate IBaseObject Fetch1Delegate(int criteria);
        delegate Task<IBaseObject> Fetch2Delegate(Guid criteria);
    }

    internal class BaseObjectFactory : FactoryBase, IBaseObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IBaseObjectFactory.Create3Delegate Create3Property { get; }
        public IBaseObjectFactory.Fetch2Delegate Fetch2Property { get; }

        public BaseObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            Create3Property = LocalCreate3;
            Fetch2Property = LocalFetch2;
        }

        public BaseObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            Create3Property = RemoteCreate3;
            Fetch2Property = RemoteFetch2;
        }

        public virtual Task<IBaseObject> Create(Guid criteria)
        {
            return Create3Property(criteria);
        }

        public virtual Task<IBaseObject> Fetch(Guid criteria)
        {
            return Fetch2Property(criteria);
        }

        public Task<IBaseObject> CreateAsync()
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            return DoMapperMethodCallAsync<IBaseObject>(target, DataMapperMethod.Create, () => target.CreateAsync());
        }

        public IBaseObject Create(int criteria)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            return DoMapperMethodCall<IBaseObject>(target, DataMapperMethod.Create, () => target.Create(criteria));
        }

        public IBaseObject Create(int i, string s)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            return DoMapperMethodCall<IBaseObject>(target, DataMapperMethod.Create, () => target.Create(i, s));
        }

        public IBaseObject Create(int i, double d)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            var dep = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IBaseObject>(target, DataMapperMethod.Create, () => target.Create(i, d, dep));
        }

        public Task<IBaseObject> LocalCreate3(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCallAsync<IBaseObject>(target, DataMapperMethod.Create, () => target.Create(criteria, dependency));
        }

        public IBaseObject Fetch()
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            return DoMapperMethodCall<IBaseObject>(target, DataMapperMethod.Fetch, () => target.Fetch());
        }

        public IBaseObject Fetch(int criteria)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            return DoMapperMethodCall<IBaseObject>(target, DataMapperMethod.Fetch, () => target.Fetch(criteria));
        }

        public Task<IBaseObject> LocalFetch2(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCallAsync<IBaseObject>(target, DataMapperMethod.Fetch, () => target.Fetch(criteria, dependency));
        }

        public virtual async Task<IBaseObject?> RemoteCreate3(Guid criteria)
        {
            return await DoRemoteRequest.ForDelegate<BaseObject?>(typeof(IBaseObjectFactory.Create3Delegate), [criteria]);
        }

        public virtual async Task<IBaseObject?> RemoteFetch2(Guid criteria)
        {
            return await DoRemoteRequest.ForDelegate<BaseObject?>(typeof(IBaseObjectFactory.Fetch2Delegate), [criteria]);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<BaseObject>();
            services.AddTransient<IBaseObject, BaseObject>();
            services.AddScoped<BaseObjectFactory>();
            services.AddScoped<IBaseObjectFactory, BaseObjectFactory>();
            services.AddScoped<IBaseObjectFactory.CreateAsyncDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<BaseObjectFactory>();
                return () => factory.CreateAsync();
            });
            services.AddScoped<IBaseObjectFactory.CreateDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<BaseObjectFactory>();
                return (int criteria) => factory.Create(criteria);
            });
            services.AddScoped<IBaseObjectFactory.Create1Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<BaseObjectFactory>();
                return (int i, string s) => factory.Create(i, s);
            });
            services.AddScoped<IBaseObjectFactory.Create2Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<BaseObjectFactory>();
                return (int i, double d) => factory.Create(i, d);
            });
            services.AddScoped<IBaseObjectFactory.Create3Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<BaseObjectFactory>();
                return (Guid criteria) => factory.LocalCreate3(criteria);
            });
            services.AddScoped<IBaseObjectFactory.FetchDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<BaseObjectFactory>();
                return () => factory.Fetch();
            });
            services.AddScoped<IBaseObjectFactory.Fetch1Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<BaseObjectFactory>();
                return (int criteria) => factory.Fetch(criteria);
            });
            services.AddScoped<IBaseObjectFactory.Fetch2Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<BaseObjectFactory>();
                return (Guid criteria) => factory.LocalFetch2(criteria);
            });
        }
    }
}