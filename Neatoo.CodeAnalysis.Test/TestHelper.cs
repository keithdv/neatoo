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
        var syntaxTrees = new List<SyntaxTree>() { CSharpSyntaxTree.ParseText(source) };
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
}
