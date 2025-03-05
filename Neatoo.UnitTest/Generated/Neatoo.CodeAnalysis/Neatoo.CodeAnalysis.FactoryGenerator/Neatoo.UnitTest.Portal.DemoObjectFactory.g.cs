using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using static Neatoo.UnitTest.Portal.AuthorizationClassTests;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neatoo.AuthorizationRules;
using Neatoo.Internal;
using Neatoo.Portal.Internal;
using Neatoo.UnitTest.ObjectPortal;
using Neatoo.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

/*
Debugging Messages:
Parent class: AuthorizationClassTests
For IAuthorization using IAuthorization
: IDemoObject
*/
namespace Neatoo.UnitTest.Portal
{
    public interface IDemoObjectFactory
    {
        IDemoObject Create();
        Authorized<IDemoObject> TryCreate();
        Authorized CanCreate();
        IDemoObject? CreateCanReturnNull();
        Authorized<IDemoObject> TryCreateCanReturnNull();
        Authorized CanCreateCanReturnNull();
        Task<IDemoObject> CreateAsync();
        Task<Authorized<IDemoObject>> TryCreateAsync();
        Task<Authorized> CanCreateAsync();
        Task<IDemoObject?> CreateRemote();
        Task<Authorized<IDemoObject>> TryCreateRemote();
        Task<Authorized> CanCreateRemote();
        Task<IDemoObject?> Save(IDemoObject target);
        Task<Authorized<IDemoObject>> TrySave(IDemoObject target);
        Task<Authorized> CanSave(IDemoObject target);
    }

    internal class DemoObjectFactory : FactoryEditBase<DemoObject>, IFactoryEditBase<DemoObject>, IDemoObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        public delegate Task<Authorized<IDemoObject>> CreateRemoteDelegate(bool checkAuthOnly);
        public delegate Task<Authorized<IDemoObject>> SaveDelegate(IDemoObject target, bool checkAuthOnly);
        // Delegate Properties to provide Local or Remote fork in execution
        public IAuthorization IAuthorization { get; }
        public CreateRemoteDelegate CreateRemoteProperty { get; }
        public SaveDelegate SaveProperty { get; }

        public DemoObjectFactory(IServiceProvider serviceProvider, IAuthorization iauthorization)
        {
            this.ServiceProvider = serviceProvider;
            this.IAuthorization = iauthorization;
            CreateRemoteProperty = LocalCreateRemote;
            SaveProperty = LocalSave;
        }

        public DemoObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate, IAuthorization iauthorization)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            this.IAuthorization = iauthorization;
            CreateRemoteProperty = RemoteCreateRemote;
            SaveProperty = RemoteSave;
        }

        public virtual IDemoObject Create()
        {
            return (LocalCreate(false)).Result;
        }

        public virtual Authorized<IDemoObject> TryCreate()
        {
            return LocalCreate(false);
        }

        public virtual Authorized CanCreate()
        {
            return LocalCreate(true);
        }

        public Authorized<IDemoObject> LocalCreate(bool checkAuthOnly)
        {
            Authorized anyaccess = IAuthorization.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IDemoObject>(anyaccess);
            }

            if (checkAuthOnly)
            {
                return new Authorized<IDemoObject>(true);
            }

            var target = ServiceProvider.GetRequiredService<DemoObject>();
            return new Authorized<IDemoObject>(DoMapperMethodCall<IDemoObject>(target, DataMapperMethod.Create, () => target.Create()));
        }

        public virtual IDemoObject? CreateCanReturnNull()
        {
            return (LocalCreateCanReturnNull(false)).Result;
        }

        public virtual Authorized<IDemoObject> TryCreateCanReturnNull()
        {
            return LocalCreateCanReturnNull(false);
        }

        public virtual Authorized CanCreateCanReturnNull()
        {
            return LocalCreateCanReturnNull(true);
        }

        public Authorized<IDemoObject> LocalCreateCanReturnNull(bool checkAuthOnly)
        {
            Authorized anyaccess = IAuthorization.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IDemoObject>(anyaccess);
            }

            if (checkAuthOnly)
            {
                return new Authorized<IDemoObject>(true);
            }

            var target = ServiceProvider.GetRequiredService<DemoObject>();
            return new Authorized<IDemoObject>(DoMapperMethodCallBool<IDemoObject>(target, DataMapperMethod.Create, () => target.CreateCanReturnNull()));
        }

        public virtual async Task<IDemoObject> CreateAsync()
        {
            return (await LocalCreateAsync(false)).Result;
        }

        public virtual async Task<Authorized<IDemoObject>> TryCreateAsync()
        {
            return await LocalCreateAsync(false);
        }

        public virtual async Task<Authorized> CanCreateAsync()
        {
            return await LocalCreateAsync(true);
        }

        public async Task<Authorized<IDemoObject>> LocalCreateAsync(bool checkAuthOnly)
        {
            Authorized anyaccess = IAuthorization.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IDemoObject>(anyaccess);
            }

            if (checkAuthOnly)
            {
                return new Authorized<IDemoObject>(true);
            }

            var target = ServiceProvider.GetRequiredService<DemoObject>();
            var disposableDependency = ServiceProvider.GetService<IDisposableDependency>();
            return new Authorized<IDemoObject>(await DoMapperMethodCallAsync<IDemoObject>(target, DataMapperMethod.Create, () => target.CreateAsync(disposableDependency)));
        }

        public virtual async Task<IDemoObject?> CreateRemote()
        {
            return (await CreateRemoteProperty(false)).Result;
        }

        public virtual async Task<Authorized<IDemoObject>> TryCreateRemote()
        {
            return await CreateRemoteProperty(false);
        }

        public virtual async Task<Authorized> CanCreateRemote()
        {
            return await CreateRemoteProperty(true);
        }

        public virtual async Task<Authorized<IDemoObject>> RemoteCreateRemote(bool checkAuthOnly)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IDemoObject>>(typeof(CreateRemoteDelegate), [checkAuthOnly]);
        }

        public async Task<Authorized<IDemoObject>> LocalCreateRemote(bool checkAuthOnly)
        {
            Authorized anyaccess = IAuthorization.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IDemoObject>(anyaccess);
            }

            if (checkAuthOnly)
            {
                return new Authorized<IDemoObject>(true);
            }

            var target = ServiceProvider.GetRequiredService<DemoObject>();
            return new Authorized<IDemoObject>(DoMapperMethodCallBool<IDemoObject>(target, DataMapperMethod.Create, () => target.CreateRemote()));
        }

        public virtual IDemoObject? LocalInsert(IDemoObject target)
        {
            var cTarget = (DemoObject)target ?? throw new Exception("DemoObject must implement IDemoObject");
            var disposableDependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IDemoObject>(cTarget, DataMapperMethod.Insert, () => cTarget.Insert(disposableDependency));
        }

        public virtual IDemoObject? LocalUpdate(IDemoObject target)
        {
            var cTarget = (DemoObject)target ?? throw new Exception("DemoObject must implement IDemoObject");
            var disposableDependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IDemoObject>(cTarget, DataMapperMethod.Update, () => cTarget.Update(disposableDependency));
        }

        public virtual IDemoObject? LocalDelete(IDemoObject target)
        {
            var cTarget = (DemoObject)target ?? throw new Exception("DemoObject must implement IDemoObject");
            var disposableDependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IDemoObject>(cTarget, DataMapperMethod.Delete, () => cTarget.Delete(disposableDependency));
        }

        public virtual Task<IDemoObject?> Save(IDemoObject target)
        {
            var authorized = (await SaveProperty(target, false));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Task<Authorized<IDemoObject>> TrySave(IDemoObject target)
        {
            return await SaveProperty(target, false);
        }

        public virtual Task<Authorized> CanSave(IDemoObject target)
        {
            return await SaveProperty(target, true);
        }

        public virtual async Task<Authorized<IDemoObject>> RemoteSave(IDemoObject target, bool checkAuthOnly)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IDemoObject>>(typeof(SaveDelegate), [target, checkAuthOnly]);
        }

        public virtual Task<Authorized<IDemoObject>> LocalSave(IDemoObject target, bool checkAuthOnly)
        {
            Authorized anyaccess = IAuthorization.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IDemoObject>(anyaccess);
            }

            if (checkAuthOnly)
            {
                return new Authorized<IDemoObject>(true);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return Task.FromResult(new Authorized<IDemoObject>(LocalDelete(target)));
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IDemoObject>(LocalInsert(target)));
            }
            else
            {
                return Task.FromResult(new Authorized<IDemoObject>(LocalUpdate(target)));
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<DemoObject>();
            services.AddScoped<DemoObjectFactory>();
            services.AddScoped<IDemoObjectFactory, DemoObjectFactory>();
            services.AddTransient<IDemoObject, DemoObject>();
            services.AddScoped<CreateRemoteDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<DemoObjectFactory>();
                return (bool checkAuthOnly) => factory.LocalCreateRemote(checkAuthOnly);
            });
            services.AddScoped<SaveDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<DemoObjectFactory>();
                return (IDemoObject target, bool checkAuthOnly) => factory.LocalSave(target, checkAuthOnly);
            });
            services.AddScoped<IFactoryEditBase<DemoObject>, DemoObjectFactory>();
        }
    }
}