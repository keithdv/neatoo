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

public class BaseObject {


    [Create]
    public void Create(int parameter){

    }

    [Create]
    public Task Create(string parameter){

    }

    [Create]
    public Task CreateString(string parameter){

    }

    [Create]
    public Task CreateDependency(Guid parameter, [Service] IDependency dependency){

    }

    [Fetch]
    public void Fetch(int parameter){

    }

    [Fetch]
    public Task Fetch(string parameter){

    }

    [Fetch]
    public Task FetchString(string parameter1, int parameter2){

    }

    [Fetch]
    public Task FetchDependency(Guid parameter, [Service] IDependency dependency){

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
    public Task InsertCriteria(int parameter, [Service] IDependency dependency){

    }

    [Update]
    public Task UpdateCriteria(int parameter, [Service] IDependency dependency){
    }

    [Delete]
    public Task DeleteCriteria(int parameter, [Service] IDependency dependency){
    }


    [Insert]
    public Task InsertCriteria(string parameter, [Service] IDependency dependency){

    }

    [Update]
    public Task UpdateCriteria(string parameter, [Service] IDependency dependency){
    }

    [Delete]
    public Task DeleteCriteria(string parameter, [Service] IDependency dependency){
    }

}
";

        // Pass the source code to our helper and snapshot test the output
        return TestHelper.Verify(source);
    }
}