using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Neatoo.CodeAnalysis
{
    [Generator(LanguageNames.CSharp)]
    public class ServiceRegistrarGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Register the source output
            context.RegisterSourceOutput(context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (s, _) => FactoryGenerator.IsSyntaxTargetForGeneration(s),
                transform: static (ctx, _) => FactoryGenerator.GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null),
                static (ctx, source) => Execute(ctx, source!.Value.classDeclaration, source.Value.semanticModel));
        }

        private static List<string> dataMapperAttributes = new() { "Create", "Fetch", "Insert", "Update", "Delete" };
        private static List<string> dataMapperSaveAttributes = new() { "Insert", "Update", "Delete" };

        private static void Execute(SourceProductionContext context, ClassDeclarationSyntax classDeclarationSyntax, SemanticModel semanticModel)
        {
            var usingDirectives = new List<string>();
            var messages = new List<string>();

            // Generate the source code for the found method
            var className = classDeclarationSyntax.Identifier.Text;
            String namespaceName = "FAILURE";

            if (classDeclarationSyntax.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax)
            {
                namespaceName = namespaceDeclarationSyntax.Name.ToString();
            }
            else if (classDeclarationSyntax.Parent is FileScopedNamespaceDeclarationSyntax parentClassDeclarationSyntax)
            {
                namespaceName = parentClassDeclarationSyntax.Name.ToString();
            }


            var source = $@"
using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
{string.Join("\n", usingDirectives)}
/*
Debugging Messages:
{string.Join("\n", messages)}
*/
namespace {namespaceName}
{{

    internal partial class ServiceRegistrar : ServiceRegistrarBase
    {{


    }}
}}";

            source = source.Replace(", )", ")");
            source = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().SyntaxTree.GetText().ToString();
            context.AddSource($"{namespaceName}.{className}ServiceRegistrar.g.cs", source);
        }

    }
}