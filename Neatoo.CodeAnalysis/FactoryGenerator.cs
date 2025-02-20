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
    public class FactoryGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Register the source output
            context.RegisterSourceOutput(context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null),
                static (ctx, source) => Execute(ctx, source!.Value.classDeclaration, source.Value.semanticModel));
        }

        private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        {
            if(node is IfDirectiveTriviaSyntax ifDirectiveTriviaSyntax)
            {
                return false;
            }

            // We are looking for methods with attributes
            return node is ClassDeclarationSyntax classDeclarationSyntax &&
                    !(classDeclarationSyntax.TypeParameterList?.Parameters.Any() ?? false || classDeclarationSyntax.Modifiers.Any(SyntaxKind.AbstractKeyword)) &&
                      classDeclarationSyntax.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "Factory");
        }

        private static (ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            if (classDeclaration.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "Factory"))
            {
                return (classDeclaration, context.SemanticModel);
            }

            foreach (var methodDeclaration in classDeclaration.Members.OfType<MethodDeclarationSyntax>())
            {
                // Check if the method has any attributes named "Create"
                foreach (var attributeList in methodDeclaration.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        if (dataMapperAttributes.Contains(attribute.Name.ToString()))
                        {
                            return (classDeclaration, context.SemanticModel);
                        }
                    }
                }
            }

            return null;
        }

        private static List<string> dataMapperAttributes = new() { "Create", "Fetch", "Insert", "Update", "Delete" };
        private static List<string> dataMapperSaveAttributes = new() { "Insert", "Update", "Delete" };

        private static void Execute(SourceProductionContext context, ClassDeclarationSyntax classDeclarationSyntax, SemanticModel semanticModel)
        {
            var usingDirectives = new List<string>();
            var messages = new List<string>();
            var methodNames = new List<string>();

            var delegates = new StringBuilder();
            var delegateAssignmentLocal = new StringBuilder();
            var delegateAssignmentRemote = new StringBuilder();

            var propertyDeclarations = new StringBuilder();
            var publicMethods = new StringBuilder();
            var localMethods = new StringBuilder();
            var remoteMethods = new StringBuilder();
            var saveMethods = new StringBuilder();
            var interfaceMethods = new StringBuilder();

            Dictionary<string, (string parameterIdentifiersText, string parameterDeclarationText, string saveMethodName, string saveUniqueMethodName,
                                    string? insertMethodName, string? updateMethodName, string? deleteMethodName)> saveMethodSets = new();
            var isEdit = false;


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

            while (classDeclarationSyntax != null)
            {

                var compilationUnitSyntax = classDeclarationSyntax.SyntaxTree.GetCompilationUnitRoot();
                foreach (var using_ in compilationUnitSyntax.Usings)
                {
                    if (!usingDirectives.Contains(using_.ToString()))
                    {
                        usingDirectives.Add(using_.ToString());
                    }
                }

                messages.Add(classDeclarationSyntax.BaseList?.ToString());

                if (classDeclarationSyntax.BaseList?.Types.Any(t => t.Type.ToString() == "EditBase<T>" || t.Type.ToString() == $"EditBase<{classDeclarationSyntax.Identifier.Text.TrimEnd()}>") ?? false)
                {
                    isEdit = true;
                }

                foreach (var methodDeclaration in classDeclarationSyntax.Members.OfType<MethodDeclarationSyntax>())
                {
                    foreach (var attribute in methodDeclaration.AttributeLists.SelectMany(a => a.Attributes))
                    {
                        if (dataMapperAttributes.Contains(attribute.Name.ToString()))
                        {
                            var attributeName = attribute.Name.ToString();
                            var uniqueMethodName = methodDeclaration.Identifier.Text;
                            var methodName = methodDeclaration.Identifier.Text;
                            var isSaveMethod = dataMapperSaveAttributes.Contains(attributeName);

                            if (methodNames.Contains(uniqueMethodName))
                            {
                                var count = 1;
                                while (methodNames.Contains($"{uniqueMethodName}{count}"))
                                {
                                    count += 1;
                                }
                                uniqueMethodName = $"{uniqueMethodName}{count}";
                            }
                            methodNames.Add(uniqueMethodName);

                            var parameters = methodDeclaration.ParameterList.Parameters;
                            var parameterIdentifiers = parameters.Select(p => p.Identifier.ToString()).ToList();
                            var serviceAssignmentsText = new StringBuilder();
                            var parameterIdentifiersNoServices = new List<string>();
                            var parameterDeclarationsNoServices = new List<string>();
                            var doRemoteCall = false;

                            if(methodDeclaration.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "Remote"))
                            {
                                doRemoteCall = true;
                            }

                            foreach (var parameter in parameters)
                            {
                                if (parameter.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "Service"))
                                {
                                    serviceAssignmentsText.AppendLine($"\t\t\tvar {parameter.Identifier} = ServiceProvider.GetService<{parameter.Type}>();");
                                    doRemoteCall = true;
                                }
                                else
                                {
                                    parameterDeclarationsNoServices.Add($"{parameter.Type} {parameter.Identifier}");
                                    parameterIdentifiersNoServices.Add($"{parameter.Identifier}");
                                }
                            }

                            var parameterIdentifiersText = string.Join(", ", parameters.Select(p => p.Identifier.ToString()));
                            var parameterIdentifiersNoServicesText = string.Join(", ", parameterIdentifiersNoServices);
                            var parameterDeclarationsNoServicesText = string.Join(", ", parameterDeclarationsNoServices);
                            var delegateName = $"{uniqueMethodName}Delegate";

                            // Consumer Delegates

                            // Local method implementations
                            if (!isSaveMethod)
                            {
                                interfaceMethods.AppendLine($"Task<I{className}> {methodName}({parameterDeclarationsNoServicesText});");


                                if (doRemoteCall)
                                {
                                    propertyDeclarations.AppendLine($"protected {delegateName} {uniqueMethodName}Property {{ get; }}");
                                    delegateAssignmentLocal.AppendLine($"{uniqueMethodName}Property = Local{uniqueMethodName};");

                                    delegates.AppendLine($"protected internal delegate Task<I{className}> {delegateName}({parameterDeclarationsNoServicesText});");
                                    localMethods.AppendLine($"[Local<{delegateName}>]");

                                    publicMethods.AppendLine($"public Task<I{className}> {methodName}({parameterDeclarationsNoServicesText})");
                                    publicMethods.AppendLine("{");
                                    publicMethods.AppendLine($"return {uniqueMethodName}Property({parameterIdentifiersNoServicesText});");
                                    publicMethods.AppendLine("}");

                                    localMethods.AppendLine($"protected async Task<I{className}> Local{uniqueMethodName}({parameterDeclarationsNoServicesText})");
                                }
                                else
                                {
                                    // Assume we can just call local if there are no services
                                    // Don't need the delegate if there aren't any services
                                    localMethods.AppendLine($"public async Task<I{className}> {methodName}({parameterDeclarationsNoServicesText})"); // Now a publicMethod...
                                }


                                localMethods.AppendLine("{");
                                localMethods.AppendLine($"var target = ServiceProvider.GetRequiredService<{className}>();");
                                localMethods.AppendLine($"{serviceAssignmentsText.ToString()}");

                                if (methodDeclaration.ReturnType.ToString() == "Task")
                                {
                                    localMethods.AppendLine($"await DoMapperMethodCall(target, DataMapperMethod.{attributeName}, () => target.{methodName}({parameterIdentifiersText}));");
                                }
                                else
                                {
                                    localMethods.AppendLine($"await DoMapperMethodCall(target, DataMapperMethod.{attributeName}, () => {{ target.{methodName}({parameterIdentifiersText});");
                                    localMethods.AppendLine("return Task.CompletedTask;");
                                    localMethods.AppendLine("});");
                                }

                                localMethods.AppendLine($" return target;");
                                localMethods.AppendLine("}");
                                localMethods.AppendLine("");
                            }
                            else
                            {
                                localMethods.AppendLine($"protected async Task Local{uniqueMethodName}(I{className} itarget, {parameterDeclarationsNoServicesText})");
                                localMethods.AppendLine("{");
                                localMethods.AppendLine($"var target = ({className})itarget ?? throw new Exception(\"{className} must implement I{className}\");");
                                localMethods.AppendLine($"{serviceAssignmentsText.ToString()}");
                                if (methodDeclaration.ReturnType.ToString() == "Task")
                                {
                                    localMethods.AppendLine($"await DoMapperMethodCall(target, DataMapperMethod.{attributeName}, () => target.{methodName}({parameterIdentifiersText}));");
                                }
                                else
                                {
                                    localMethods.AppendLine($"await DoMapperMethodCall(target, DataMapperMethod.{attributeName}, () => {{ target.{methodName}({parameterIdentifiersText});");
                                    localMethods.AppendLine("return Task.CompletedTask;");
                                    localMethods.AppendLine("});");
                                }

                                localMethods.AppendLine("}");
                                localMethods.AppendLine("");
                            }

                            if (!isSaveMethod && doRemoteCall)
                            {
                                delegateAssignmentRemote.AppendLine($"{uniqueMethodName}Property = Remote{uniqueMethodName};");

                                // Remote method implementations
                                remoteMethods.AppendLine($"protected async Task<I{className}?> Remote{uniqueMethodName}({parameterDeclarationsNoServicesText})");
                                remoteMethods.AppendLine("{");
                                remoteMethods.AppendLine($" return (I{className}?) await DoRemoteRequest(typeof({delegateName}), [{parameterIdentifiersNoServicesText}]);");
                                remoteMethods.AppendLine("}");
                                remoteMethods.AppendLine("");
                            }


                            if (isSaveMethod)
                            {
                                (string parameterIdentifiersText, string parameterDeclarationText, string saveMethodName, string saveUniqueMethodName,
                                        string? insertMethodName, string? updateMethodName, string? deleteMethodName) saveMethodSet = (parameterIdentifiersNoServicesText, parameterDeclarationsNoServicesText,
                                        $"Save{methodName.Replace("Insert", "").Replace("Update", "")}",
                                        $"Save{uniqueMethodName.Replace("Insert", "").Replace("Update", "")}", null, null, null);

                                if (!saveMethodSets.ContainsKey(parameterDeclarationsNoServicesText))
                                {
                                    saveMethodSets.Add(parameterDeclarationsNoServicesText, saveMethodSet);
                                }
                                else
                                {
                                    saveMethodSet = saveMethodSets[parameterDeclarationsNoServicesText];
                                }

                                if (attributeName == "Insert")
                                {
                                    saveMethodSet.insertMethodName = uniqueMethodName;
                                }
                                else if (attributeName == "Update")
                                {
                                    saveMethodSet.updateMethodName = uniqueMethodName;
                                }
                                else if (attributeName == "Delete")
                                {
                                    saveMethodSet.deleteMethodName = uniqueMethodName;
                                }

                                saveMethodSets[parameterDeclarationsNoServicesText] = saveMethodSet;
                            }
                        }
                    }
                }


                classDeclarationSyntax = GetBaseClassDeclarationSyntax(semanticModel, classDeclarationSyntax, messages);
            }

            foreach (var saveMethodSet in saveMethodSets.Select(s => s.Value))
            {
                var isDefault = string.IsNullOrWhiteSpace(saveMethodSet.parameterIdentifiersText);

                delegates.AppendLine($"protected internal delegate Task<I{className}?> {saveMethodSet.saveUniqueMethodName}Delegate(I{className} target, {saveMethodSet.parameterDeclarationText});");
                propertyDeclarations.AppendLine($"protected {saveMethodSet.saveUniqueMethodName}Delegate {saveMethodSet.saveUniqueMethodName}Property {{ get; set; }}");
                delegateAssignmentLocal.AppendLine($"{saveMethodSet.saveUniqueMethodName}Property = Local{saveMethodSet.saveUniqueMethodName};");
                delegateAssignmentRemote.AppendLine($"{saveMethodSet.saveUniqueMethodName}Property = Remote{saveMethodSet.saveUniqueMethodName};");


                interfaceMethods.AppendLine($"Task<I{className}?> {saveMethodSet.saveMethodName}(I{className} target, {saveMethodSet.parameterDeclarationText});");

                if (isDefault)
                {
                    publicMethods.AppendLine($"public override async Task<IEditBase?> Save({className} target)");
                    publicMethods.AppendLine("{");
                    publicMethods.AppendLine($"return await {saveMethodSet.saveUniqueMethodName}Property(target);");
                    publicMethods.AppendLine("}");
                }

                publicMethods.AppendLine($"public Task<I{className}?> {saveMethodSet.saveMethodName}(I{className} target, {saveMethodSet.parameterDeclarationText})");
                publicMethods.AppendLine("{");
                publicMethods.AppendLine($"return {saveMethodSet.saveUniqueMethodName}Property(target, {saveMethodSet.parameterIdentifiersText});");
                publicMethods.AppendLine("}");

                saveMethods.AppendLine($@"[Local<{saveMethodSet.saveUniqueMethodName}Delegate>]");
                saveMethods.AppendLine($@"
protected async Task<I{className}?> Local{saveMethodSet.saveUniqueMethodName}(I{className} target, {saveMethodSet.parameterDeclarationText})
{{");

                saveMethods.AppendLine($@"

                if (target.IsDeleted)
        {{
            if (target.IsNew)
            {{
                return null;
            }}
            {(saveMethodSet.deleteMethodName != null ? $"await Local{saveMethodSet.deleteMethodName}(target, {saveMethodSet.parameterIdentifiersText});" : $"throw new NotImplementedException(\"{className}Factory.Update()\");")}
        }}
        else if (target.IsNew)
        {{
            {(saveMethodSet.insertMethodName != null ? $"await Local{saveMethodSet.insertMethodName}(target, {saveMethodSet.parameterIdentifiersText});" : $"throw new NotImplementedException(\"{className}Factory.Update()\");")}
        }}
        else
        {{
            {(saveMethodSet.updateMethodName != null ? $"await Local{saveMethodSet.updateMethodName}(target, {saveMethodSet.parameterIdentifiersText});" : $"throw new NotImplementedException(\"{className}Factory.Update()\");")}
        }}
        return target;
");

                saveMethods.AppendLine("}");

                saveMethods.AppendLine($@"protected async Task<I{className}?> Remote{saveMethodSet.saveUniqueMethodName}(I{className} target, {saveMethodSet.parameterDeclarationText}){{");
                saveMethods.AppendLine($@"return (I{className}?) await DoRemoteRequest(typeof({saveMethodSet.saveUniqueMethodName}Delegate), [target, {saveMethodSet.parameterIdentifiersText}]);");
                saveMethods.AppendLine("}");
            }

            //if(saveMethodSets.Any() && !saveMethodSets.Any(saveMethodSet => string.IsNullOrWhiteSpace(saveMethodSet.Value.parameterIdentifiersText)))
            //{
            //    publicMethods.AppendLine($"Task<IEditBase?> ISave({className} target)");
            //    publicMethods.AppendLine("{");
            //    publicMethods.AppendLine($"throw new NotImplementedException(\"No default parameterless save available\");");
            //    publicMethods.AppendLine("}");
            //}

            


            var editText = isEdit ? "Edit" : "";

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

    public interface I{className}Factory
    {{
{interfaceMethods.ToString()}
    }}
    
    [Factory<I{className}>]
    internal class {className}Factory : Factory{editText}Base<{className}>, I{className}Factory
    {{
    
        private readonly IServiceProvider ServiceProvider;  
        private readonly DoRemoteRequest DoRemoteRequest;

{delegates.ToString()}

{propertyDeclarations.ToString()}

        public {className}Factory(IServiceProvider serviceProvider)
        {{
                this.ServiceProvider = serviceProvider;
                {delegateAssignmentLocal.ToString()}
        }}

        public {className}Factory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {{
                this.ServiceProvider = serviceProvider;
                this.DoRemoteRequest = remoteMethodDelegate;
                {delegateAssignmentRemote.ToString()}
        }}

{publicMethods.ToString()}
{localMethods.ToString()}
{remoteMethods.ToString()}
{saveMethods.ToString()}
    }}
}}";

            source = source.Replace(", )", ")");
            source = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().SyntaxTree.GetText().ToString();
            context.AddSource($"{namespaceName}.{className}Factory.g.cs", source);
        }

        private static ClassDeclarationSyntax? GetBaseClassDeclarationSyntax(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, List<string> messages)
        {
            try
            {

                var correctSemanticModel = semanticModel.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);

                var classSymbol = correctSemanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

                if (classSymbol?.BaseType == null)
                {
                    return null;
                }

                var baseTypeSymbol = classSymbol.BaseType;
                var baseTypeSyntaxReference = baseTypeSymbol.DeclaringSyntaxReferences.FirstOrDefault();

                if (baseTypeSyntaxReference == null)
                {
                    return null;
                }

                var baseTypeSyntaxNode = baseTypeSyntaxReference.GetSyntax() as ClassDeclarationSyntax;


                return baseTypeSyntaxNode;
            }
            catch (Exception ex)
            {
                messages.Add(ex.Message);
                return null;
            }
        }
    }
}