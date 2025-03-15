﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Neato.BaseGenerator
{


    [Generator(LanguageNames.CSharp)]
    public class PartialBaseGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context) =>
            // Register the source output
            context.RegisterSourceOutput(context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null),
                static (ctx, source) => Execute(ctx, source!.Value.classDeclaration, source.Value.semanticModel));

        public static bool IsSyntaxTargetForGeneration(SyntaxNode node) => node is ClassDeclarationSyntax classDeclarationSyntax
                     && !(classDeclarationSyntax.TypeParameterList?.Parameters.Any() ?? false || classDeclarationSyntax.Modifiers.Any(SyntaxKind.AbstractKeyword))
                     && !(classDeclarationSyntax.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "SuppressFactory"));

        public static (ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            try
            {
                var classDeclaration = (ClassDeclarationSyntax)context.Node;

                var classNamedTypeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);

                if (classNamedTypeSymbol == null)
                {
                    return null;
                }

                if(classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)) && ClassOrBaseClassIsBaseClass(classNamedTypeSymbol))
                {
                    return (classDeclaration, context.SemanticModel);
                }

                return null;
            }
            catch (Exception)
            {

            }

            return null;
        }

        private static bool ClassOrBaseClassIsBaseClass(INamedTypeSymbol namedTypeSymbol)
        {
            if(namedTypeSymbol.Name == "Base" && namedTypeSymbol.ContainingNamespace.Name == "Neatoo")
            {
                return true;
            }
            if (namedTypeSymbol.BaseType != null)
            {
                return ClassOrBaseClassIsBaseClass(namedTypeSymbol.BaseType);
            }
            return false;
        }

        internal class PartialBaseText
        {
            public StringBuilder PropertyDeclarations { get; set; } = new();
        }

        private static void Execute(SourceProductionContext context, ClassDeclarationSyntax classDeclarationSyntax, SemanticModel semanticModel)
        {
            var messages = new List<string>();
            string source;

            try
            {
                var classNamedSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax) ?? throw new Exception($"Cannot get named symbol for {classDeclarationSyntax}");

                var usingDirectives = new List<string>() { "using Neatoo;", "using Microsoft.Extensions.DependencyInjection;" };
                var partialText = new PartialBaseText();
                PartialBaseText? interfacePartialText = null;
                var targetClassName = classNamedSymbol.Name;
                // Generate the source code for the found method
                var namespaceName = FindNamespace(classDeclarationSyntax);

                var accessModifier = classNamedSymbol.DeclaredAccessibility.ToString();

                var interfaceSyntax = classNamedSymbol.Interfaces.FirstOrDefault(i => i.Name == $"I{classNamedSymbol.Name}");

                if (interfaceSyntax != null)
                    //&& 
                    //                    i.DeclaringSyntaxReferences
                    //                    .OfType<InterfaceDeclarationSyntax>()
                    //                    .Any(ids => ids.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))))
                {
                    var interfaceDeclarationSyntax = classNamedSymbol.Interfaces.First(i => i.Name == $"I{classNamedSymbol.Name}").DeclaringSyntaxReferences.First().GetSyntax() as InterfaceDeclarationSyntax;

                    if (interfaceDeclarationSyntax != null && interfaceDeclarationSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
                    {
                        interfacePartialText = new PartialBaseText();
                    }
                }

                foreach (var property in classDeclarationSyntax.Members.OfType<PropertyDeclarationSyntax>())
                {
                    if(property.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
                    {
                        var accessibility = property.Modifiers.First().ToString();
                        var propertyType = property.Type.ToString();
                        var propertyName = property.Identifier.Text;
                        
                        var propertySymbol = semanticModel.GetDeclaredSymbol(property) ?? throw new Exception($"Cannot get named symbol for {property}");


                        partialText.PropertyDeclarations.AppendLine($"{accessibility} partial {propertyType} {propertyName} {{ get => Getter<{propertyType}>();  set=>Setter(value); }}");
                        if(interfacePartialText != null)
                        {
                            interfacePartialText.PropertyDeclarations.AppendLine($"{propertyType} {propertyName} {{ get; set; }}");
                        }

                    }
                }

                try
                {
                    var interfaceSource = "";

                    if (interfacePartialText != null)
                    {
                        interfaceSource = $@"
                        public partial interface I{targetClassName} {{
                            {interfacePartialText.PropertyDeclarations}
                        }}";
                    }

                        source =  $@"
                    #nullable enable

                    {WithStringBuilder(usingDirectives)}
                    /*
                    Debugging Messages:
                    Yay!
                    {WithStringBuilder(messages)}
                    namespace {namespaceName}
                    {{
                        {interfaceSource}
                        {accessModifier.ToLower()} partial class {classNamedSymbol.Name} {{
                            {partialText.PropertyDeclarations}
                        }}

                    }}";
                    source = source.Replace("[, ", "[");
                    source = source.Replace("(, ", "(");
                    source = source.Replace(", )", ")");
                    source = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().SyntaxTree.GetText().ToString();
                }
                catch (Exception ex)
                {
                    source = @$"/* Error: {ex.GetType().FullName} {ex.Message} */";
                }

                context.AddSource($"{namespaceName}.{targetClassName}.g.cs", source);
            }
            catch (Exception ex)
            {
                source = $"// Error: {ex.Message}";
            }
        }



        public static string? FindNamespace(SyntaxNode syntaxNode)
        {
            if (syntaxNode.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax)
            {
                return namespaceDeclarationSyntax.Name.ToString();
            }
            else if (syntaxNode.Parent is FileScopedNamespaceDeclarationSyntax parentClassDeclarationSyntax)
            {
                return parentClassDeclarationSyntax.Name.ToString();
            }
            else if (syntaxNode.Parent != null)
            {
                return FindNamespace(syntaxNode.Parent);
            }
            else
            {
                return null;
            }
        }

        public static string WithStringBuilder(IEnumerable<string> strings)
        {
            var sb = new StringBuilder();
            foreach (var s in strings)
            {
                sb.AppendLine(s);
            }
            return sb.ToString();
        }
    }
}