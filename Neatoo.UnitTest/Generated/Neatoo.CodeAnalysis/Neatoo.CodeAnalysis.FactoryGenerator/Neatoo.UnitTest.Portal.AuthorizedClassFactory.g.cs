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
using Neatoo.Portal.Internal;
using Neatoo.UnitTest.ObjectPortal;
using Neatoo.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
Debugging Messages:
Parent class: AuthorizationClassTests
For IAuthorizationClass using IAuthorizationClass
: EditBase<AuthorizedClass>, IAuthorizedClass
*/
namespace Neatoo.UnitTest.Portal
{
    public interface IAuthorizedClassFactory
    {
        Authorized<IAuthorizedClass> TryCreate();
        IAuthorizedClass? Create();
        Task<Authorized<IAuthorizedClass>> TryCreateInt(int param);
        Task<IAuthorizedClass?> CreateInt(int param);
        Task<Authorized<IAuthorizedClass>> TryCreateString(string name);
        Task<IAuthorizedClass?> CreateString(string name);
        IAuthorizedClass? Save(IAuthorizedClass target);
        Authorized<IAuthorizedClass> TrySave(IAuthorizedClass target);
        IAuthorizedClass? Save(IAuthorizedClass target, int name);
        Authorized<IAuthorizedClass> TrySave(IAuthorizedClass target, int name);
        Task<IAuthorizedClass?> Save(IAuthorizedClass target, string name);
        Task<Authorized<IAuthorizedClass>> TrySave(IAuthorizedClass target, string name);
    }

    internal class AuthorizedClassFactory : FactoryEditBase<AuthorizedClass>, IFactoryEditBase<AuthorizedClass>, IAuthorizedClassFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IAuthorizationClass IAuthorizationClass { get; }
        public CreateStringDelegate CreateStringProperty { get; }
        public Save2Delegate Save2Property { get; set; }

        public delegate Task<Authorized<IAuthorizedClass>> CreateStringDelegate(string name);
        public delegate Task<Authorized<IAuthorizedClass>> Save2Delegate(IAuthorizedClass target, string name);
        public AuthorizedClassFactory(IServiceProvider serviceProvider, IAuthorizationClass iauthorizationclass)
        {
            this.ServiceProvider = serviceProvider;
            this.IAuthorizationClass = iauthorizationclass;
            CreateStringProperty = LocalCreateString;
            Save2Property = LocalSave2;
        }

        public AuthorizedClassFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate, IAuthorizationClass iauthorizationclass)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            this.IAuthorizationClass = iauthorizationclass;
            CreateStringProperty = RemoteCreateString;
            Save2Property = RemoteSave2;
        }

        public IAuthorizedClass? Create()
        {
            var authorized = (TryCreate());
            return authorized.Result;
        }

        public async Task<IAuthorizedClass?> CreateInt(int param)
        {
            var authorized = (await TryCreateInt(param));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedClass>> TryCreateString(string name)
        {
            return CreateStringProperty(name);
        }

        public async Task<IAuthorizedClass?> CreateString(string name)
        {
            var authorized = (await TryCreateString(name));
            return authorized.Result;
        }

        public Authorized<IAuthorizedClass> TryCreate()
        {
            Authorized anyaccess = IAuthorizationClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(anyaccess);
            }

            Authorized canread = IAuthorizationClass.CanRead();
            if (!canread.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(canread);
            }

            Authorized canreadint2 = IAuthorizationClass.CanReadInt2();
            if (!canreadint2.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(canreadint2);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedClass>();
            return new Authorized<IAuthorizedClass>(DoMapperMethodCall<IAuthorizedClass>(target, DataMapperMethod.Create, () => target.Create()));
        }

        public async Task<Authorized<IAuthorizedClass>> TryCreateInt(int param)
        {
            Authorized anyaccess = IAuthorizationClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(anyaccess);
            }

            Authorized canread = IAuthorizationClass.CanRead();
            if (!canread.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(canread);
            }

            Authorized canreadint = IAuthorizationClass.CanReadInt(param);
            if (!canreadint.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(canreadint);
            }

            Authorized canreadint2 = IAuthorizationClass.CanReadInt2();
            if (!canreadint2.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(canreadint2);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedClass>();
            return new Authorized<IAuthorizedClass>(await DoMapperMethodCallAsync<IAuthorizedClass>(target, DataMapperMethod.Create, () => target.CreateInt(param)));
        }

        public Task<Authorized<IAuthorizedClass>> LocalCreateString(string name)
        {
            Authorized anyaccess = IAuthorizationClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedClass>(anyaccess));
            }

            Authorized canread = IAuthorizationClass.CanRead();
            if (!canread.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedClass>(canread));
            }

            Authorized canreadint2 = IAuthorizationClass.CanReadInt2();
            if (!canreadint2.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedClass>(canreadint2));
            }

            Authorized canreadstringremote = IAuthorizationClass.CanReadStringRemote(name);
            if (!canreadstringremote.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedClass>(canreadstringremote));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedClass>();
            return Task.FromResult(new Authorized<IAuthorizedClass>(DoMapperMethodCall<IAuthorizedClass>(target, DataMapperMethod.Create, () => target.CreateString(name))));
        }

        public virtual IAuthorizedClass? LocalInsert(IAuthorizedClass itarget)
        {
            var target = (AuthorizedClass)itarget ?? throw new Exception("AuthorizedClass must implement IAuthorizedClass");
            return DoMapperMethodCall<IAuthorizedClass>(target, DataMapperMethod.Insert, () => target.Insert());
        }

        public virtual IAuthorizedClass? LocalInsertInt(IAuthorizedClass itarget, int name)
        {
            var target = (AuthorizedClass)itarget ?? throw new Exception("AuthorizedClass must implement IAuthorizedClass");
            return DoMapperMethodCall<IAuthorizedClass>(target, DataMapperMethod.Insert, () => target.InsertInt(name));
        }

        public virtual IAuthorizedClass? LocalInsertString(IAuthorizedClass itarget, string name)
        {
            var target = (AuthorizedClass)itarget ?? throw new Exception("AuthorizedClass must implement IAuthorizedClass");
            return DoMapperMethodCall<IAuthorizedClass>(target, DataMapperMethod.Insert, () => target.InsertString(name));
        }

        public virtual IAuthorizedClass? LocalUpdateString(IAuthorizedClass itarget, string name)
        {
            var target = (AuthorizedClass)itarget ?? throw new Exception("AuthorizedClass must implement IAuthorizedClass");
            return DoMapperMethodCall<IAuthorizedClass>(target, DataMapperMethod.Update, () => target.UpdateString(name));
        }

        public virtual async Task<Authorized<IAuthorizedClass>> RemoteCreateString(string name)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedClass>>(typeof(CreateStringDelegate), [name]);
        }

        public IAuthorizedClass? Save(IAuthorizedClass target)
        {
            var authorized = (TrySave(target));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        async Task<IEditBase?> IFactoryEditBase<AuthorizedClass>.Save(AuthorizedClass target)
        {
            return await Task.FromResult((IEditBase? )Save(target));
        }

        public virtual Authorized<IAuthorizedClass> TrySave(IAuthorizedClass target)
        {
            Authorized anyaccess = IAuthorizationClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(anyaccess);
            }

            Authorized canwrite = IAuthorizationClass.CanWrite();
            if (!canwrite.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(canwrite);
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
                return new Authorized<IAuthorizedClass>(LocalInsert(target));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IAuthorizedClass? Save(IAuthorizedClass target, int name)
        {
            var authorized = (TrySave(target, name));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedClass> TrySave(IAuthorizedClass target, int name)
        {
            Authorized anyaccess = IAuthorizationClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(anyaccess);
            }

            Authorized canwrite = IAuthorizationClass.CanWrite();
            if (!canwrite.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(canwrite);
            }

            Authorized canwriteint = IAuthorizationClass.CanWriteInt(name);
            if (!canwriteint.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(canwriteint);
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
                return new Authorized<IAuthorizedClass>(LocalInsertInt(target, name));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedClass?> Save(IAuthorizedClass target, string name)
        {
            var authorized = (await TrySave(target, name));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedClass>> TrySave(IAuthorizedClass target, string name)
        {
            return Save2Property(target, name);
        }

        public async Task<Authorized<IAuthorizedClass>> RemoteSave2(IAuthorizedClass target, string name)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedClass>?>(typeof(Save2Delegate), [target, name]);
        }

        public virtual Task<Authorized<IAuthorizedClass>> LocalSave2(IAuthorizedClass target, string name)
        {
            Authorized anyaccess = IAuthorizationClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedClass>(anyaccess));
            }

            Authorized canwrite = IAuthorizationClass.CanWrite();
            if (!canwrite.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedClass>(canwrite));
            }

            Authorized canwritestringremote = IAuthorizationClass.CanWriteStringRemote(name);
            if (!canwritestringremote.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedClass>(canwritestringremote));
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
                return Task.FromResult(new Authorized<IAuthorizedClass>(LocalInsertString(target, name)));
            }
            else
            {
                return Task.FromResult(new Authorized<IAuthorizedClass>(LocalUpdateString(target, name)));
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<AuthorizedClass>();
            services.AddScoped<AuthorizedClassFactory>();
            services.AddScoped<IAuthorizedClassFactory, AuthorizedClassFactory>();
            services.AddTransient<IAuthorizedClass, AuthorizedClass>();
            services.AddScoped<CreateStringDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedClassFactory>();
                return (string name) => factory.LocalCreateString(name);
            });
            services.AddScoped<Save2Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedClassFactory>();
                return (target, name) => factory.LocalSave2(target, name);
            });
            services.AddScoped<IFactoryEditBase<AuthorizedClass>, AuthorizedClassFactory>();
        }
    }
}