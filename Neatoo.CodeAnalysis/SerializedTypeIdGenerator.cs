//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;

//namespace Neatoo.CodeAnalysis
//{
//    [Generator(LanguageNames.CSharp)]
//    public class SerializedTypeIdGenerator : IIncrementalGenerator
//    {
//        public void Initialize(IncrementalGeneratorInitializationContext context)
//        {
//            // Register the source output
//            context.RegisterSourceOutput(context.SyntaxProvider.CreateSyntaxProvider(
//                predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
//                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
//                .Where(static m => m is not null),
//                static (ctx, source) => Execute(ctx, source));
//        }

//        public static bool IsSyntaxTargetForGeneration(SyntaxNode node)
//        {
//            return node is ClassDeclarationSyntax classDeclarationSyntax
//                    && !(classDeclarationSyntax.TypeParameterList?.Parameters.Any() ?? false || classDeclarationSyntax.Modifiers.Any(SyntaxKind.AbstractKeyword));
//        }

//        public static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
//        {
//            try
//            {
//                var classDeclaration = (ClassDeclarationSyntax)context.Node;

//                return classDeclaration;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }

//        }

//        private static long TypeId = new Random().Next(10000, 100000);

//        private static void Execute(SourceProductionContext context, ClassDeclarationSyntax classDeclarationSyntax)
//        {
//            var usingDirectives = new List<string>();
//            var messages = new List<string>();
//            try
//            {
//                // Generate the source code for the found method
//                var className = classDeclarationSyntax.Identifier.Text;
//                string? namespaceName = FactoryGenerator.FindNamespace(classDeclarationSyntax);

//                if(namespaceName == null)
//                {
//                    return;
//                }

//                if (classDeclarationSyntax.Parent is ClassDeclarationSyntax)
//                {
//                    return;
//                }

//                StringBuilder staticClasses = new StringBuilder();

//                NestedClasses(classDeclarationSyntax, staticClasses);

//                var source = $@"

//using Neatoo.Portal;

///*
//Debugging Messages:
//{string.Join("\n", messages)}
//*/


//namespace {namespaceName}
//{{
//    {staticClasses.ToString()}
//}}";

//                source = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().SyntaxTree.GetText().ToString();
//                context.AddSource($"{namespaceName}.{className}TypeId.g.cs", source);
//            }
//            catch (Exception ex)
//            {
//                context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("NT0001", "Error", ex.Message, "Error", DiagnosticSeverity.Error, true), Location.None));
//            }
//        }

//        private static void NestedClasses(ClassDeclarationSyntax classDeclarationSyntax, StringBuilder staticClasses)
//        {

//            staticClasses.AppendLine($"internal static partial class {classDeclarationSyntax.Identifier.Text}TypeId");
//            staticClasses.AppendLine("{");
//            staticClasses.AppendLine($"public static long TypeId = {TypeId++};");
//            foreach (var member in classDeclarationSyntax.Members)
//            {
//                if (member is ClassDeclarationSyntax nestedClass)
//                {
//                    NestedClasses(nestedClass, staticClasses);
//                }
//            }
//            staticClasses.AppendLine("}");
//        }
//    }
//}