using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using static Neatoo.UnitTest.Portal.AuthorizationConcreteClassTests;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neatoo.AuthorizationRules;
using Neatoo.Portal.Internal;
using Neatoo.UnitTest.ObjectPortal;
using Neatoo.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Neatoo.UnitTest.Portal.AuthorizationClassTests;

/*
Debugging Messages:
Parent class: AuthorizationConcreteClassTests
For AuthorizationConcreteClass using AuthorizationConcreteClass
: EditBase<AuthorizedConcreteClass>
*/
namespace Neatoo.UnitTest.Portal
{
    public interface IAuthorizedConcreteClassFactory
    {
        Authorized<AuthorizedConcreteClass> TryCreate();
        AuthorizedConcreteClass? Create();
        Task<Authorized<AuthorizedConcreteClass>> TryCreateString(string name);
        Task<AuthorizedConcreteClass?> CreateString(string name);
        AuthorizedConcreteClass? Save(AuthorizedConcreteClass target);
        Authorized<AuthorizedConcreteClass> TrySave(AuthorizedConcreteClass target);
        Task<AuthorizedConcreteClass?> Save(AuthorizedConcreteClass target, string name);
        Task<Authorized<AuthorizedConcreteClass>> TrySave(AuthorizedConcreteClass target, string name);
    }

    internal class AuthorizedConcreteClassFactory : FactoryEditBase<AuthorizedConcreteClass>, IFactoryEditBase<AuthorizedConcreteClass>, IAuthorizedConcreteClassFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public AuthorizationConcreteClass AuthorizationConcreteClass { get; }
        public CreateStringDelegate CreateStringProperty { get; }
        public Save1Delegate Save1Property { get; set; }

        public delegate Task<Authorized<AuthorizedConcreteClass>> CreateStringDelegate(string name);
        public delegate Task<Authorized<AuthorizedConcreteClass>> Save1Delegate(AuthorizedConcreteClass target, string name);
        public AuthorizedConcreteClassFactory(IServiceProvider serviceProvider, AuthorizationConcreteClass authorizationconcreteclass)
        {
            this.ServiceProvider = serviceProvider;
            this.AuthorizationConcreteClass = authorizationconcreteclass;
            CreateStringProperty = LocalCreateString;
            Save1Property = LocalSave1;
        }

        public AuthorizedConcreteClassFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate, AuthorizationConcreteClass authorizationconcreteclass)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            this.AuthorizationConcreteClass = authorizationconcreteclass;
            CreateStringProperty = RemoteCreateString;
            Save1Property = RemoteSave1;
        }

        public AuthorizedConcreteClass? Create()
        {
            var authorized = (TryCreate());
            return authorized.Result;
        }

        public virtual Task<Authorized<AuthorizedConcreteClass>> TryCreateString(string name)
        {
            return CreateStringProperty(name);
        }

        public async Task<AuthorizedConcreteClass?> CreateString(string name)
        {
            var authorized = (await TryCreateString(name));
            return authorized.Result;
        }

        public Authorized<AuthorizedConcreteClass> TryCreate()
        {
            Authorized anyaccess = AuthorizationConcreteClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<AuthorizedConcreteClass>(anyaccess);
            }

            Authorized canread = AuthorizationConcreteClass.CanRead();
            if (!canread.HasAccess)
            {
                return new Authorized<AuthorizedConcreteClass>(canread);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedConcreteClass>();
            return new Authorized<AuthorizedConcreteClass>(DoMapperMethodCall<AuthorizedConcreteClass>(target, DataMapperMethod.Create, () => target.Create()));
        }

        public Task<Authorized<AuthorizedConcreteClass>> LocalCreateString(string name)
        {
            Authorized anyaccess = AuthorizationConcreteClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return Task.FromResult(new Authorized<AuthorizedConcreteClass>(anyaccess));
            }

            Authorized canread = AuthorizationConcreteClass.CanRead();
            if (!canread.HasAccess)
            {
                return Task.FromResult(new Authorized<AuthorizedConcreteClass>(canread));
            }

            Authorized canreadstringremote = AuthorizationConcreteClass.CanReadStringRemote(name);
            if (!canreadstringremote.HasAccess)
            {
                return Task.FromResult(new Authorized<AuthorizedConcreteClass>(canreadstringremote));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedConcreteClass>();
            return Task.FromResult(new Authorized<AuthorizedConcreteClass>(DoMapperMethodCall<AuthorizedConcreteClass>(target, DataMapperMethod.Create, () => target.CreateString(name))));
        }

        public virtual AuthorizedConcreteClass? LocalInsert(AuthorizedConcreteClass itarget)
        {
            var target = (AuthorizedConcreteClass)itarget ?? throw new Exception("AuthorizedConcreteClass must implement AuthorizedConcreteClass");
            return DoMapperMethodCall<AuthorizedConcreteClass>(target, DataMapperMethod.Insert, () => target.Insert());
        }

        public virtual AuthorizedConcreteClass? LocalInsertString(AuthorizedConcreteClass itarget, string name)
        {
            var target = (AuthorizedConcreteClass)itarget ?? throw new Exception("AuthorizedConcreteClass must implement AuthorizedConcreteClass");
            return DoMapperMethodCall<AuthorizedConcreteClass>(target, DataMapperMethod.Insert, () => target.InsertString(name));
        }

        public virtual async Task<Authorized<AuthorizedConcreteClass>> RemoteCreateString(string name)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<AuthorizedConcreteClass>>(typeof(CreateStringDelegate), [name]);
        }

        public AuthorizedConcreteClass? Save(AuthorizedConcreteClass target)
        {
            var authorized = (TrySave(target));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        async Task<IEditBase?> IFactoryEditBase<AuthorizedConcreteClass>.Save(AuthorizedConcreteClass target)
        {
            return await Task.FromResult((IEditBase? )Save(target));
        }

        public virtual Authorized<AuthorizedConcreteClass> TrySave(AuthorizedConcreteClass target)
        {
            Authorized anyaccess = AuthorizationConcreteClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<AuthorizedConcreteClass>(anyaccess);
            }

            Authorized canwrite = AuthorizationConcreteClass.CanWrite();
            if (!canwrite.HasAccess)
            {
                return new Authorized<AuthorizedConcreteClass>(canwrite);
            }

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
                return new Authorized<AuthorizedConcreteClass>(LocalInsert(target));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<AuthorizedConcreteClass?> Save(AuthorizedConcreteClass target, string name)
        {
            var authorized = (await TrySave(target, name));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<AuthorizedConcreteClass>> TrySave(AuthorizedConcreteClass target, string name)
        {
            return Save1Property(target, name);
        }

        public async Task<Authorized<AuthorizedConcreteClass>> RemoteSave1(AuthorizedConcreteClass target, string name)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<AuthorizedConcreteClass>?>(typeof(Save1Delegate), [target, name]);
        }

        public virtual Task<Authorized<AuthorizedConcreteClass>> LocalSave1(AuthorizedConcreteClass target, string name)
        {
            Authorized anyaccess = AuthorizationConcreteClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return Task.FromResult(new Authorized<AuthorizedConcreteClass>(anyaccess));
            }

            Authorized canwrite = AuthorizationConcreteClass.CanWrite();
            if (!canwrite.HasAccess)
            {
                return Task.FromResult(new Authorized<AuthorizedConcreteClass>(canwrite));
            }

            Authorized canwritestringremote = AuthorizationConcreteClass.CanWriteStringRemote(name);
            if (!canwritestringremote.HasAccess)
            {
                return Task.FromResult(new Authorized<AuthorizedConcreteClass>(canwritestringremote));
            }

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
                return Task.FromResult(new Authorized<AuthorizedConcreteClass>(LocalInsertString(target, name)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<AuthorizedConcreteClass>();
            services.AddScoped<AuthorizedConcreteClassFactory>();
            services.AddScoped<IAuthorizedConcreteClassFactory, AuthorizedConcreteClassFactory>();
            services.AddScoped<CreateStringDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedConcreteClassFactory>();
                return (string name) => factory.LocalCreateString(name);
            });
            services.AddScoped<Save1Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedConcreteClassFactory>();
                return (target, name) => factory.LocalSave1(target, name);
            });
            services.AddScoped<IFactoryEditBase<AuthorizedConcreteClass>, AuthorizedConcreteClassFactory>();
        }
    }
}