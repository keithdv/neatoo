using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neatoo.AuthorizationRules;
using Neatoo.Internal;
using Neatoo.Portal;
using Neatoo.Portal.Internal;
using Neatoo.UnitTest.ObjectPortal;
using Neatoo.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.Portal;

[TestClass]
public class AuthorizationClassTests
{


    public interface IAuthorization
    {
        [Authorize(DataMapperMethodType.Read | DataMapperMethodType.Write)]
        bool AnyAccess();

        [Authorize(DataMapperMethodType.Read | DataMapperMethodType.Write)]
        string? AnyAccess(int p);
    }
    internal class Authorization : IAuthorization
    {
        public bool AnyAccess()
        {
            return true;
        }


        public string? AnyAccess(int p)
        {
            return "This means auth failed";
        }
    }

    public interface IDemoObject : IEditMetaSaveProperties { }

    [Factory]
    [Authorize<IAuthorization>]
    public class DemoObject : IDemoObject
    {
        public bool IsDeleted => throw new NotImplementedException();

        public bool IsNew => throw new NotImplementedException();

        [Create]
        public void Create()
        {

        }

        [Create]
        public bool CreateCanReturnNull()
        {
            return true;
        }

        [Create]
        public Task CreateAsync([Service] IDisposableDependency disposableDependency)
        {
            return Task.CompletedTask;
        }


        [Remote]
        [Create]
        public bool CreateRemote()
        {
            return true;
        }

        [Remote]
        [Insert]
        public void Insert([Service] IDisposableDependency disposableDependency)
        {

        }

        [Update]
        public void Update([Service] IDisposableDependency disposableDependency)
        {

        }

        [Delete]
        public void Delete([Service] IDisposableDependency disposableDependency)
        {
        }

    }

}
