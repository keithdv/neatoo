﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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

        public static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        {
            return node is ClassDeclarationSyntax classDeclarationSyntax
                    && !(classDeclarationSyntax.TypeParameterList?.Parameters.Any() ?? false || classDeclarationSyntax.Modifiers.Any(SyntaxKind.AbstractKeyword))
                    && !(classDeclarationSyntax.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "SuppressFactory"));
        }

        public static (ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            var classNamedTypeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);

            if (!classNamedTypeSymbol.Interfaces.Any(i => i.Name == $"I{classNamedTypeSymbol.Name}"))
            {
                return null;
            }

            if (classNamedTypeSymbol == null)
            {
                return null;
            }

            if (classNamedTypeSymbol.GetAttributes().Any(a => (a.AttributeClass?.Name.ToString() ?? "") == "SuppressFactory"))
            {
                return null;
            }

            if (ClassOrBaseClassHasAttribute(classNamedTypeSymbol, "FactoryAttribute"))
            {
                return (classDeclaration, context.SemanticModel);
            }

            return null;
        }

        private static bool ClassOrBaseClassHasAttribute(INamedTypeSymbol namedTypeSymbol, string attributeName)
        {
            if (namedTypeSymbol.GetAttributes().Any(a => (a.AttributeClass?.Name ?? "") == attributeName))
            {
                return true;
            }
            if (namedTypeSymbol.BaseType != null)
            {
                return ClassOrBaseClassHasAttribute(namedTypeSymbol.BaseType, attributeName);
            }
            return false;
        }

        private static List<string> dataMapperAttributes = new() { "Create", "Fetch", "Insert", "Update", "Delete" };
        private static List<string> dataMapperSaveAttributes = new() { "Insert", "Update", "Delete" };

        private static void Execute(SourceProductionContext context, ClassDeclarationSyntax classDeclarationSyntax, SemanticModel semanticModel)
        {
            var usingDirectives = new List<string>() { "using Neatoo;", "using Neatoo.Portal;" };
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
            var serviceRegistrations = new StringBuilder();

            Dictionary<string, (string parameterIdentifiersText, string parameterDeclarationText, string saveMethodName, string saveUniqueMethodName,
                                    string? insertMethod, string? updateMethod, string? deleteMethod)> saveMethodSets = new();
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

                            if (methodDeclaration.AttributeLists
                                                    .SelectMany(a => a.Attributes)
                                                    .Any(a => a.Name.ToString() == "Remote") // Has [Remote] attribute - always remote
                                   || (parameters.SelectMany(p => p.AttributeLists) // Any parameter has [Service] attribute and return type is not void. So assume void is usually local
                                                .SelectMany(a => a.Attributes)
                                                .Any(a => a.Name.ToString() == "Service")
                                       && methodDeclaration.ReturnType.ToString() != "void"))
                            {
                                doRemoteCall = true;
                            }

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
                            var delegateName = $"I{className}Factory.{uniqueMethodName}Delegate";
                            var methodReturn = $"I{className}";
                            var saveMethodReturn = $"I{className}?";
                            var doMapperMethodName = $"DoMapperMethodCall<I{className}>";

                            if (methodDeclaration.ReturnType.ToString() == "Task<bool>" ||
                                (doRemoteCall && methodDeclaration.ReturnType.ToString() == "bool"))
                            {
                                methodReturn = $"Task<I{className}?>";
                                doMapperMethodName = $"DoMapperMethodCallBoolAsync<I{className}?>";
                            }
                            else if (methodDeclaration.ReturnType.ToString() == "bool")
                            {
                                methodReturn = $"I{className}?";
                                doMapperMethodName = $"DoMapperMethodCallBool<I{className}?>";
                            }
                            else if (methodDeclaration.ReturnType.ToString() == "Task" || doRemoteCall)
                            {
                                methodReturn = $"Task<I{className}>";
                                saveMethodReturn = $"Task<I{className}?>";
                                doMapperMethodName = $"DoMapperMethodCallAsync<I{className}>";
                            }

                            // Consumer Delegates

                            // Local method implementations
                            if (!isSaveMethod)
                            {
                                interfaceMethods.AppendLine($"{methodReturn} {methodName}({parameterDeclarationsNoServicesText});");
                                delegates.AppendLine($"delegate {methodReturn} {uniqueMethodName}Delegate({parameterDeclarationsNoServicesText});");

                                if (doRemoteCall)
                                {
                                    propertyDeclarations.AppendLine($"public {delegateName} {uniqueMethodName}Property {{ get; }}");
                                    delegateAssignmentLocal.AppendLine($"{uniqueMethodName}Property = Local{uniqueMethodName};");


                                    publicMethods.AppendLine($"public virtual {methodReturn} {methodName}({parameterDeclarationsNoServicesText})");
                                    publicMethods.AppendLine("{");
                                    publicMethods.AppendLine($"return {uniqueMethodName}Property({parameterIdentifiersNoServicesText});");
                                    publicMethods.AppendLine("}");

                                    localMethods.AppendLine($"public {methodReturn} Local{uniqueMethodName}({parameterDeclarationsNoServicesText})");

                                    serviceRegistrations.AppendLine($@"services.AddScoped<{delegateName}>(cc => {{
                                        var factory = cc.GetRequiredService<{className}Factory>();
                                        return ({parameterDeclarationsNoServicesText}) => factory.Local{uniqueMethodName}({parameterIdentifiersNoServicesText});
                                    }});");
                                }
                                else
                                {
                                    // Assume we can just call local if there are no services
                                    // Don't need the delegate if there aren't any services
                                    localMethods.AppendLine($"public {methodReturn} {methodName}({parameterDeclarationsNoServicesText})"); // Now a publicMethod...

                                    serviceRegistrations.AppendLine($@"services.AddScoped<{delegateName}>(cc => {{
                                        var factory = cc.GetRequiredService<{className}Factory>();
                                        return ({parameterDeclarationsNoServicesText}) => factory.{methodName}({parameterIdentifiersNoServicesText});
                                    }});");
                                }


                                localMethods.AppendLine("{");
                                localMethods.AppendLine($"var target = ServiceProvider.GetRequiredService<{className}>();");
                                localMethods.AppendLine($"{serviceAssignmentsText.ToString()}");

                                localMethods.AppendLine($"return {doMapperMethodName}(target, DataMapperMethod.{attributeName}, () => target.{methodName}({parameterIdentifiersText}));");

                                localMethods.AppendLine("}");
                                localMethods.AppendLine("");
                            }
                            else
                            {
                                localMethods.AppendLine($"public virtual {saveMethodReturn} Local{uniqueMethodName}(I{className} itarget, {parameterDeclarationsNoServicesText})");
                                localMethods.AppendLine("{");
                                localMethods.AppendLine($"var target = ({className})itarget ?? throw new Exception(\"{className} must implement I{className}\");");
                                localMethods.AppendLine($"{serviceAssignmentsText.ToString()}");
                                localMethods.AppendLine($"return {doMapperMethodName}(target, DataMapperMethod.{attributeName}, () => target.{methodName}({parameterIdentifiersText}));");

                                localMethods.AppendLine("}");
                                localMethods.AppendLine("");
                            }

                            if (!isSaveMethod && doRemoteCall)
                            {
                                delegateAssignmentRemote.AppendLine($"{uniqueMethodName}Property = Remote{uniqueMethodName};");

                                // Remote method implementations
                                remoteMethods.AppendLine($"public virtual async Task<I{className}?> Remote{uniqueMethodName}({parameterDeclarationsNoServicesText})");
                                remoteMethods.AppendLine("{");
                                remoteMethods.AppendLine($" return await DoRemoteRequest.ForDelegate<{className}?>(typeof({delegateName}), [{parameterIdentifiersNoServicesText}]);");
                                remoteMethods.AppendLine("}");
                                remoteMethods.AppendLine("");
                            }


                            if (isSaveMethod)
                            {
                                (string parameterIdentifiersText, string parameterDeclarationText, string saveMethodName, string saveUniqueMethodName,
                                        string? insertMethod, string? updateMethod, string? deleteMethod) saveMethodSet = (parameterIdentifiersNoServicesText, parameterDeclarationsNoServicesText,
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

                                var methodCall = $"{(saveMethodReturn.Contains("Task") ? "await" : "")} Local{uniqueMethodName}";

                                if (attributeName == "Insert")
                                {
                                    saveMethodSet.insertMethod = methodCall;
                                }
                                else if (attributeName == "Update")
                                {
                                    saveMethodSet.updateMethod = methodCall;
                                }
                                else if (attributeName == "Delete")
                                {
                                    saveMethodSet.deleteMethod = methodCall;
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

                var delegateName = $"I{className}Factory.{saveMethodSet.saveUniqueMethodName}Delegate";
                var saveMethodReturn = $"I{className}?";
                var isAsync = false;

                if ((saveMethodSet.deleteMethod?.Contains("await") ?? false)
                        || (saveMethodSet.insertMethod?.Contains("await") ?? false)
                        || (saveMethodSet.updateMethod?.Contains("await") ?? false))
                {
                    isAsync = true;
                    saveMethodReturn = $"Task<I{className}?>";
                    delegateAssignmentRemote.AppendLine($"{saveMethodSet.saveUniqueMethodName}Property = Remote{saveMethodSet.saveUniqueMethodName};");
                }

                delegates.AppendLine($"delegate {saveMethodReturn}  {saveMethodSet.saveUniqueMethodName}Delegate(I{className} target, {saveMethodSet.parameterDeclarationText});");
                propertyDeclarations.AppendLine($"public {delegateName} {saveMethodSet.saveUniqueMethodName}Property {{ get; set; }}");
                delegateAssignmentLocal.AppendLine($"{saveMethodSet.saveUniqueMethodName}Property = Local{saveMethodSet.saveUniqueMethodName};");

                interfaceMethods.AppendLine($"{saveMethodReturn.Trim()} {saveMethodSet.saveMethodName}(I{className} target, {saveMethodSet.parameterDeclarationText});");

                if (isDefault)
                {
                    if (isAsync)
                    {
                        publicMethods.AppendLine($"public override async Task<IEditBase?> Save({className} target)");
                        publicMethods.AppendLine("{");
                        publicMethods.AppendLine($"return (IEditBase?) (await {saveMethodSet.saveUniqueMethodName}Property(target));");
                        publicMethods.AppendLine("}");
                    }
                    else
                    {
                        publicMethods.AppendLine($"public override Task<IEditBase?> Save({className} target)");
                        publicMethods.AppendLine("{");
                        publicMethods.AppendLine($"return Task.FromResult<IEditBase?>({saveMethodSet.saveUniqueMethodName}Property(target));");
                        publicMethods.AppendLine("}");
                    }
                }

                publicMethods.AppendLine($"public {saveMethodReturn} {saveMethodSet.saveMethodName}(I{className} target, {saveMethodSet.parameterDeclarationText})");
                publicMethods.AppendLine("{");
                publicMethods.AppendLine($"return {saveMethodSet.saveUniqueMethodName}Property(target, {saveMethodSet.parameterIdentifiersText});");
                publicMethods.AppendLine("}");

                saveMethods.AppendLine($@"
public virtual {(isAsync ? "async" : "")} {saveMethodReturn} Local{saveMethodSet.saveUniqueMethodName}(I{className} target, {saveMethodSet.parameterDeclarationText})
{{");

                saveMethods.AppendLine($@"

                if (target.IsDeleted)
        {{
            if (target.IsNew)
            {{
                return null;
            }}
            {(saveMethodSet.deleteMethod != null ? $"return {saveMethodSet.deleteMethod}(target, {saveMethodSet.parameterIdentifiersText});" : $"throw new NotImplementedException(\"{className}Factory.Update()\");")}
        }}
        else if (target.IsNew)
        {{
            {(saveMethodSet.insertMethod != null ? $"return {saveMethodSet.insertMethod}(target, {saveMethodSet.parameterIdentifiersText});" : $"throw new NotImplementedException(\"{className}Factory.Update()\");")}
        }}
        else
        {{
            {(saveMethodSet.updateMethod != null ? $"return {saveMethodSet.updateMethod}(target, {saveMethodSet.parameterIdentifiersText});" : $"throw new NotImplementedException(\"{className}Factory.Update()\");")}
        }}
");



                saveMethods.AppendLine("}");

                if (isAsync)
                {
                    saveMethods.AppendLine($@"public async Task<I{className}?> Remote{saveMethodSet.saveUniqueMethodName}(I{className} target, {saveMethodSet.parameterDeclarationText}){{");
                    saveMethods.AppendLine($@"return await DoRemoteRequest.ForDelegate<{className}?>(typeof({delegateName}), [target, {saveMethodSet.parameterIdentifiersText}]);");
                    saveMethods.AppendLine("}");

                    serviceRegistrations.AppendLine($@"services.AddScoped<{delegateName}>(cc => {{
                                        var factory = cc.GetRequiredService<{className}Factory>();
                                        return (target, {saveMethodSet.parameterIdentifiersText}) => factory.Local{saveMethodSet.saveUniqueMethodName}(target, {saveMethodSet.parameterIdentifiersText});
                                    }});");
                }
            }

            //if(saveMethodSets.Any() && !saveMethodSets.Any(saveMethodSet => string.IsNullOrWhiteSpace(saveMethodSet.Value.parameterIdentifiersText)))
            //{
            //    publicMethods.AppendLine($"Task<IEditBase?> ISave({className} target)");
            //    publicMethods.AppendLine("{");
            //    publicMethods.AppendLine($"throw new NotImplementedException(\"No default parameterless save available\");");
            //    publicMethods.AppendLine("}");
            //}

            if (isEdit)
            {
                serviceRegistrations.AppendLine($@"services.AddScoped<IFactoryEditBase<{className}>, {className}Factory>();");
            }


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
{delegates.ToString()}
    }}
    
    internal class {className}Factory : Factory{editText}Base{(isEdit ? $"<{className}>" : "")}, I{className}Factory
    {{
    
        private readonly IServiceProvider ServiceProvider;  
        private readonly IDoRemoteRequest DoRemoteRequest;


{propertyDeclarations.ToString()}

        public {className}Factory(IServiceProvider serviceProvider)
        {{
                this.ServiceProvider = serviceProvider;
                {delegateAssignmentLocal.ToString()}
        }}

        public {className}Factory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {{
                this.ServiceProvider = serviceProvider;
                this.DoRemoteRequest = remoteMethodDelegate;
                {delegateAssignmentRemote.ToString()}
        }}

{publicMethods.ToString()}
{localMethods.ToString()}
{remoteMethods.ToString()}
{saveMethods.ToString()}

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {{
            services.AddTransient<{className}>();
            services.AddTransient<I{className}, {className}>();
            services.AddScoped<{className}Factory>();
            services.AddScoped<I{className}Factory, {className}Factory>();
{serviceRegistrations.ToString()}
        }}

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