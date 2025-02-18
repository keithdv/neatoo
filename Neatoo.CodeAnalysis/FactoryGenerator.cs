using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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
                static (ctx, source) => Execute(ctx, source!));
        }

        private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        {
            // We are looking for methods with attributes
            return node is ClassDeclarationSyntax classDeclarationSyntax &&
                    !(classDeclarationSyntax.TypeParameterList?.Parameters.Any() ?? false) &&
                   classDeclarationSyntax.Members.OfType<MethodDeclarationSyntax>().Any(m => m.AttributeLists.Any());
        }

        private static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            foreach (var methodDeclaration in classDeclaration.Members.OfType<MethodDeclarationSyntax>())
            {
                // Check if the method has any attributes named "Create"
                foreach (var attributeList in methodDeclaration.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        if (dataMapperAttributes.Contains(attribute.Name.ToString()))
                        {
                            return classDeclaration;
                        }
                    }
                }
            }

            return null;
        }

        private static List<string> dataMapperAttributes = ["Create", "Fetch", "Insert", "Update", "Delete"];
        private static List<string> dataMapperSaveAttributes = ["Insert", "Update", "Delete"];

        private static void Execute(SourceProductionContext context, ClassDeclarationSyntax classDeclarationSyntax)
        {
            var compilationUnitSyntax = classDeclarationSyntax.SyntaxTree.GetCompilationUnitRoot();
            var usingDirectives = new StringBuilder();

            var methodNames = new List<string>();


            var delegates = new StringBuilder();
            var delegateAssignmentLocal = new StringBuilder();
            var delegateAssignmentRemote = new StringBuilder();

            var propertyDeclarations = new StringBuilder();
            var localMethods = new StringBuilder();
            var remoteMethods = new StringBuilder();
            var saveMethods = new StringBuilder();
            Dictionary<string, (string parameterIdentifiersText, string parameterDeclarationText, string saveMethodName, string? insertMethodName, string? updateMethodName, string? deleteMethodName)> saveMethodSets = new();

            foreach (var using_ in compilationUnitSyntax.Usings)
            {
                usingDirectives.AppendLine(using_.ToString());
            }


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

            
            foreach (var methodDeclaration in classDeclarationSyntax.Members.OfType<MethodDeclarationSyntax>())
            {
                foreach (var attribute in methodDeclaration.AttributeLists.SelectMany(a => a.Attributes))
                {
                    if (dataMapperAttributes.Contains(attribute.Name.ToString()))
                    {
                        var attributeName = attribute.Name.ToString();
                        var uniqueMethodName = methodDeclaration.Identifier.Text;
                        var methodName = methodDeclaration.Identifier.Text;
                        var saveMethodName = string.Empty;
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

                        if (isSaveMethod)
                        {
                            saveMethodName = $"Save{uniqueMethodName.Replace("Insert", "").Replace("Update", "")}";
                        }

                        var parameters = methodDeclaration.ParameterList.Parameters;
                        var parameterIdentifiers = parameters.Select(p => p.Identifier.ToString()).ToList();
                        var serviceAssignmentsText = new StringBuilder();
                        var parameterIdentifiersNoServices = new List<string>();
                        var parameterDeclarationsNoServices = new List<string>();

                        foreach (var parameter in parameters)
                        {
                            if (parameter.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "Service"))
                            {
                                serviceAssignmentsText.AppendLine($"\t\t\tvar {parameter.Identifier} = ServiceProvider.GetService<{parameter.Type}>();");
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

                        var @public = "public";

                        if (isSaveMethod)
                        {
                            @public = "private";
                        }

                        //Consumer Delegates

                        propertyDeclarations.AppendLine($"{@public} {delegateName} {uniqueMethodName} {{ get; }}");
                        delegateAssignmentLocal.AppendLine($"{uniqueMethodName} = Local{uniqueMethodName};");

                        // Local method implementations
                        if (!isSaveMethod)
                        {
                            delegates.AppendLine($"{@public} delegate Task<I{className}> {delegateName}({parameterDeclarationsNoServicesText});"); localMethods.AppendLine($"[Local<{delegateName}>]");
                            localMethods.AppendLine($"protected async Task<I{className}> Local{uniqueMethodName}({parameterDeclarationsNoServicesText})");
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
                            delegates.AppendLine($"{@public} delegate Task {delegateName}({className} target, {parameterDeclarationsNoServicesText});");
                            localMethods.AppendLine($"protected async Task Local{uniqueMethodName}({className} target, {parameterDeclarationsNoServicesText})");
                            localMethods.AppendLine("{");
                            localMethods.AppendLine($"{serviceAssignmentsText.ToString()}");
                            localMethods.AppendLine($"await DoMapperMethodCall(target, DataMapperMethod.{attributeName}, () => target.{methodName}({parameterIdentifiersText}));");
                            localMethods.AppendLine("}");
                            localMethods.AppendLine("");
                        }

                        if (!isSaveMethod)
                        {
                            delegateAssignmentRemote.AppendLine($"{uniqueMethodName} = Remote{uniqueMethodName};");

                            // Remote method implementations
                            remoteMethods.AppendLine($"protected async Task<I{className}?> Remote{uniqueMethodName}({parameterDeclarationsNoServicesText})");
                            remoteMethods.AppendLine("{");
                            remoteMethods.AppendLine($" return (I{className}?) await RemoteRead(typeof({delegateName}), [{parameterIdentifiersNoServicesText}]);");
                            remoteMethods.AppendLine("}");
                            remoteMethods.AppendLine("");
                        }

                        if (isSaveMethod && methodDeclaration.ReturnType.ToString() == "Task")
                        {
                            (string parameterIdentifiersText, string parameterDeclarationText, string saveMethodName, string? insertMethodName, string? updateMethodName, string? deleteMethodName) saveMethodSet = (parameterIdentifiersNoServicesText, parameterDeclarationsNoServicesText, saveMethodName, null, null, null);

                            if (!saveMethodSets.ContainsKey(parameterDeclarationsNoServicesText))
                            {
                                saveMethodSets.Add(parameterDeclarationsNoServicesText, saveMethodSet);
                            } else
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

            foreach (var saveMethodSet in saveMethodSets.Select(s => s.Value))
            {
                delegates.AppendLine($"public delegate Task<I{className}?> {saveMethodSet.saveMethodName}Delegate(I{className} target, {saveMethodSet.parameterDeclarationText});");
                propertyDeclarations.AppendLine($"public {saveMethodSet.saveMethodName}Delegate {saveMethodSet.saveMethodName} {{ get; private set; }}");
                delegateAssignmentLocal.AppendLine($"{saveMethodSet.saveMethodName} = Local{saveMethodSet.saveMethodName};");
                delegateAssignmentRemote.AppendLine($"{saveMethodSet.saveMethodName} = Remote{saveMethodSet.saveMethodName};");

                saveMethods.AppendLine($@"[Local<{saveMethodSet.saveMethodName}Delegate>]");
                saveMethods.AppendLine($@"
protected async Task<I{className}?> Local{saveMethodSet.saveMethodName}(I{className} iTarget, {saveMethodSet.parameterDeclarationText})
{{");

                if (saveMethodSet.updateMethodName == null ||
                    saveMethodSet.insertMethodName == null ||
                    saveMethodSet.deleteMethodName == null)
                {
                    saveMethods.AppendLine($@"/* Missing method for {saveMethodSet.saveMethodName}");
                }
                saveMethods.AppendLine($@"
        var target = ({className})iTarget ?? throw new Exception(""{className} must implement I{className}"");

                if (target.IsDeleted)
        {{
            if (target.IsNew)
            {{
                return null;
            }}
            await {saveMethodSet.deleteMethodName ?? "MISSING"}(target, {saveMethodSet.parameterIdentifiersText}); 
        }}
        else if (target.IsNew)
        {{
            await {saveMethodSet.insertMethodName ?? "MISSING"}(target, {saveMethodSet.parameterIdentifiersText});
        }}
        else
        {{
            await {saveMethodSet.updateMethodName ?? "MISSING"}(target, {saveMethodSet.parameterIdentifiersText});
        }}
        return target;
");

                if (saveMethodSet.updateMethodName == null ||
    saveMethodSet.insertMethodName == null ||
    saveMethodSet.deleteMethodName == null)
                {
                    saveMethods.AppendLine($@"*/");
                    saveMethods.AppendLine($"throw new NotImplementedException();");
                    
                }
                saveMethods.AppendLine("}");

                saveMethods.AppendLine($@"protected async Task<I{className}?> Remote{saveMethodSet.saveMethodName}(I{className} target, {saveMethodSet.parameterDeclarationText}){{");
                saveMethods.AppendLine($@"return (I{className}?) await RemoteRead(typeof({saveMethodSet.saveMethodName}Delegate), [target, {saveMethodSet.parameterIdentifiersText}]);");
                saveMethods.AppendLine("}");
            }
            var source = $@"
using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
{usingDirectives.ToString()}

namespace {namespaceName}
{{

    public class {className}Factory : FactoryBase
    {{
    
        private readonly IServiceProvider ServiceProvider;  
        private readonly RemoteRead RemoteRead;

{delegates.ToString()}

{propertyDeclarations.ToString()}

        public {className}Factory(IServiceProvider serviceProvider)
        {{
                this.ServiceProvider = serviceProvider;
                {delegateAssignmentLocal.ToString()}
        }}

        public {className}Factory(IServiceProvider serviceProvider, RemoteRead remoteMethodDelegate)
        {{
                this.ServiceProvider = serviceProvider;
                this.RemoteRead = remoteMethodDelegate;
                {delegateAssignmentRemote.ToString()}
        }}

{localMethods.ToString()}

{remoteMethods.ToString()}
{saveMethods.ToString()}
    }}
}}";

            source = source.Replace(", )", ")");
            source = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().SyntaxTree.GetText().ToString();
            context.AddSource($"{namespaceName}.{className}Factory.g.cs", source);
        }
    }
}