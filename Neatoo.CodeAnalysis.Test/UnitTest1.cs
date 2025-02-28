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



internal class BaseHasAttributes : SharedObject<BaseHasAttributes> {

}


[Factory]
internal class BaseObject {


    [Insert]
    public bool Insert(){

    }

    [Update]
    public bool Update(){
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

namespace Neatoo;

[Factory]
[Authorize<IAuthorizeBaseObject>]
internal class BaseObject {

        [Create]
        public void Create(VoidTaskStringDeny v) { List.Add(v); }

        [Insert]
        public void Insert(VoidTaskStringDeny v) { List.Add(v); }
}
";

        var source2 = @"
using Neatoo;

namespace Neatoo;

public interface IAuthorizeBaseObject {


        [Authorize(DataMapperMethodType.Write)]
        bool Create(Int v);

        [Authorize(DataMapperMethodType.Write)]
        bool Insert(int v);
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