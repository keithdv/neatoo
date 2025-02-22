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
using NeatooLibrary;

namespace NeatooLibrary.Specific {


    internal class EditObject : EditBaseA<EditObject> {

    }

    [Factory]
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

public interface IAuthorizeBaseObject {

    [Authorize(DataMapperMethodType.Read)]
    IAuthorizationRuleResult Create();

    [Authorize(DataMapperMethodType.Read)]
    IAuthorizationRuleResult Create(int parameter);
    
    [Authorize(DataMapperMethodType.Read)]
    Task<IAuthorizationRuleResult> CreateAuthorizeAsync(int parameter);
}

[Factory]
[Authorize<IAuthorizeBaseObject>]
internal class BaseObject : SharedObject<BaseObject> {


    [Create]
    public void Create(){

    }

    [Create]
    public void Create(int parameter){

    }

    [Create]
    public void CreateAuthorizeAsync(int parameter){

    }
}
";

        // Pass the source code to our helper and snapshot test the output
        return TestHelper.Verify(source);
    }

}