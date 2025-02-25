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
    }

    internal class BaseObjectFactory : FactoryBase, IBaseObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public Create2Delegate Create2Property { get; }
        public Fetch2Delegate Fetch2Property { get; }

        public delegate Task<IBaseObject> Create2Delegate(Guid criteria);
        public delegate Task<IBaseObject> Fetch2Delegate(Guid criteria);
        public BaseObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            Create2Property = LocalCreate2;
            Fetch2Property = LocalFetch2;
        }

        public BaseObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            Create2Property = RemoteCreate2;
            Fetch2Property = RemoteFetch2;
        }

        public virtual Task<IBaseObject> Create(Guid criteria)
        {
            return Create2Property(criteria);
        }

        public virtual Task<IBaseObject> Fetch(Guid criteria)
        {
            return Fetch2Property(criteria);
        }

        public async Task<IBaseObject> CreateAsync()
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            return await DoMapperMethodCallAsync<IBaseObject>(target, DataMapperMethod.Create, () => target.CreateAsync());
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

        public async Task<IBaseObject> LocalCreate2(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return await DoMapperMethodCallAsync<IBaseObject>(target, DataMapperMethod.Create, () => target.Create(criteria, dependency));
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

        public async Task<IBaseObject> LocalFetch2(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            return await DoMapperMethodCallAsync<IBaseObject>(target, DataMapperMethod.Fetch, () => target.Fetch(criteria, dependency));
        }

        public virtual async Task<IBaseObject> RemoteCreate2(Guid criteria)
        {
            return await DoRemoteRequest.ForDelegate<IBaseObject>(typeof(Create2Delegate), [criteria]);
        }

        public virtual async Task<IBaseObject> RemoteFetch2(Guid criteria)
        {
            return await DoRemoteRequest.ForDelegate<IBaseObject>(typeof(Fetch2Delegate), [criteria]);
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<BaseObject>();
            services.AddScoped<BaseObjectFactory>();
            services.AddScoped<IBaseObjectFactory, BaseObjectFactory>();
            services.AddTransient<IBaseObject, BaseObject>();
            services.AddScoped<Create2Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<BaseObjectFactory>();
                return (Guid criteria) => factory.LocalCreate2(criteria);
            });
            services.AddScoped<Fetch2Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<BaseObjectFactory>();
                return (Guid criteria) => factory.LocalFetch2(criteria);
            });
        }
    }
}