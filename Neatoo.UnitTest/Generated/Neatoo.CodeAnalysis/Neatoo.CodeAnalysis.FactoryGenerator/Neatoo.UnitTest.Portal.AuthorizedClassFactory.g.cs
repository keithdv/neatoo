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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

/*
Debugging Messages:
Parent class: AuthorizationClassTests
For IAuthorization using IAuthorization
: IAuthorizedClass
*/
namespace Neatoo.UnitTest.Portal
{
    public interface IAuthorizedClassFactory
    {
        IAuthorizedClass Create(int p);
        Task<IAuthorizedClass?> Save(IAuthorizedClass target, int p, string onno);
        Task<Authorized<IAuthorizedClass>> TrySave(IAuthorizedClass target, int p, string onno);
        Task<Authorized> CanSave(IAuthorizedClass target, int p, string onno);
        IAuthorizedClass? Save(IAuthorizedClass target, int p);
        Authorized<IAuthorizedClass> TrySave(IAuthorizedClass target, int p);
        Authorized CanSave(IAuthorizedClass target, int p);
    }

    internal class AuthorizedClassFactory : FactoryEditBase<AuthorizedClass>, IFactoryEditBase<AuthorizedClass>, IAuthorizedClassFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        public delegate Task<Authorized<IAuthorizedClass>> SaveDelegate(IAuthorizedClass target, int p, string onno, bool checkAuthOnly);
        // Delegate Properties to provide Local or Remote fork in execution
        public IAuthorization IAuthorization { get; }
        public SaveDelegate SaveProperty { get; }

        public AuthorizedClassFactory(IServiceProvider serviceProvider, IAuthorization iauthorization)
        {
            this.ServiceProvider = serviceProvider;
            this.IAuthorization = iauthorization;
            SaveProperty = LocalSave;
        }

        public AuthorizedClassFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate, IAuthorization iauthorization)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            this.IAuthorization = iauthorization;
            SaveProperty = RemoteSave;
        }

        public virtual IAuthorizedClass Create(int p)
        {
            return LocalCreate(p);
        }

        public IAuthorizedClass LocalCreate(int p)
        {
            var target = ServiceProvider.GetRequiredService<AuthorizedClass>();
            return DoMapperMethodCall<IAuthorizedClass>(target, DataMapperMethod.Create, () => target.Create(p));
        }

        public virtual IAuthorizedClass? LocalInsert(IAuthorizedClass target, int p, string onno)
        {
            var cTarget = (AuthorizedClass)target ?? throw new Exception("AuthorizedClass must implement IAuthorizedClass");
            var disposableDependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IAuthorizedClass>(cTarget, DataMapperMethod.Insert, () => cTarget.Insert(p, onno, disposableDependency));
        }

        public virtual Task<IAuthorizedClass?> LocalUpdate(IAuthorizedClass target, int p, string onno)
        {
            var cTarget = (AuthorizedClass)target ?? throw new Exception("AuthorizedClass must implement IAuthorizedClass");
            var disposableDependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCallBoolAsync<IAuthorizedClass>(cTarget, DataMapperMethod.Update, () => cTarget.Update(p, onno, disposableDependency));
        }

        public virtual IAuthorizedClass? LocalDelete(IAuthorizedClass target, int p)
        {
            var cTarget = (AuthorizedClass)target ?? throw new Exception("AuthorizedClass must implement IAuthorizedClass");
            var disposableDependency = ServiceProvider.GetService<IDisposableDependency>();
            return DoMapperMethodCall<IAuthorizedClass>(cTarget, DataMapperMethod.Delete, () => cTarget.Delete(p, disposableDependency));
        }

        public virtual async Task<IAuthorizedClass?> Save(IAuthorizedClass target, int p, string onno)
        {
            var authorized = (await SaveProperty(target, p, onno, false));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedClass>> TrySave(IAuthorizedClass target, int p, string onno)
        {
            return await SaveProperty(target, p, onno, false);
        }

        public virtual async Task<Authorized> CanSave(IAuthorizedClass target, int p, string onno)
        {
            return await SaveProperty(target, p, onno, true);
        }

        public virtual async Task<Authorized<IAuthorizedClass>> RemoteSave(IAuthorizedClass target, int p, string onno, bool checkAuthOnly)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedClass>>(typeof(SaveDelegate), [target, p, onno, checkAuthOnly]);
        }

        public virtual async Task<Authorized<IAuthorizedClass>> LocalSave(IAuthorizedClass target, int p, string onno, bool checkAuthOnly)
        {
            Authorized anyaccess = IAuthorization.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(anyaccess);
            }

            Authorized anyaccess = IAuthorization.AnyAccess(p);
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(anyaccess);
            }

            if (checkAuthOnly)
            {
                return new Authorized<IAuthorizedClass>(true);
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
                return new Authorized<IAuthorizedClass>(LocalInsert(target, p, onno));
            }
            else
            {
                return new Authorized<IAuthorizedClass>(await LocalUpdate(target, p, onno));
            }
        }

        public virtual IAuthorizedClass? Save(IAuthorizedClass target, int p)
        {
            var authorized = (LocalSave1(target, p, false));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedClass> TrySave(IAuthorizedClass target, int p)
        {
            return LocalSave1(target, p, false);
        }

        public virtual Authorized CanSave(IAuthorizedClass target, int p)
        {
            return LocalSave1(target, p, true);
        }

        public virtual Authorized<IAuthorizedClass> LocalSave1(IAuthorizedClass target, int p, bool checkAuthOnly)
        {
            Authorized anyaccess = IAuthorization.AnyAccess();
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(anyaccess);
            }

            Authorized anyaccess = IAuthorization.AnyAccess(p);
            if (!anyaccess.HasAccess)
            {
                return new Authorized<IAuthorizedClass>(anyaccess);
            }

            if (checkAuthOnly)
            {
                return new Authorized<IAuthorizedClass>(true);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return new Authorized<IAuthorizedClass>(LocalDelete(target, p));
            }
            else if (target.IsNew)
            {
                throw new NotImplementedException();
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
            services.AddScoped<SaveDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedClassFactory>();
                return (IAuthorizedClass target, int p, string onno, bool checkAuthOnly) => factory.LocalSave(target, p, onno, checkAuthOnly);
            });
            services.AddScoped<IFactoryEditBase<AuthorizedClass>, AuthorizedClassFactory>();
        }
    }
}