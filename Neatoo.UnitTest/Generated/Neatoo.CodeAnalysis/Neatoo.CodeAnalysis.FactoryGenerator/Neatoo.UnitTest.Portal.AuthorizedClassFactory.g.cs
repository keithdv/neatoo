using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neatoo.AuthorizationRules;
using Neatoo.Portal.Internal;
using Neatoo.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
Debugging Messages:
For IAuthorizationClass using IAuthorizationClass
: EditBase<AuthorizedClass>, IAuthorizedClass
*/
namespace Neatoo.UnitTest.Portal
{
    public interface IAuthorizedClassFactory
    {
        IAuthorizedClass Create();
        IAuthorizedClass CreateInt(int param);
        Task<IAuthorizedClass> CreateString(string name);
        IAuthorizedClass? Save(IAuthorizedClass target);
        IAuthorizedClass? Save(IAuthorizedClass target, int name);
        Task<IAuthorizedClass?> Save(IAuthorizedClass target, string name);
    }

    internal class AuthorizedClassFactory : FactoryEditBase<AuthorizedClass>, IAuthorizedClassFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IAuthorizationClass IAuthorizationClass { get; }
        public CreateStringDelegate CreateStringProperty { get; }
        public Save2Delegate Save2Property { get; set; }

        public delegate Task<IAuthorizedClass> CreateStringDelegate(string name);
        public delegate Task<IAuthorizedClass?> Save2Delegate(IAuthorizedClass target, string name);
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

        public virtual Task<IAuthorizedClass> CreateString(string name)
        {
            return CreateStringProperty(name);
        }

        public IAuthorizedClass Create()
        {
            var anyaccess = IAuthorizationClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                throw new NotAuthorizedException(anyaccess.Message);
            }

            var canread = IAuthorizationClass.CanRead();
            if (!canread.HasAccess)
            {
                throw new NotAuthorizedException(canread.Message);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedClass>();
            return DoMapperMethodCall<IAuthorizedClass>(target, DataMapperMethod.Create, () => target.Create());
        }

        public IAuthorizedClass CreateInt(int param)
        {
            var anyaccess = IAuthorizationClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                throw new NotAuthorizedException(anyaccess.Message);
            }

            var canread = IAuthorizationClass.CanRead();
            if (!canread.HasAccess)
            {
                throw new NotAuthorizedException(canread.Message);
            }

            var canreadint = IAuthorizationClass.CanReadInt(param);
            if (!canreadint.HasAccess)
            {
                throw new NotAuthorizedException(canreadint.Message);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedClass>();
            return DoMapperMethodCall<IAuthorizedClass>(target, DataMapperMethod.Create, () => target.CreateInt(param));
        }

        public Task<IAuthorizedClass> LocalCreateString(string name)
        {
            var anyaccess = IAuthorizationClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                throw new NotAuthorizedException(anyaccess.Message);
            }

            var canread = IAuthorizationClass.CanRead();
            if (!canread.HasAccess)
            {
                throw new NotAuthorizedException(canread.Message);
            }

            var canreadstringremote = IAuthorizationClass.CanReadStringRemote(name);
            if (!canreadstringremote.HasAccess)
            {
                throw new NotAuthorizedException(canreadstringremote.Message);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedClass>();
            return DoMapperMethodCallAsync<IAuthorizedClass>(target, DataMapperMethod.Create, () => target.CreateString(name));
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

        public virtual async Task<IAuthorizedClass> RemoteCreateString(string name)
        {
            return await DoRemoteRequest.ForDelegate<AuthorizedClass>(typeof(CreateStringDelegate), [name]);
        }

        // Not able to call CanWriteInt due to parameter mismatch;
        // Not able to call CanWriteStringRemote due to parameter mismatch;
        public override async Task<IEditBase?> Save(AuthorizedClass target)
        {
            return await Task.FromResult((IEditBase? )Save(target));
        }

        public virtual IAuthorizedClass? Save(IAuthorizedClass target)
        {
            var anyaccess = IAuthorizationClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                throw new NotAuthorizedException(anyaccess.Message);
            }

            var canwrite = IAuthorizationClass.CanWrite();
            if (!canwrite.HasAccess)
            {
                throw new NotAuthorizedException(canwrite.Message);
            }

            var canwritetarget = IAuthorizationClass.CanWriteTarget(target);
            if (!canwritetarget.HasAccess)
            {
                throw new NotAuthorizedException(canwritetarget.Message);
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
                return LocalInsert(target);
                ;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // Not able to call CanWriteStringRemote due to parameter mismatch;
        public virtual IAuthorizedClass? Save(IAuthorizedClass target, int name)
        {
            var anyaccess = IAuthorizationClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                throw new NotAuthorizedException(anyaccess.Message);
            }

            var canwrite = IAuthorizationClass.CanWrite();
            if (!canwrite.HasAccess)
            {
                throw new NotAuthorizedException(canwrite.Message);
            }

            var canwritetarget = IAuthorizationClass.CanWriteTarget(target);
            if (!canwritetarget.HasAccess)
            {
                throw new NotAuthorizedException(canwritetarget.Message);
            }

            var canwriteint = IAuthorizationClass.CanWriteInt(name);
            if (!canwriteint.HasAccess)
            {
                throw new NotAuthorizedException(canwriteint.Message);
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
                return LocalInsertInt(target, name);
                ;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // Not able to call CanWriteInt due to parameter mismatch;
        public Task<IAuthorizedClass?> Save(IAuthorizedClass target, string name)
        {
            return Save2Property(target, name);
        }

        public async Task<IAuthorizedClass?> RemoteSave2(IAuthorizedClass target, string name)
        {
            return await DoRemoteRequest.ForDelegate<AuthorizedClass?>(typeof(Save2Delegate), [target, name]);
        }

        public virtual Task<IAuthorizedClass?> LocalSave2(IAuthorizedClass target, string name)
        {
            var anyaccess = IAuthorizationClass.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                throw new NotAuthorizedException(anyaccess.Message);
            }

            var canwrite = IAuthorizationClass.CanWrite();
            if (!canwrite.HasAccess)
            {
                throw new NotAuthorizedException(canwrite.Message);
            }

            var canwritetarget = IAuthorizationClass.CanWriteTarget(target);
            if (!canwritetarget.HasAccess)
            {
                throw new NotAuthorizedException(canwritetarget.Message);
            }

            var canwritestringremote = IAuthorizationClass.CanWriteStringRemote(name);
            if (!canwritestringremote.HasAccess)
            {
                throw new NotAuthorizedException(canwritestringremote.Message);
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
                return Task.FromResult(LocalInsertString(target, name));
                ;
            }
            else
            {
                throw new NotImplementedException();
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