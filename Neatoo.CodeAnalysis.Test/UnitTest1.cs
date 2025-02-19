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
internal class BaseHasAttributes : SharedObject<BaseHasAttributes> {

}

internal class SharedObject<T> {
    [Create]
    public void Create(long sharedParameter){

    }

}

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
}
";

        // Pass the source code to our helper and snapshot test the output
        return TestHelper.Verify(source);
    }
}