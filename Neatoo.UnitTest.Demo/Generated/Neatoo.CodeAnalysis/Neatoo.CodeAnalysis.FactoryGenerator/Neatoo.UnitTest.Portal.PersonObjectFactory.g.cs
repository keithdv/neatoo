using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using static Neatoo.UnitTest.Portal.PersonObjectTests;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Neatoo.AuthorizationRules;
using Neatoo.Internal;
using Neatoo.UnitTest.Demo;

/*
                    Debugging Messages:
                    Parent class: PersonObjectTests
: IPersonObject
No MethodDeclarationSyntax for GetType
No MethodDeclarationSyntax for MemberwiseClone
                    */
namespace Neatoo.UnitTest.Portal
{
    public interface IPersonObjectFactory
    {
        Task<Authorized> CanInsertA(OuterClass.InnerClass innerClass);
        Task<Authorized> CanUpdateA();
        Task<Authorized> CanDeleteA();
        Task<IPersonObject?> SaveA(IPersonObject target, OuterClass.InnerClass innerClass);
        Task<Authorized<IPersonObject>> TrySaveA(IPersonObject target, OuterClass.InnerClass innerClass);
        Task<IPersonObject?> SaveA(IPersonObject target);
        Task<Authorized<IPersonObject>> TrySaveA(IPersonObject target);
    }

    internal class PersonObjectFactory : FactoryEditBase<PersonObject>, IFactoryEditBase<PersonObject>, IPersonObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        public delegate Task<Authorized> CanInsertADelegate(OuterClass.InnerClass innerClass);
        public delegate Task<Authorized> CanUpdateADelegate();
        public delegate Task<Authorized> CanDeleteADelegate();
        public delegate Task<Authorized<IPersonObject>> SaveA1Delegate(IPersonObject target, OuterClass.InnerClass innerClass);
        public delegate Task<Authorized<IPersonObject>> SaveADelegate(IPersonObject target);
        // Delegate Properties to provide Local or Remote fork in execution
        public IAuthorizePersonObject IAuthorizePersonObject { get; }
        public CanInsertADelegate CanInsertAProperty { get; }
        public CanUpdateADelegate CanUpdateAProperty { get; }
        public CanDeleteADelegate CanDeleteAProperty { get; }
        public SaveA1Delegate SaveA1Property { get; }
        public SaveADelegate SaveAProperty { get; }

        public PersonObjectFactory(IServiceProvider serviceProvider, IAuthorizePersonObject iauthorizepersonobject)
        {
            this.ServiceProvider = serviceProvider;
            this.IAuthorizePersonObject = iauthorizepersonobject;
            CanInsertAProperty = LocalCanInsertA;
            CanUpdateAProperty = LocalCanUpdateA;
            CanDeleteAProperty = LocalCanDeleteA;
            SaveA1Property = LocalSaveA1;
            SaveAProperty = LocalSaveA;
        }

        public PersonObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate, IAuthorizePersonObject iauthorizepersonobject)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            this.IAuthorizePersonObject = iauthorizepersonobject;
            CanInsertAProperty = RemoteCanInsertA;
            CanUpdateAProperty = RemoteCanUpdateA;
            CanDeleteAProperty = RemoteCanDeleteA;
            SaveA1Property = RemoteSaveA1;
            SaveAProperty = RemoteSaveA;
        }

        public async Task<Authorized<IPersonObject>> LocalInsertA(IPersonObject target, OuterClass.InnerClass innerClass)
        {
            Authorized authorized;
            authorized = await IAuthorizePersonObject.CanWriteTarget();
            if (!authorized.HasAccess)
            {
                return new Authorized<IPersonObject>(authorized);
            }

            authorized = await IAuthorizePersonObject.CanWrite();
            if (!authorized.HasAccess)
            {
                return new Authorized<IPersonObject>(authorized);
            }

            authorized = IAuthorizePersonObject.CanInsertRemote();
            if (!authorized.HasAccess)
            {
                return new Authorized<IPersonObject>(authorized);
            }

            var cTarget = (PersonObject)target ?? throw new Exception("IPersonObject must implement PersonObject");
            var disposable = ServiceProvider.GetService<IDisposable>();
            return new Authorized<IPersonObject>(DoMapperMethodCall<IPersonObject>(cTarget, DataMapperMethod.Insert, () => cTarget.InsertA(innerClass, disposable)));
        }

        public async Task<Authorized<IPersonObject>> LocalUpdateA(IPersonObject target)
        {
            Authorized authorized;
            authorized = await IAuthorizePersonObject.CanWriteTarget();
            if (!authorized.HasAccess)
            {
                return new Authorized<IPersonObject>(authorized);
            }

            authorized = IAuthorizePersonObject.CanInsertRemote();
            if (!authorized.HasAccess)
            {
                return new Authorized<IPersonObject>(authorized);
            }

            var cTarget = (PersonObject)target ?? throw new Exception("IPersonObject must implement PersonObject");
            return new Authorized<IPersonObject>(DoMapperMethodCall<IPersonObject>(cTarget, DataMapperMethod.Update, () => cTarget.UpdateA()));
        }

        public async Task<Authorized<IPersonObject>> LocalDeleteA(IPersonObject target)
        {
            Authorized authorized;
            authorized = await IAuthorizePersonObject.CanWriteTarget();
            if (!authorized.HasAccess)
            {
                return new Authorized<IPersonObject>(authorized);
            }

            authorized = IAuthorizePersonObject.CanInsertRemote();
            if (!authorized.HasAccess)
            {
                return new Authorized<IPersonObject>(authorized);
            }

            var cTarget = (PersonObject)target ?? throw new Exception("IPersonObject must implement PersonObject");
            return new Authorized<IPersonObject>(DoMapperMethodCall<IPersonObject>(cTarget, DataMapperMethod.Delete, () => cTarget.DeleteA()));
        }

        public virtual Task<Authorized> CanInsertA(OuterClass.InnerClass innerClass)
        {
            return CanInsertAProperty(innerClass);
        }

        public virtual async Task<Authorized> RemoteCanInsertA(OuterClass.InnerClass innerClass)
        {
            return await DoRemoteRequest.ForDelegate<Authorized>(typeof(CanInsertADelegate), [innerClass]);
        }

        public async Task<Authorized> LocalCanInsertA(OuterClass.InnerClass innerClass)
        {
            Authorized authorized;
            authorized = await IAuthorizePersonObject.CanWriteTarget();
            if (!authorized.HasAccess)
            {
                return authorized;
            }

            authorized = await IAuthorizePersonObject.CanWrite();
            if (!authorized.HasAccess)
            {
                return authorized;
            }

            authorized = IAuthorizePersonObject.CanInsertRemote();
            if (!authorized.HasAccess)
            {
                return authorized;
            }

            return new Authorized(true);
        }

        public virtual Task<Authorized> CanUpdateA()
        {
            return CanUpdateAProperty();
        }

        public virtual async Task<Authorized> RemoteCanUpdateA()
        {
            return await DoRemoteRequest.ForDelegate<Authorized>(typeof(CanUpdateADelegate), []);
        }

        public async Task<Authorized> LocalCanUpdateA()
        {
            Authorized authorized;
            authorized = await IAuthorizePersonObject.CanWriteTarget();
            if (!authorized.HasAccess)
            {
                return authorized;
            }

            authorized = IAuthorizePersonObject.CanInsertRemote();
            if (!authorized.HasAccess)
            {
                return authorized;
            }

            return new Authorized(true);
        }

        public virtual Task<Authorized> CanDeleteA()
        {
            return CanDeleteAProperty();
        }

        public virtual async Task<Authorized> RemoteCanDeleteA()
        {
            return await DoRemoteRequest.ForDelegate<Authorized>(typeof(CanDeleteADelegate), []);
        }

        public async Task<Authorized> LocalCanDeleteA()
        {
            Authorized authorized;
            authorized = await IAuthorizePersonObject.CanWriteTarget();
            if (!authorized.HasAccess)
            {
                return authorized;
            }

            authorized = IAuthorizePersonObject.CanInsertRemote();
            if (!authorized.HasAccess)
            {
                return authorized;
            }

            return new Authorized(true);
        }

        public virtual async Task<IPersonObject?> SaveA(IPersonObject target, OuterClass.InnerClass innerClass)
        {
            var authorized = (await SaveA1Property(target, innerClass));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Task<Authorized<IPersonObject>> TrySaveA(IPersonObject target, OuterClass.InnerClass innerClass)
        {
            return SaveA1Property(target, innerClass);
        }

        public virtual async Task<Authorized<IPersonObject>> RemoteSaveA1(IPersonObject target, OuterClass.InnerClass innerClass)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IPersonObject>>(typeof(SaveA1Delegate), [target, innerClass]);
        }

        public virtual Task<Authorized<IPersonObject>> LocalSaveA1(IPersonObject target, OuterClass.InnerClass innerClass)
        {
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
                return LocalInsertA(target, innerClass);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public virtual async Task<IPersonObject?> SaveA(IPersonObject target)
        {
            var authorized = (await SaveAProperty(target));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Task<Authorized<IPersonObject>> TrySaveA(IPersonObject target)
        {
            return SaveAProperty(target);
        }

        public virtual async Task<Authorized<IPersonObject>> RemoteSaveA(IPersonObject target)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IPersonObject>>(typeof(SaveADelegate), [target]);
        }

        async Task<IEditBase?> IFactoryEditBase<PersonObject>.Save(PersonObject target)
        {
            return (IEditBase? )await SaveA(target);
        }

        public virtual Task<Authorized<IPersonObject>> LocalSaveA(IPersonObject target)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                return LocalDeleteA(target);
            }
            else if (target.IsNew)
            {
                throw new NotImplementedException();
            }
            else
            {
                return LocalUpdateA(target);
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<PersonObject>();
            services.AddScoped<PersonObjectFactory>();
            services.AddScoped<IPersonObjectFactory, PersonObjectFactory>();
            services.AddTransient<IPersonObject, PersonObject>();
            services.AddScoped<CanInsertADelegate>(cc =>
            {
                var factory = cc.GetRequiredService<PersonObjectFactory>();
                return (OuterClass.InnerClass innerClass) => factory.LocalCanInsertA(innerClass);
            });
            services.AddScoped<CanUpdateADelegate>(cc =>
            {
                var factory = cc.GetRequiredService<PersonObjectFactory>();
                return () => factory.LocalCanUpdateA();
            });
            services.AddScoped<CanDeleteADelegate>(cc =>
            {
                var factory = cc.GetRequiredService<PersonObjectFactory>();
                return () => factory.LocalCanDeleteA();
            });
            services.AddScoped<SaveA1Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<PersonObjectFactory>();
                return (IPersonObject target, OuterClass.InnerClass innerClass) => factory.LocalSaveA1(target, innerClass);
            });
            services.AddScoped<SaveADelegate>(cc =>
            {
                var factory = cc.GetRequiredService<PersonObjectFactory>();
                return (IPersonObject target) => factory.LocalSaveA(target);
            });
            services.AddScoped<IFactoryEditBase<PersonObject>, PersonObjectFactory>();
        }
    }
}