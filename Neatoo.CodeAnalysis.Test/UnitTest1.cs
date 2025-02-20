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
internal class SharedObject<T> {
    [Create]
    public void Create(long sharedParameter){

    }

}


[Factory]
internal class EditObject : EditBase<EditObject> {

}


[Factory]
internal class BaseObject : SharedObject<BaseObject> {


    [Create]
    public void Create(int parameter){

    }

    [Create]
    public Task Create(string parameter){

    }

    [Create]
    public Task Create(string parameter){

    }

    [Create]
    public Task Create(Guid parameter, [Service] IDependency dependency){

    }

    [Fetch]
    public void Fetch(int parameter){

    }

    [Fetch]
    public Task Fetch(string parameter){

    }

    [Fetch]
    public Task Fetch(string parameter1, int parameter2){

    }

#if CLIENT

    [Fetch]
    public Task Fetch(Guid parameter, [Service] IDependency dependency){

    }

    [Insert]
    public Task Insert([Service] IDependency dependency){

    }

    [Update]
    public Task Update([Service] IDependency dependency){
    }

    [Delete]
    public Task Delete([Service] IDependency dependency){
    }

    [Insert]
    public Task Insert(int parameter, [Service] IDependency dependency){

    }

    [Update]
    public Task Update(int parameter, [Service] IDependency dependency){
    }

    [Delete]
    public Task Delete(int parameter, [Service] IDependency dependency){
    }


    [Insert]
    public void Insert(string parameter){

    }
    [Update]
    public Task Update(string parameter, [Service] IDependency dependency){
    }

    [Delete]
    public void Delete(string parameter){
    }

    [Update]
    public Task UpdateList(Guid makeUnique){
        // Lists only have an update, don't force to have a corresponding insert and delete
    }
#endif
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

namespace NeatooLibrary.Specific {

    [Factory]
    internal class EditObject : EditBaseA<EditObject> {

    }

    internal abstract class EditBaseA<T> : EditBaseB<T> {

    }

}";

var source2 = @"

namespace NeatooLibrary {

internal abstract class EditBaseB<T> : EditBase<T> {

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

}