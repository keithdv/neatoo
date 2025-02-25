using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using static Neatoo.UnitTest.Portal.AuthorizationCompilesTests;
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
Parent class: AuthorizationCompilesTests
For IAuthorizationCompiles using IAuthorizationCompiles
: EditBase<AuthorizedVoidCompiles>, IAuthorizedVoidCompiles
*/
namespace Neatoo.UnitTest.Portal
{
    public interface IAuthorizedVoidCompilesFactory
    {
        Authorized<IAuthorizedVoidCompiles> TryCreate(int A);
        IAuthorizedVoidCompiles? Create(int A);
        Authorized<IAuthorizedVoidCompiles> TryCreate(double B);
        IAuthorizedVoidCompiles? Create(double B);
        Task<Authorized<IAuthorizedVoidCompiles>> TryCreate(Guid C);
        Task<IAuthorizedVoidCompiles?> Create(Guid C);
        Task<Authorized<IAuthorizedVoidCompiles>> TryCreate(long D);
        Task<IAuthorizedVoidCompiles?> Create(long D);
        IAuthorizedVoidCompiles? Save(IAuthorizedVoidCompiles target, int A);
        Authorized<IAuthorizedVoidCompiles> TrySave(IAuthorizedVoidCompiles target, int A);
        IAuthorizedVoidCompiles? Save(IAuthorizedVoidCompiles target, double B);
        Authorized<IAuthorizedVoidCompiles> TrySave(IAuthorizedVoidCompiles target, double B);
        Task<IAuthorizedVoidCompiles?> Save(IAuthorizedVoidCompiles target, Guid C);
        Task<Authorized<IAuthorizedVoidCompiles>> TrySave(IAuthorizedVoidCompiles target, Guid C);
        Task<IAuthorizedVoidCompiles?> Save(IAuthorizedVoidCompiles target, long D);
        Task<Authorized<IAuthorizedVoidCompiles>> TrySave(IAuthorizedVoidCompiles target, long D);
    }

    internal class AuthorizedVoidCompilesFactory : FactoryEditBase<AuthorizedVoidCompiles>, IFactoryEditBase<AuthorizedVoidCompiles>, IAuthorizedVoidCompilesFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public IAuthorizationCompiles IAuthorizationCompiles { get; }

        public AuthorizedVoidCompilesFactory(IServiceProvider serviceProvider, IAuthorizationCompiles iauthorizationcompiles)
        {
            this.ServiceProvider = serviceProvider;
            this.IAuthorizationCompiles = iauthorizationcompiles;
        }

        public AuthorizedVoidCompilesFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate, IAuthorizationCompiles iauthorizationcompiles)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            this.IAuthorizationCompiles = iauthorizationcompiles;
        }

        public IAuthorizedVoidCompiles? Create(int A)
        {
            var authorized = (TryCreate(A));
            return authorized.Result;
        }

        public IAuthorizedVoidCompiles? Create(double B)
        {
            var authorized = (TryCreate(B));
            return authorized.Result;
        }

        public async Task<IAuthorizedVoidCompiles?> Create(Guid C)
        {
            var authorized = (await TryCreate(C));
            return authorized.Result;
        }

        public async Task<IAuthorizedVoidCompiles?> Create(long D)
        {
            var authorized = (await TryCreate(D));
            return authorized.Result;
        }

        public Authorized<IAuthorizedVoidCompiles> TryCreate(int A)
        {
            Authorized readbool = IAuthorizationCompiles.ReadBool(A);
            if (!readbool.HasAccess)
            {
                return new Authorized<IAuthorizedVoidCompiles>(readbool);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedVoidCompiles>();
            return new Authorized<IAuthorizedVoidCompiles>(DoMapperMethodCall<IAuthorizedVoidCompiles>(target, DataMapperMethod.Create, () => target.Create(A)));
        }

        public Authorized<IAuthorizedVoidCompiles> TryCreate(double B)
        {
            Authorized readstring = IAuthorizationCompiles.ReadString(B);
            if (!readstring.HasAccess)
            {
                return new Authorized<IAuthorizedVoidCompiles>(readstring);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedVoidCompiles>();
            return new Authorized<IAuthorizedVoidCompiles>(DoMapperMethodCall<IAuthorizedVoidCompiles>(target, DataMapperMethod.Create, () => target.Create(B)));
        }

        public async Task<Authorized<IAuthorizedVoidCompiles>> TryCreate(Guid C)
        {
            Authorized readbooltask = await IAuthorizationCompiles.ReadBoolTask(C);
            if (!readbooltask.HasAccess)
            {
                return new Authorized<IAuthorizedVoidCompiles>(readbooltask);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedVoidCompiles>();
            return new Authorized<IAuthorizedVoidCompiles>(await DoMapperMethodCallAsync<IAuthorizedVoidCompiles>(target, DataMapperMethod.Create, () => target.Create(C)));
        }

        public async Task<Authorized<IAuthorizedVoidCompiles>> TryCreate(long D)
        {
            Authorized readlongtask = await IAuthorizationCompiles.ReadLongTask(D);
            if (!readlongtask.HasAccess)
            {
                return new Authorized<IAuthorizedVoidCompiles>(readlongtask);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedVoidCompiles>();
            return new Authorized<IAuthorizedVoidCompiles>(await DoMapperMethodCallAsync<IAuthorizedVoidCompiles>(target, DataMapperMethod.Create, () => target.Create(D)));
        }

        public virtual IAuthorizedVoidCompiles? LocalInsert(IAuthorizedVoidCompiles itarget, int A)
        {
            var target = (AuthorizedVoidCompiles)itarget ?? throw new Exception("AuthorizedVoidCompiles must implement IAuthorizedVoidCompiles");
            return DoMapperMethodCall<IAuthorizedVoidCompiles>(target, DataMapperMethod.Insert, () => target.Insert(A));
        }

        public virtual IAuthorizedVoidCompiles? LocalInsert1(IAuthorizedVoidCompiles itarget, double B)
        {
            var target = (AuthorizedVoidCompiles)itarget ?? throw new Exception("AuthorizedVoidCompiles must implement IAuthorizedVoidCompiles");
            return DoMapperMethodCall<IAuthorizedVoidCompiles>(target, DataMapperMethod.Insert, () => target.Insert(B));
        }

        public virtual IAuthorizedVoidCompiles? LocalInsert2(IAuthorizedVoidCompiles itarget, Guid C)
        {
            var target = (AuthorizedVoidCompiles)itarget ?? throw new Exception("AuthorizedVoidCompiles must implement IAuthorizedVoidCompiles");
            return DoMapperMethodCall<IAuthorizedVoidCompiles>(target, DataMapperMethod.Insert, () => target.Insert(C));
        }

        public virtual IAuthorizedVoidCompiles? LocalInsert3(IAuthorizedVoidCompiles itarget, long D)
        {
            var target = (AuthorizedVoidCompiles)itarget ?? throw new Exception("AuthorizedVoidCompiles must implement IAuthorizedVoidCompiles");
            return DoMapperMethodCall<IAuthorizedVoidCompiles>(target, DataMapperMethod.Insert, () => target.Insert(D));
        }

        // Not able to call WriteString due to parameter mismatch;
        // Not able to call WriteBoolTask due to parameter mismatch;
        // Not able to call WriteLongTask due to parameter mismatch;
        public IAuthorizedVoidCompiles? Save(IAuthorizedVoidCompiles target, int A)
        {
            var authorized = (TrySave(target, A));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedVoidCompiles> TrySave(IAuthorizedVoidCompiles target, int A)
        {
            Authorized writebool = IAuthorizationCompiles.WriteBool(A);
            if (!writebool.HasAccess)
            {
                return new Authorized<IAuthorizedVoidCompiles>(writebool);
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
                return new Authorized<IAuthorizedVoidCompiles>(LocalInsert(target, A));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // Not able to call WriteBool due to parameter mismatch;
        // Not able to call WriteBoolTask due to parameter mismatch;
        // Not able to call WriteLongTask due to parameter mismatch;
        public IAuthorizedVoidCompiles? Save(IAuthorizedVoidCompiles target, double B)
        {
            var authorized = (TrySave(target, B));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedVoidCompiles> TrySave(IAuthorizedVoidCompiles target, double B)
        {
            Authorized writestring = IAuthorizationCompiles.WriteString(B);
            if (!writestring.HasAccess)
            {
                return new Authorized<IAuthorizedVoidCompiles>(writestring);
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
                return new Authorized<IAuthorizedVoidCompiles>(LocalInsert1(target, B));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // Not able to call WriteBool due to parameter mismatch;
        // Not able to call WriteString due to parameter mismatch;
        // Not able to call WriteLongTask due to parameter mismatch;
        public async Task<IAuthorizedVoidCompiles?> Save(IAuthorizedVoidCompiles target, Guid C)
        {
            var authorized = (await TrySave(target, C));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedVoidCompiles>> TrySave(IAuthorizedVoidCompiles target, Guid C)
        {
            Authorized writebooltask = await IAuthorizationCompiles.WriteBoolTask(C);
            if (!writebooltask.HasAccess)
            {
                return new Authorized<IAuthorizedVoidCompiles>(writebooltask);
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
                return new Authorized<IAuthorizedVoidCompiles>(LocalInsert2(target, C));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // Not able to call WriteBool due to parameter mismatch;
        // Not able to call WriteString due to parameter mismatch;
        // Not able to call WriteBoolTask due to parameter mismatch;
        public async Task<IAuthorizedVoidCompiles?> Save(IAuthorizedVoidCompiles target, long D)
        {
            var authorized = (await TrySave(target, D));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedVoidCompiles>> TrySave(IAuthorizedVoidCompiles target, long D)
        {
            Authorized writelongtask = await IAuthorizationCompiles.WriteLongTask(D);
            if (!writelongtask.HasAccess)
            {
                return new Authorized<IAuthorizedVoidCompiles>(writelongtask);
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
                return new Authorized<IAuthorizedVoidCompiles>(LocalInsert3(target, D));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<AuthorizedVoidCompiles>();
            services.AddScoped<AuthorizedVoidCompilesFactory>();
            services.AddScoped<IAuthorizedVoidCompilesFactory, AuthorizedVoidCompilesFactory>();
            services.AddTransient<IAuthorizedVoidCompiles, AuthorizedVoidCompiles>();
            services.AddScoped<IFactoryEditBase<AuthorizedVoidCompiles>, AuthorizedVoidCompilesFactory>();
        }
    }
}