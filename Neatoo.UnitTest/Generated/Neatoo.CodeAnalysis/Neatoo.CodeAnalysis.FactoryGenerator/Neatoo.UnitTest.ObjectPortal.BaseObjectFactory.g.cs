using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
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
        Task<IBaseObject> Create();
        Task<IBaseObject> Create(int criteria);
        Task<IBaseObject> Create(int i, string s);
        Task<IBaseObject> Create(int i, double d);
        Task<IBaseObject> Create(Guid criteria);
        Task<IBaseObject> Fetch();
        Task<IBaseObject> Fetch(int criteria);
        Task<IBaseObject> Fetch(Guid criteria);
    }

    [Factory<IBaseObject>]
    internal class BaseObjectFactory : FactoryBase<BaseObject>, IBaseObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        protected internal delegate Task<IBaseObject> Create3Delegate(int i, double d);
        protected internal delegate Task<IBaseObject> Create4Delegate(Guid criteria);
        protected internal delegate Task<IBaseObject> Fetch2Delegate(Guid criteria);
        protected Create3Delegate Create3Property { get; }
        protected Create4Delegate Create4Property { get; }
        protected Fetch2Delegate Fetch2Property { get; }

        public BaseObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            Create3Property = LocalCreate3;
            Create4Property = LocalCreate4;
            Fetch2Property = LocalFetch2;
        }

        public BaseObjectFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            Create3Property = RemoteCreate3;
            Create4Property = RemoteCreate4;
            Fetch2Property = RemoteFetch2;
        }

        public Task<IBaseObject> Create(int i, double d)
        {
            return Create3Property(i, d);
        }

        public Task<IBaseObject> Create(Guid criteria)
        {
            return Create4Property(criteria);
        }

        public Task<IBaseObject> Fetch(Guid criteria)
        {
            return Fetch2Property(criteria);
        }

        public async Task<IBaseObject> Create()
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () => target.Create());
            return target;
        }

        public async Task<IBaseObject> Create(int criteria)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create(criteria);
                return Task.CompletedTask;
            });
            return target;
        }

        public async Task<IBaseObject> Create(int i, string s)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create(i, s);
                return Task.CompletedTask;
            });
            return target;
        }

        [Local<Create3Delegate>]
        protected async Task<IBaseObject> LocalCreate3(int i, double d)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            var dep = ServiceProvider.GetService<IDisposableDependency>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create(i, d, dep);
                return Task.CompletedTask;
            });
            return target;
        }

        [Local<Create4Delegate>]
        protected async Task<IBaseObject> LocalCreate4(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            await DoMapperMethodCall(target, DataMapperMethod.Create, () =>
            {
                target.Create(criteria, dependency);
                return Task.CompletedTask;
            });
            return target;
        }

        public async Task<IBaseObject> Fetch()
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () =>
            {
                target.Fetch();
                return Task.CompletedTask;
            });
            return target;
        }

        public async Task<IBaseObject> Fetch(int criteria)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () =>
            {
                target.Fetch(criteria);
                return Task.CompletedTask;
            });
            return target;
        }

        [Local<Fetch2Delegate>]
        protected async Task<IBaseObject> LocalFetch2(Guid criteria)
        {
            var target = ServiceProvider.GetRequiredService<BaseObject>();
            var dependency = ServiceProvider.GetService<IDisposableDependency>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () =>
            {
                target.Fetch(criteria, dependency);
                return Task.CompletedTask;
            });
            return target;
        }

        protected async Task<IBaseObject?> RemoteCreate3(int i, double d)
        {
            return (IBaseObject? )await DoRemoteRequest(typeof(Create3Delegate), [i, d]);
        }

        protected async Task<IBaseObject?> RemoteCreate4(Guid criteria)
        {
            return (IBaseObject? )await DoRemoteRequest(typeof(Create4Delegate), [criteria]);
        }

        protected async Task<IBaseObject?> RemoteFetch2(Guid criteria)
        {
            return (IBaseObject? )await DoRemoteRequest(typeof(Fetch2Delegate), [criteria]);
        }
    }
}