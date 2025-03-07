using VerifyXunit;
using Xunit;

namespace Neatoo.CodeAnalysis.Test;

public class FactoryGeneratorTests
{
    [Fact]
    public Task FactoryGenerator()
    {
        // The source code to test
        var source = @"
using Neatoo;

namespace Neatoo;
        [Factory]
        public class WriteDataMapper
        {
            public bool IsDeleted { get; set; }

            public bool IsNew { get; set; }

            public bool InsertCalled { get; set; }
            [Insert]
            public void InsertVoid()
            {
                InsertCalled = true;
            }

            [Insert]
            public bool InsertBool()
            {
                InsertCalled = true;
                return true;
            }

            [Insert]
            public Task InsertTask()
            {
                InsertCalled = true;
                return Task.CompletedTask;
            }

            [Insert]
            public Task<bool> InsertTaskBool()
            {
                InsertCalled = true;
                return Task.FromResult(true);
            }

            [Insert]
            public void InsertVoid(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
            }

            [Insert]
            public bool InsertBool(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                return true;
            }

            [Insert]
            public Task InsertTask(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                return Task.CompletedTask;
            }

            [Insert]
            public Task<bool> InsertTaskBool(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(true);
            }

            [Insert]
            public Task<bool> InsertTaskBoolFalse(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(false);
            }

            [Insert]
            public void InsertVoidDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Insert]
            public bool InsertBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Insert]
            public bool InsertBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Insert]
            public Task InsertTaskDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Insert]
            public Task<bool> InsertTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Insert]
            public Task<bool> InsertTaskBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(false);
            }

            [Insert]
            public void InsertVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Insert]
            public bool InsertBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Insert]
            public bool InsertBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Insert]
            public Task InsertTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Insert]
            public Task<bool> InsertTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }



            public bool UpdateCalled { get; set; }
            [Update]
            public void UpdateVoid()
            {
                UpdateCalled = true;
            }

            [Update]
            public bool UpdateBool()
            {
                UpdateCalled = true;
                return true;
            }

            [Update]
            public Task UpdateTask()
            {
                UpdateCalled = true;
                return Task.CompletedTask;
            }

            [Update]
            public Task<bool> UpdateTaskBool()
            {
                UpdateCalled = true;
                return Task.FromResult(true);
            }

            [Update]
            public void UpdateVoid(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
            }

            [Update]
            public bool UpdateBool(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                return true;
            }

            [Update]
            public Task UpdateTask(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                return Task.CompletedTask;
            }

            [Update]
            public Task<bool> UpdateTaskBool(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(true);
            }

            [Update]
            public Task<bool> UpdateTaskBoolFalse(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(false);
            }

            [Update]
            public void UpdateVoidDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Update]
            public bool UpdateBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Update]
            public bool UpdateBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Update]
            public Task UpdateTaskDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Update]
            public Task<bool> UpdateTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Update]
            public Task<bool> UpdateTaskBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(false);
            }

            [Update]
            public void UpdateVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Update]
            public bool UpdateBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Update]
            public bool UpdateBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Update]
            public Task UpdateTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Update]
            public Task<bool> UpdateTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            public bool DeleteCalled { get; set; }
            [Delete]
            public void DeleteVoid()
            {
                DeleteCalled = true;
            }

            [Delete]
            public bool DeleteBool()
            {
                DeleteCalled = true;
                return true;
            }

            [Delete]
            public Task DeleteTask()
            {
                DeleteCalled = true;
                return Task.CompletedTask;
            }

            [Delete]
            public Task<bool> DeleteTaskBool()
            {
                DeleteCalled = true;
                return Task.FromResult(true);
            }

            [Delete]
            public void DeleteVoid(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
            }

            [Delete]
            public bool DeleteBool(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                return true;
            }

            [Delete]
            public Task DeleteTask(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                return Task.CompletedTask;
            }

            [Delete]
            public Task<bool> DeleteTaskBool(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(true);
            }

            [Delete]
            public Task<bool> DeleteTaskBoolFalse(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(false);
            }

            [Delete]
            public void DeleteVoidDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Delete]
            public bool DeleteBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Delete]
            public bool DeleteBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Delete]
            public Task DeleteTaskDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Delete]
            public Task<bool> DeleteTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Delete]
            public Task<bool> DeleteTaskBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(false);
            }

            [Delete]
            public void DeleteVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Delete]
            public bool DeleteBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Delete]
            public bool DeleteBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Delete]
            public Task DeleteTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Delete]
            public Task<bool> DeleteTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

        }
";

        // Pass the source code to our helper and snapshot test the output
        return TestHelper.Verify(source);
    }

    [Fact]
    public Task AbstractEditBase()
    {
        // The source code to test
        var source = @"
using Neatoo;
using NeatooLibrary;

namespace NeatooLibrary.Specific {


    internal class EditObject : EditBaseA<EditObject> {

    }

    internal abstract class EditBaseA<T> : EditBaseB<T> {

    }

[System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class FactoryAttribute : Attribute
{
    public FactoryAttribute()
    {
    }
}

}";

var source2 = @"

namespace NeatooLibrary {


[Factory]
internal abstract class EditBaseB<T> : EditBase<T> {

}



}
";

        // Pass the source code to our helper and snapshot test the output
        return TestHelper.Verify(source, source2);
    }

    [Fact]
    public Task Remote()
    {
        // The source code to test
        var source = @"
using Neatoo;

namespace Neatoo;

[Factory]
internal class BaseObject : SharedObject<BaseObject> {


    [Create]
    public void Create(int parameter){

    }

    [Create]
    public Task Create(string parameter){

    }

    [Remote]
    [Create]
    public Task Create(string parameter){

    }

}
";

        // Pass the source code to our helper and snapshot test the output
        return TestHelper.Verify(source);
    }


    [Fact]
    public void NoBaseClass()
    {
        // The source code to test
        var source = @"

namespace Neatoo;

    public interface INoBaseClass
    {
        string Name { get; set; }
        string Description { get; set; }
        string Type { get; set; }
    }

    [Factory]
    public class NoBaseClass : INoBaseClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }


        [Create]
        public void NoBaseClass_Create()
        {
            Name = ""Name"";
            Description = ""Description"";
            Type = ""Type"";
        }

    }";
        // Pass the source code to our helper and snapshot test the output
        TestHelper.Verify(source);
    }



    [Fact]
    public Task Authorization()
    {
        // The source code to test
        var source = @"
using Neatoo;
using System.Collections.Generic;

namespace Neatoo;

[Factory]
//[Authorize<AuthorizeBaseObject>]
internal class BaseObject {


    [Fetch]
    [Remote]
    public void Fetch()
    {
    }

    [Create]
    [Remote]
    public void Create(){
    }

    [Update]
    [Remote]
    public void Update()
    {
    }

}

public class Dto {}
";

        var source2 = @"
using Neatoo;

namespace Neatoo;

public class AuthorizeBaseObject {

        [Authorize(DataMapperMethodType.Read | DataMapperMethodType.Write)]
        bool AnyAccess(BaseObject b);

}
";
        // Pass the source code to our helper and snapshot test the output
        return TestHelper.Verify(source, source2);
    }



    [Fact]
    public Task AuthorizationConcrete()
    {
        // The source code to test
        var source = @"
using Neatoo;

namespace Neatoo;

[Factory]
[Authorize<AuthorizeBaseObject>]
internal class BaseObject {


        [Create]
        public void Create(VoidBool voidBool)
        {
            List.Add(voidBool);
        }

        [Create]
        public Task Create(TaskBool taskBool)
        {
            List.Add(taskBool);
            return Task.CompletedTask;
        }

        [Create]
        public void Create(VoidBoolRemote voidBoolRemote)
        {
            List.Add(voidBoolRemote);
        }

        [Insert]
        public void Insert(VoidBool voidBool)
        {
            List.Add(voidBool);
        }

        [Insert]
        public Task Insert(TaskBool taskBool)
        {
            List.Add(taskBool);
            return Task.CompletedTask;
        }

        [Create]
        public void Create(VoidTaskStringDeny v) { List.Add(v); }

        [Insert]
        public void Insert(VoidTaskStringDeny v) { List.Add(v); }
}
";

        var source2 = @"
using Neatoo;

namespace Neatoo;

public class AuthorizeBaseObject {


        [Authorize(DataMapperMethodType.Read)]
        bool CanRead(VoidBool voidBool);

        [Authorize(DataMapperMethodType.Read)]
        bool CanRead(TaskBool voidBool);

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        bool CanRead(VoidBoolRemote voidBoolRemote);

        [Authorize(DataMapperMethodType.Write)]
        bool CanWrite(VoidBool voidBool);

        [Authorize(DataMapperMethodType.Write)]
        bool CanWrite(TaskBool voidBool);

        [Authorize(DataMapperMethodType.Write)]
        bool CanWrite(VoidBoolRemote voidBoolRemote);

        [Authorize(DataMapperMethodType.Read)]
        Task<bool> Read(VoidTaskStringDeny v);

        [Authorize(DataMapperMethodType.Write)]
        Task<string> Write(VoidTaskStringDeny v);
   
}


";
        // Pass the source code to our helper and snapshot test the output
        return TestHelper.Verify(source, source2);
    }
}