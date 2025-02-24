using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.CodeAnalysis.Test;

public static class TestHelper
{
    public static Task Verify(string source, string? source2 = null)
    {
        // Parse the provided string into a C# syntax tree
        var syntaxTrees = new List<SyntaxTree>() { CSharpSyntaxTree.ParseText(source), CSharpSyntaxTree.ParseText(SourceAttributes) };
        if (source2 != null)
        {
            syntaxTrees.Add(CSharpSyntaxTree.ParseText(source2));
        }

            // Create a Roslyn compilation for the syntax tree.
            CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: syntaxTrees.ToArray());


        // Create an instance of our EnumGenerator incremental source generator
        var generator = new FactoryGenerator();

        // The GeneratorDriver is used to run our generator against a compilation
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Run the source generator!
        driver = driver.RunGenerators(compilation);

        driver.GetRunResult().Results.SelectMany(r => r.GeneratedSources).ToList().ForEach(g => Console.WriteLine(g));
        // Use verify to snapshot test the source generator output!
        return Verifier.Verify(driver);
    }

    public static string SourceAttributes = @"

using System;

namespace Neatoo;

[System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class FactoryAttribute : Attribute
{
    public FactoryAttribute()
    {
    }
}

[System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class SuppressFactoryAttribute : Attribute
{
    public SuppressFactoryAttribute()
    {
    }
}

[System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class FactoryAttribute<T> : Attribute
{
    public FactoryAttribute()
    {
    }
}

[System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class RemoteAttribute : Attribute
{
    public RemoteAttribute()
    {
    }
}

[System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class DataMapperMethodAttribute : Attribute
{
    public DataMapperMethod Operation { get; }

    public DataMapperMethodAttribute(DataMapperMethod operation)
    {
        this.Operation = operation;
    }
}

[System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
public sealed class ServiceAttribute : Attribute
{
    public ServiceAttribute()
    {
    }
}

public sealed class CreateAttribute : DataMapperMethodAttribute
{
    public CreateAttribute() : base(DataMapperMethod.Create)
    {
    }
}

public sealed class FetchAttribute : DataMapperMethodAttribute
{
    public FetchAttribute() : base(DataMapperMethod.Fetch)
    {
    }
}

public sealed class InsertAttribute : DataMapperMethodAttribute
{
    public InsertAttribute() : base(DataMapperMethod.Insert)
    {
    }
}

public sealed class UpdateAttribute : DataMapperMethodAttribute
{
    public UpdateAttribute() : base(DataMapperMethod.Update)
    {
    }
}

public sealed class DeleteAttribute : DataMapperMethodAttribute
{
    public DeleteAttribute() : base(DataMapperMethod.Delete)
    {
    }
}

public sealed class  ExecuteAttribute<D> : DataMapperMethodAttribute
    where D : Delegate
{
    public ExecuteAttribute() : base(DataMapperMethod.Execute)
    {
    }
}

[System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class AuthorizeAttribute<T> : Attribute
{
    public AuthorizeAttribute()
    {

    }
}


[System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class AuthorizeAttribute : DataMapperMethodAttribute
{
    public DataMapperMethodType DataMapperMethodType { get; }
    public AuthorizeAttribute(DataMapperMethodType operation) : base(DataMapperMethod.Authorize)
    {
        this.DataMapperMethodType = operation;
    }
}

    public enum DataMapperMethodType
    {
        Create = 1,
        Fetch = 2,
        Insert = 4,
        Update = 8,
        Delete = 16,
        Read = 64,
        Write = 128,
        Authorization = 256,
    }

    public enum DataMapperMethod
    {
        Execute = DataMapperMethodType.Read,
        Create = DataMapperMethodType.Create | DataMapperMethodType.Read,
        Fetch = DataMapperMethodType.Fetch | DataMapperMethodType.Read,
        Insert = DataMapperMethodType.Insert | DataMapperMethodType.Write,
        Update = DataMapperMethodType.Update | DataMapperMethodType.Write,
        Delete = DataMapperMethodType.Delete | DataMapperMethodType.Write,
        Authorize = DataMapperMethodType.Authorization,
    }


";
}
