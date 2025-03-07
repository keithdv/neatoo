using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Neatoo.AuthorizationRules;
using Neatoo.Internal;
using Neatoo.Portal;
using Neatoo.UnitTest.Demo;

namespace Neatoo.UnitTest.Portal;


public class OuterClass
{
    public class InnerClass
    {
    }
}

[TestClass]
public class PersonObjectTests
{

    public interface IAuthorizePersonObject
    {
        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        bool CanRead();

        [Authorize(DataMapperMethodType.Read)]
        bool CanRead(int a);

        [Authorize(DataMapperMethodType.Write)]
        Task<bool> CanWriteTarget();

        [Authorize(DataMapperMethodType.Insert)]
        Task<bool> CanWrite();

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        bool CanInsertRemote();
    }

    public interface IPersonObject : IEditMetaSaveProperties
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


    [Factory]
    [Authorize<IAuthorizePersonObject>()]
    public class PersonObject : IPersonObject
    {
        public PersonObject()
        {

        }

        public bool IsDeleted { get; set; } = false;

        public bool IsNew { get; set; } = true;

        public string FirstName { get; set; }
        public string LastName { get; set; }

        
        [Insert]
        public void InsertA(OuterClass.InnerClass innerClass, [Service] IDisposable disposable)
        {

        }

        [Update]
        public void UpdateA()
        {

        }

        [Delete]
        public void DeleteA()
        {

        }

    }

    //[TestClass]
    //public class PersonObjectFactoryTests
    //{
    //    private IServiceScope serverScope;
    //    private IServiceScope clientScope;
    //    private IPersonObjectFactory factory;

    //    [TestInitialize]
    //    public void TestIntialize()
    //    {
    //        // Simulates a client to server call
    //        // including serialization and deserialization
    //        var scopes = FactoryContainers.Scopes();
    //        serverScope = scopes.server;
    //        clientScope = scopes.client;
    //        factory = clientScope.ServiceProvider.GetRequiredService<IPersonObjectFactory>();
    //    }

    //    //[TestMethod]
    //    //public void PersonObjectFactoryTest_Create()
    //    //{
    //    //    var result = factory.Create("John", "Doe");

    //    //    //factory.CanCreate()
    //    //   // Assert.AreEqual("John", result.FirstName);

    //    //    //Assert.IsNull(result);
    //    //}

    //    //[TestMethod]
    //    //public void PersonObjectFactoryTest_Save()
    //    //{
    //    //    var result = factory.Create("John", "Doe");

    //    //    factory.Save(result);
    //    //}
    //}
}
