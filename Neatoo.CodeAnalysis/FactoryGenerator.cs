using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Neatoo.Portal.DataMapper;

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

            if (classNamedTypeSymbol == null)
            {
                return null;
            }

            if (ClassOrBaseClassHasAttribute(classNamedTypeSymbol, "SuppressFactory") != null)
            {
                return null;
            }

            if (ClassOrBaseClassHasAttribute(classNamedTypeSymbol, "FactoryAttribute") != null)
            {
                return (classDeclaration, context.SemanticModel);
            }

            return null;
        }

        private static AttributeData? ClassOrBaseClassHasAttribute(INamedTypeSymbol namedTypeSymbol, string attributeName)
        {
            var attribute = namedTypeSymbol.GetAttributes().FirstOrDefault(a => (a.AttributeClass?.Name ?? "") == attributeName);

            if (attribute != null)
            {
                return attribute;
            }
            if (namedTypeSymbol.BaseType != null)
            {
                return ClassOrBaseClassHasAttribute(namedTypeSymbol.BaseType, attributeName);
            }
            return null;
        }

        private static List<string> dataMapperAttributes = Enum.GetNames(typeof(DataMapper.DataMapperMethod)).ToList();
        private static List<string> dataMapperSaveAttributes = Enum.GetValues(typeof(DataMapper.DataMapperMethod)).Cast<int>().Where(v => (v & (int)DataMapper.DataMapperMethodType.Write) != 0).Select(v => Enum.GetName(typeof(DataMapper.DataMapperMethod), v)).ToList();

        internal class MethodInfo
        {

            public MethodInfo(IEnumerable<MethodInfo> methodDeclarations)
            {
                var methodDeclaration = methodDeclarations.First();
                Name = methodDeclaration.Name;
                ClassName = methodDeclaration.ClassName;
                ClassType = methodDeclaration.ClassType;
                UniqueName = methodDeclaration.Name;
                Parameters = methodDeclaration.Parameters;
                IsSave = methodDeclaration.IsSave;
                IsSelfTask = methodDeclarations.Any(m => m.IsSelfTask);
                IsRemote = IsSelfRemote = methodDeclarations.Any(m => m.IsRemote);
                IsTask = IsSelfTask || IsRemote;
            }

            public MethodInfo(string classType, MethodDeclarationSyntax methodDeclaration)
            {
                Name = methodDeclaration.Identifier.Text;
                ClassType = classType;
                UniqueName = methodDeclaration.Identifier.Text;
                IsSelfBool = methodDeclaration.ReturnType.ToString().Contains("bool");
                IsSelfTask = methodDeclaration.ReturnType.ToString().Contains("Task");
                IsRemote = methodDeclaration.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "Remote");
                IsSelfRemote = IsRemote;
                IsSave = methodDeclaration.AttributeLists.SelectMany(a => a.Attributes).Any(a => dataMapperSaveAttributes.Contains(a.Name.ToString()));
                Attributes = methodDeclaration.AttributeLists.SelectMany(a => a.Attributes).Select(a => a.Name.ToString()).ToList();
                Parameters = methodDeclaration.ParameterList.Parameters.Select(p => new ParameterInfo()
                {
                    Name = p.Identifier.ToString(),
                    Type = p.Type.ToString(),
                    IsService = p.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "Service")
                }).ToList();

                if (IsSave)
                {
                    Parameters.Insert(0, new ParameterInfo() { Name = "target", Type = ClassType, IsService = false, IsTarget = true });
                    IsTask = IsSelfTask;
                } 
                else
                {
                    IsTask = IsSelfTask || IsRemote;
                }
            }

            public string ClassName { get; set; }
            public string Name { get; set; }
            public string UniqueName { get; set; }
            public int? UniqueNumber { get; set; }
            public string ClassType { get; }
            public bool IsSelfBool { get; set; }
            public List<ParameterInfo> Parameters { get; set; } = new List<ParameterInfo>();
            public bool IsAsync { get; set; }
            public string AsyncKeyword => IsAsync ? "async" : "";
            public string AwaitKeyword => IsTask ? "await" : "";
            public bool IsTask { get; set; }
            public bool IsSelfTask { get; set; }
            public bool IsRemote { get; set; }
            public bool IsSelfRemote { get; set; }
            public bool IsSave { get; }
            public bool HasAuth => AuthMethods.Count > 0;
            public List<string> Attributes { get; set; }
            public DataMapper.DataMapperMethod? DataMapperMethod { get; set; }
            public DataMapper.DataMapperMethodType? DataMapperMethodType { get; set; }
            public string FactoryMethodDelegateName => $"{UniqueName}Delegate";
            public string AuthResult => HasAuth ? $".Result" : "";

            public string ReturnType(bool includeTask = true, bool includeAuth = true, bool includeBool = true, bool authCheck = false)
            {
                var returnType = ClassType;

                if (AuthMethods.Count > 0 && authCheck)
                {
                    returnType = $"Authorized";
                }
                else if (AuthMethods.Count > 0 && includeAuth)
                {
                    returnType = $"Authorized<{returnType}>";
                }
                else if ((IsSave || IsSelfBool) && includeBool)
                {
                    returnType = $"{returnType}?";
                }

                if (includeTask && IsTask)
                {
                    returnType = $"Task<{returnType}>";
                }

                return returnType;
            }

            public string AttributeName => Attributes.FirstOrDefault(a => dataMapperAttributes.Contains(a)) ?? "";

            public string ParameterDeclarationsText(bool includeServices = false, bool includeAuth = false)
            {
                return string.Join(", ", Parameters.Where(p => (includeServices || !p.IsService) && (includeAuth || !p.IsAuth)).Select(p => $"{p.Type} {p.Name}"));
            }

            public string ParameterIdentifiersText(bool includeServices = false, bool includeAuth = false, string? checkAuthOnly = "false", bool includeTarget = true)
            {
                var result = string.Join(", ", Parameters.Where(p => (includeServices || !p.IsService) && (includeAuth || !p.IsAuth) && (includeTarget || !p.IsTarget)).Select(p => p.Name));

                if (HasAuth && !string.IsNullOrEmpty(checkAuthOnly))
                {
                    result += $", {checkAuthOnly}";
                }

                return result;
            }

            public string DoMapperMethodCall
            {
                get
                {
                    var methodCall = "DoMapperMethodCall";

                    if (IsSelfBool)
                    {
                        methodCall += "Bool";
                    }

                    if (IsSelfTask)
                    {
                        methodCall += "Async";
                    }

                    methodCall += $"<{ClassType}>";

                    methodCall = $"{methodCall}(target, DataMapperMethod.{AttributeName}, () => target.{Name} ({ParameterIdentifiersText(includeServices: true, includeTarget: false, checkAuthOnly: null)}))";


                    if (AuthMethods.Count > 0)
                    {
                        if (IsSelfTask)
                        {
                            methodCall = $"await {methodCall}";
                        }
                        methodCall = $"new Authorized<{ClassType}>({methodCall})";
                    } else if (!IsSelfTask && IsTask)
                    {
                        methodCall = $"Task.FromResult({methodCall})";
                    }

                    return methodCall;
                }
            }
            public string ParameterTypesNoServicesText => string.Join(", ", Parameters.Where(p => !p.IsService).Select(p => p.Type));
            public List<MethodInfo> AuthMethods { get; set; } = new List<MethodInfo>();
            public string ServiceAssignmentsText => string.Join("\n", Parameters.Where(p => p.IsService).Select(p => $"\t\t\tvar {p.Name} = ServiceProvider.GetService<{p.Type}>();"));
            public List<MethodInfo> SaveMethods { get; set; }
        }

        public class ParameterInfo
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public bool IsService { get; set; }
            public bool IsAuth { get; internal set; }
            public bool IsTarget { get; set; }
        }

        private class ClassText
        {
            public StringBuilder Delegates { get; set; } = new StringBuilder();
            public StringBuilder ConstructorPropertyAssignmentsLocal { get; set; } = new StringBuilder();
            public StringBuilder ConstructorPropertyAssignmentsRemote { get; set; } = new StringBuilder();
            public StringBuilder PropertyDeclarations { get; set; } = new StringBuilder();
            public StringBuilder MethodsBuilder { get; set; } = new StringBuilder();
            public StringBuilder SaveMethods { get; set; } = new StringBuilder();
            public StringBuilder InterfaceMethods { get; set; } = new StringBuilder();
            public StringBuilder ServiceRegistrations { get; set; } = new StringBuilder();
            public List<string> ConstructorParametersLocal { get; set; } = new List<string>();
            public List<string> ConstructorParametersRemote { get; set; } = new List<string>();
        }

        private static void Execute(SourceProductionContext context, ClassDeclarationSyntax classDeclarationSyntax, SemanticModel semanticModel)
        {
            var messages = new List<string>();
            string source;

            try
            {
                var classNamedSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax) ?? throw new Exception($"Cannot get named symbol for {classDeclarationSyntax.ToString()}");
                var usingDirectives = new List<string>() { "using Neatoo;", "using Neatoo.Portal;" };
                var methodNames = new List<string>();
                var className = classDeclarationSyntax.Identifier.Text;
                var returnType = $"{className}";
                var concreteType = $"{className}";
                var hasInterface = false;
                var classText = new ClassText();

                if (classNamedSymbol.Interfaces.Any(i => i.Name == $"I{classNamedSymbol.Name}"))
                {
                    returnType = $"I{classNamedSymbol.Name}";
                    classText.ServiceRegistrations.AppendLine($@"services.AddTransient<I{classNamedSymbol.Name}, {classNamedSymbol.Name}>();");
                    hasInterface = true;
                }


                // Generate the source code for the found method
                String namespaceName = FindNamespace(classDeclarationSyntax);

                try
                {

                    var parentClassDeclaration = classDeclarationSyntax.Parent as ClassDeclarationSyntax;
                    var parentClassUsingText = "";

                    while (parentClassDeclaration != null)
                    {
                        messages.Add("Parent class: " + parentClassDeclaration.Identifier.Text);
                        parentClassUsingText = $"{parentClassDeclaration.Identifier.Text}.{parentClassUsingText}";
                        parentClassDeclaration = parentClassDeclaration.Parent as ClassDeclarationSyntax;
                    }

                    if (!string.IsNullOrEmpty(parentClassUsingText))
                    {
                        usingDirectives.Add($"using static {namespaceName}.{parentClassUsingText.TrimEnd('.')};");
                    }

                    var authorizeAttribute = ClassOrBaseClassHasAttribute(classNamedSymbol, "AuthorizeAttribute");

                    List<MethodInfo> authClassMethods = [];

                    if (authorizeAttribute != null)
                    {
                        ITypeSymbol? authorizationRuleType = authorizeAttribute.AttributeClass?.TypeArguments[0];

                        TypeDeclarationSyntax? syntax = authorizationRuleType?.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as TypeDeclarationSyntax;

                        if (syntax != null)
                        {
                            messages.Add($"For {syntax.Identifier.Text} using {syntax.Identifier.Text}");

                            foreach (var method in syntax.Members.OfType<MethodDeclarationSyntax>())
                            {
                                // Ex [Authorize(DataMapperMethodType.Read | DataMapperMethodType.Write)]
                                var attr = method.AttributeLists.SelectMany(a => a.Attributes)
                                                .Where(a => a.Name.ToString() == "Authorize")
                                                .SingleOrDefault()?
                                                .ArgumentList?.Arguments.ToFullString();

                                string pattern = @"DataMapperMethodType\.(\w+)";

                                // Use Regex.Matches to find all matches in the attr string
                                var matches = Regex.Matches(attr, pattern);
                                var dataMapperMethodTypes = new List<DataMapperMethodType>();

                                foreach (Match match in matches)
                                {
                                    // Extract the matched value (e.g., "Read", "Write")
                                    string value = match.Groups[1].Value;

                                    // Try to parse the value into the DataMapperMethodType enum
                                    if (Enum.TryParse<DataMapperMethodType>(value, out var dmType))
                                    {
                                        // Successfully parsed the value into the DataMapperMethodType enum
                                        dataMapperMethodTypes.Add(dmType);
                                    }
                                }

                                if (dataMapperMethodTypes.Count > 0)
                                {
                                    var methodReturnType = method.ReturnType.ToString();
                                    if (!(methodReturnType == "string?" || methodReturnType == "Task<string?>" || methodReturnType == "Task<string>"
                                        || methodReturnType == "bool" || methodReturnType == "Task<bool>"))
                                    {
                                        messages.Add($"Auth: Invalid return type of {methodReturnType} for {method.Identifier.Text}. Must be string?, Task<string?>, Task<string>, bool or Task<bool>.");
                                        continue;
                                    }

                                    var dmType = dataMapperMethodTypes.Aggregate((a, b) => a | b);

                                    var methodInfo = new MethodInfo(returnType, method)
                                    {
                                        ClassName = syntax.Identifier.Text,
                                        DataMapperMethodType = dmType
                                    };

                                    authClassMethods.Add(methodInfo);
                                }
                                else
                                {
                                    messages.Add($"Auth: No DataMapperMethod for {method.Identifier.Text} in {string.Join(", ", method.AttributeLists.SelectMany(a => a.Attributes).Select(a => a.ToFullString()))}");
                                }
                            }
                        }
                        else
                        {
                            messages.Add($"No concrete classes for {authorizeAttribute}");
                        }


                    }

                    List<MethodInfo> classMethods = [];

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

                        messages.Add(item: classDeclarationSyntax.BaseList?.ToString());

                        foreach (var methodDeclaration in classDeclarationSyntax.Members.OfType<MethodDeclarationSyntax>())
                        {
                            var attribute = methodDeclaration.AttributeLists.SelectMany(a => a.Attributes).Where(a => dataMapperAttributes.Contains(a.Name.ToString())).FirstOrDefault();

                            if (attribute != null && (Enum.TryParse<DataMapper.DataMapperMethod>(attribute.Name.ToString(), out var dmm)))
                            {
                                var method = new MethodInfo(returnType, methodDeclaration)
                                {
                                    ClassName = className,
                                    DataMapperMethod = dmm
                                };

                                classMethods.Add(method);
                            }
                            else
                            {
                                messages.Add($"No DataMapperMethod attribute for {methodDeclaration.Identifier.Text}");
                            }
                        }
                        classDeclarationSyntax = GetBaseClassDeclarationSyntax(semanticModel, classDeclarationSyntax, messages);

                    }

                    foreach (var method in classMethods.OrderBy(m => m.Parameters.Count).ToList())
                    {
                        if (methodNames.Contains(method.Name))
                        {
                            var count = 1;
                            while (methodNames.Contains($"{method.UniqueName}{count}"))
                            {
                                count += 1;
                            }
                            method.UniqueName = $"{method.UniqueName}{count}";
                            method.UniqueNumber = count;
                        }
                        methodNames.Add(method.UniqueName);
                    }

                    foreach (var authMethods in authClassMethods.GroupBy(a => a.ClassName))
                    {
                        var authMethod = authMethods.First();
                        var constructorParameter = $"{authMethod.ClassName} {authMethod.ClassName.ToLower()}";
                        var propertyAssignment = $"this.{authMethod.ClassName} = {authMethod.ClassName.ToLower()};";

                        if (authMethods.Any(a => !a.IsRemote))
                        {
                            classText.ConstructorParametersRemote.Add(constructorParameter);
                            classText.ConstructorPropertyAssignmentsRemote.AppendLine(propertyAssignment);
                        }
                        classText.ConstructorParametersLocal.Add(constructorParameter);
                        classText.ConstructorPropertyAssignmentsLocal.AppendLine(propertyAssignment);

                        classText.PropertyDeclarations.AppendLine($"public {authMethod.ClassName} {authMethod.ClassName} {{ get; }}");
                    }

                    foreach (var method in classMethods)
                    {



                        foreach (var authMethod in authClassMethods)
                        {
                            var assignAuthMethod = false;

                            if (((int)authMethod.DataMapperMethodType & (int)method.DataMapperMethod) != 0)
                            {
                                if (authMethod.ParameterTypesNoServicesText == method.ParameterTypesNoServicesText
                                    || authMethod.Parameters.Count() == 0)
                                {
                                    assignAuthMethod = true;
                                }
                            }

                            if (((int)authMethod.DataMapperMethodType & (int)DataMapperMethodType.Write) != 0
                                && ((int)method.DataMapperMethod & (int)DataMapperMethodType.Write) != 0)
                            {
                                // Want to allow the write method to have the target as a parameter
                                assignAuthMethod = true;
                            }

                            if (assignAuthMethod)
                            {
                                if (!method.IsSave)
                                {
                                    method.IsTask = method.IsTask || authMethod.IsTask;
                                    method.IsRemote = method.IsRemote || authMethod.IsRemote;
                                    method.IsAsync = method.IsTask;
                                }
                                method.AuthMethods.Add(authMethod);
                            }
                        }

                        if (method.IsSave) { continue; }


                        if (method.HasAuth)
                        {
                            method.Name = $"{method.Name}";
                            method.Parameters.Add(new ParameterInfo() { Name = "checkAuthOnly", Type = "bool", IsAuth = true, IsService = false });
                        }
                        // Consumer Delegates

                        var methodBuilder = new StringBuilder();

                        PublicMethods(classText, method, methodBuilder);

                        RemoteMethod(classText, method, methodBuilder);

                        LocalMethod(method, methodBuilder);

                        classText.MethodsBuilder.Append(methodBuilder);
                    }

                    var saveMethodSets = classMethods.Where(m => m.IsSave).GroupBy(m => m.ParameterTypesNoServicesText)
                        .Select(m =>
                        new MethodInfo(m)
                        {
                            Name = $"Save",
                            SaveMethods = m.ToList(),
                            AuthMethods = m.SelectMany(m => m.AuthMethods).Distinct().ToList()
                        });

                    var isEdit = false;

                    foreach (var saveMethod in saveMethodSets)
                    {
                        isEdit = true;

                        saveMethod.UniqueName = saveMethod.Name;

                        if (methodNames.Contains(saveMethod.UniqueName))
                        {
                            var count = 1;
                            while (methodNames.Contains($"{saveMethod.UniqueName}{count}"))
                            {
                                count += 1;
                            }
                            saveMethod.UniqueName = $"{saveMethod.UniqueName}{count}";
                        }
                        methodNames.Add(saveMethod.UniqueName);

                        var isDefault = !saveMethod.Parameters.Where(p => !p.IsTarget && !p.IsAuth).Any();

                        foreach (var authMethod in saveMethod.AuthMethods.ToList())
                        {
                            var varText = authMethod.UniqueName.ToLower();

                            var saveMethodParameter = saveMethod.Parameters.Where(p => !p.IsTarget).GetEnumerator();
                            var authMethodParameter = authMethod.Parameters.GetEnumerator();
                            var parameters = new List<ParameterInfo>();

                            var matchFail = false;

                            saveMethodParameter.MoveNext();
                            authMethodParameter.MoveNext();

                            if (authMethodParameter.Current != null)
                            {
                                do
                                {
                                    if (authMethodParameter.Current.Type == returnType || authMethodParameter.Current.Type == $"I{returnType}")
                                    {
                                        parameters.Add(new ParameterInfo() { Name = "target", Type = returnType });
                                        authMethodParameter.MoveNext();
                                        continue;
                                    }
                                    else if (authMethodParameter.Current.Type != saveMethodParameter.Current?.Type)
                                    {
                                        matchFail = true;
                                        break;
                                    }
                                    else
                                    {
                                        parameters.Add(saveMethodParameter.Current);
                                    }
                                } while (authMethodParameter.MoveNext() && saveMethodParameter.MoveNext());
                            }
                            if (matchFail)
                            {
                                saveMethod.AuthMethods.Remove(authMethod);
                                continue;
                            }

                            authMethod.Parameters = parameters;
                        }

                        if (saveMethod.HasAuth)
                        {
                            saveMethod.Parameters.Add(new ParameterInfo() { Name = "checkAuthOnly", Type = "bool", IsAuth = true, IsService = false });
                        }

                        saveMethod.IsRemote = saveMethod.IsRemote || saveMethod.AuthMethods.Any(m => m.IsRemote);

                        saveMethod.IsSelfTask = saveMethod.IsSelfTask || saveMethod.AuthMethods.Any(m => m.IsSelfTask);
                        saveMethod.IsTask = saveMethod.IsTask || saveMethod.AuthMethods.Any(m => m.IsTask);
                        saveMethod.IsAsync = saveMethod.AuthMethods.Any() && (saveMethod.SaveMethods.Any(m => m.IsTask) || saveMethod.AuthMethods.Any(m => m.IsTask));

                        saveMethod.SaveMethods.ForEach(s => s.AuthMethods.Clear());

                        if (isDefault)
                        {
                            classText.SaveMethods.AppendLine($"async Task<IEditBase?> IFactoryEditBase<{className}>.Save({className} target)");
                            classText.SaveMethods.AppendLine("{");
                            if (saveMethod.IsRemote)
                            {
                                classText.SaveMethods.AppendLine($"return (IEditBase?) (await {saveMethod.UniqueName}Property(target));");
                            }
                            else if (saveMethod.IsTask)
                            {
                                classText.SaveMethods.AppendLine($"return (IEditBase?) await Save(target);");
                            }
                            else
                            {
                                classText.SaveMethods.AppendLine($"return await Task.FromResult((IEditBase?) Save(target));");
                            }
                            classText.SaveMethods.AppendLine("}");
                        }

                        PublicMethods(classText, saveMethod, classText.SaveMethods);
                        RemoteMethod(classText, saveMethod, classText.SaveMethods);


                        classText.SaveMethods.AppendLine($@"public virtual {saveMethod.AsyncKeyword} {saveMethod.ReturnType()} Local{saveMethod.UniqueName}({saveMethod.ParameterDeclarationsText(includeAuth: true)})
                            {{");

                        AuthMethods(saveMethod, classText.SaveMethods);

                        string DoInsertUpdateDeleteMethodCall(MethodInfo? method)
                        {
                            if (method == null)
                            {
                                return $"throw new NotImplementedException()";
                            }

                            var methodCall = $"Local{method.UniqueName}({saveMethod.ParameterIdentifiersText(checkAuthOnly: null)})";



                            if (saveMethod.HasAuth)
                            {
                                if (method.IsTask)
                                {
                                    methodCall = $"await {methodCall}";
                                }
                                methodCall = $"new Authorized<{returnType}>({methodCall})";
                            }

                            if (!method.IsTask && saveMethod.IsTask && !saveMethod.IsAsync)
                            {
                                methodCall = $"Task.FromResult({methodCall})";
                            }

                            return $"return {methodCall}";
                        }

                        classText.SaveMethods.AppendLine($@"

                            if (target.IsDeleted)
                    {{
                        if (target.IsNew)
                        {{
                            return null;
                        }}
                        {DoInsertUpdateDeleteMethodCall(saveMethod.SaveMethods.Where(s => s.AttributeName == "Delete").SingleOrDefault())};
                    }}
                    else if (target.IsNew)
                    {{
                        {DoInsertUpdateDeleteMethodCall(saveMethod.SaveMethods.Where(s => s.AttributeName == "Insert").SingleOrDefault())};
                    }}
                    else
                    {{
                         {DoInsertUpdateDeleteMethodCall(saveMethod.SaveMethods.Where(s => s.AttributeName == "Update").SingleOrDefault())};
                    }}
            ");

                        classText.SaveMethods.AppendLine("}");



                        void AddInsertDeleteUpdateLocalMethod(MethodInfo? method)
                        {
                            if (method == null)
                            {
                                return;
                            }
                            var doMapperMethodCall = method.DoMapperMethodCall;
                            classText.MethodsBuilder.AppendLine($"public virtual {method.ReturnType(includeAuth: false)} Local{method.UniqueName}({method.ParameterDeclarationsText()})");
                            classText.MethodsBuilder.AppendLine("{");
                            classText.MethodsBuilder.AppendLine($"var cTarget = ({concreteType}) target ?? throw new Exception(\"{className} must implement {returnType}\");");
                            classText.MethodsBuilder.AppendLine($"{method.ServiceAssignmentsText.ToString()}");
                            classText.MethodsBuilder.AppendLine($"return {method.DoMapperMethodCall.Replace("target", "cTarget")};");
                            classText.MethodsBuilder.AppendLine("}");
                            classText.MethodsBuilder.AppendLine("");
                        }
                        AddInsertDeleteUpdateLocalMethod(saveMethod.SaveMethods.Where(s => s.AttributeName == "Insert").SingleOrDefault());
                        AddInsertDeleteUpdateLocalMethod(saveMethod.SaveMethods.Where(s => s.AttributeName == "Update").SingleOrDefault());
                        AddInsertDeleteUpdateLocalMethod(saveMethod.SaveMethods.Where(s => s.AttributeName == "Delete").SingleOrDefault());
                    }


                    if (isEdit)
                    {
                        classText.ServiceRegistrations.AppendLine($@"services.AddScoped<IFactoryEditBase<{className}>, {className}Factory>();");
                    }

                    var editText = isEdit ? "Edit" : "";

                    source = $@"
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
{classText.InterfaceMethods.ToString()}
    }}
    
    internal class {className}Factory : Factory{editText}Base{(isEdit ? $"<{className}>, IFactoryEditBase<{className}>" : "")}, I{className}Factory
    {{
    
        private readonly IServiceProvider ServiceProvider;  
        private readonly IDoRemoteRequest DoRemoteRequest;

// Delegates
{classText.Delegates.ToString()}
// Delegate Properties to provide Local or Remote fork in execution
{classText.PropertyDeclarations.ToString()}

        public {className}Factory(IServiceProvider serviceProvider, {string.Join("\n,", classText.ConstructorParametersLocal)})
        {{
                this.ServiceProvider = serviceProvider;
                {classText.ConstructorPropertyAssignmentsLocal.ToString()}
        }}

        public {className}Factory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate, {string.Join("\n,", classText.ConstructorParametersLocal)})
        {{
                this.ServiceProvider = serviceProvider;
                this.DoRemoteRequest = remoteMethodDelegate;
                {classText.ConstructorPropertyAssignmentsRemote.ToString()}
        }}

{classText.MethodsBuilder.ToString()}
{classText.SaveMethods.ToString()}

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {{
            services.AddTransient<{className}>();
            services.AddScoped<{className}Factory>();
            services.AddScoped<I{className}Factory, {className}Factory>();
{classText.ServiceRegistrations.ToString()}
        }}

    }}
}}";
                    source = source.Replace("(, ", "(");
                    source = source.Replace(", )", ")");
                    source = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().SyntaxTree.GetText().ToString();
                } catch(Exception ex)
                {
                    source = $"/* Error: {ex.GetType().FullName} {ex.Message} */";
                }

                context.AddSource($"{namespaceName}.{className}Factory.g.cs", source);
            }
            catch (Exception ex)
            {
                source = $"// Error: {ex.Message}";
            }
                
        }

        private static void PublicMethods(ClassText classText, MethodInfo method, StringBuilder methodBuilder)
        {
            classText.InterfaceMethods.AppendLine($"{method.ReturnType(includeAuth: false)} {method.Name}({method.ParameterDeclarationsText()});");

            if (!method.HasAuth)
            {
                methodBuilder.AppendLine($"public virtual {method.ReturnType(includeAuth: false)} {method.Name}({method.ParameterDeclarationsText()})");
                methodBuilder.AppendLine("{");
                methodBuilder.AppendLine($"return Local{method.UniqueName}({method.ParameterIdentifiersText()});");
                methodBuilder.AppendLine("}");
            }
            else
            {
                methodBuilder.AppendLine($"public virtual {method.AsyncKeyword} {method.ReturnType(includeAuth: false)} {method.Name}({method.ParameterDeclarationsText()})");
                methodBuilder.AppendLine("{");

                if (method.IsSave)
                {
                    methodBuilder.AppendLine($"var authorized = ({method.AwaitKeyword} Local{method.UniqueName}({method.ParameterIdentifiersText()}));");

                    methodBuilder.AppendLine("if (!authorized.HasAccess)");
                    methodBuilder.AppendLine("{");
                    methodBuilder.AppendLine("throw new NotAuthorizedException(authorized.Message);");
                    methodBuilder.AppendLine("}");
                    methodBuilder.AppendLine("return authorized.Result;");
                } else
                {
                    methodBuilder.AppendLine($"return ({method.AwaitKeyword} Local{method.UniqueName}({method.ParameterIdentifiersText()})).Result;");
                }
                methodBuilder.AppendLine("}");

                classText.InterfaceMethods.AppendLine($"{method.ReturnType()} Try{method.Name}({method.ParameterDeclarationsText()});");

                // Try and Can methods
                methodBuilder.AppendLine($"public virtual {method.AsyncKeyword} {method.ReturnType()} Try{method.Name}({method.ParameterDeclarationsText()})");
                methodBuilder.AppendLine("{");
                methodBuilder.AppendLine($"return {method.AwaitKeyword} Local{method.UniqueName}({method.ParameterIdentifiersText(includeAuth: false)});");
                methodBuilder.AppendLine("}");

                classText.InterfaceMethods.AppendLine($"{method.ReturnType(authCheck: true)} Can{method.Name}({method.ParameterDeclarationsText()});");

                // Try and Can methods
                methodBuilder.AppendLine($"public virtual {method.AsyncKeyword} {method.ReturnType(authCheck: true)} Can{method.Name}({method.ParameterDeclarationsText()})");
                methodBuilder.AppendLine("{");
                methodBuilder.AppendLine($"return {method.AwaitKeyword} Local{method.UniqueName}({method.ParameterIdentifiersText(includeAuth: false, checkAuthOnly: "true")});");
                methodBuilder.AppendLine("}");
            }

        }

        private static void RemoteMethod(ClassText classText, MethodInfo method, StringBuilder methodBuilder)
        {
            if (method.IsRemote)
            {
                methodBuilder.Replace($"Local{method.UniqueName}", $"{method.UniqueName}Property");

                classText.Delegates.AppendLine($"public delegate {method.ReturnType()} {method.FactoryMethodDelegateName}({method.ParameterDeclarationsText(includeAuth: true)});");
                classText.PropertyDeclarations.AppendLine($"public {method.FactoryMethodDelegateName} {method.UniqueName}Property {{ get; }}");
                classText.ConstructorPropertyAssignmentsLocal.AppendLine($"{method.UniqueName}Property = Local{method.UniqueName};");
                classText.ConstructorPropertyAssignmentsRemote.AppendLine($"{method.UniqueName}Property = Remote{method.UniqueName};");

                methodBuilder.AppendLine($"public virtual async {method.ReturnType()} Remote{method.UniqueName}({method.ParameterDeclarationsText(includeAuth: true)})");
                methodBuilder.AppendLine("{");
                methodBuilder.AppendLine($" return await DoRemoteRequest.ForDelegate<{method.ReturnType(includeTask: false)}>(typeof({method.FactoryMethodDelegateName}), [{method.ParameterIdentifiersText(checkAuthOnly: "checkAuthOnly")}]);");
                methodBuilder.AppendLine("}");
                methodBuilder.AppendLine("");

                classText.ServiceRegistrations.AppendLine($@"services.AddScoped<{method.FactoryMethodDelegateName}>(cc => {{
                                        var factory = cc.GetRequiredService<{method.ClassName}Factory>();
                                        return ({method.ParameterDeclarationsText(includeAuth: true)}) => factory.Local{method.UniqueName}({method.ParameterIdentifiersText(checkAuthOnly: "checkAuthOnly")});
                                    }});");
            }
        }

        private static void LocalMethod(MethodInfo method, StringBuilder methodBuilder)
        {
            methodBuilder.AppendLine($"public {method.AsyncKeyword} {method.ReturnType()} Local{method.UniqueName}({method.ParameterDeclarationsText(includeAuth: true)})");
            methodBuilder.AppendLine("{");

            AuthMethods(method, methodBuilder);

            methodBuilder.AppendLine($"var target = ServiceProvider.GetRequiredService<{method.ClassName}>();");
            methodBuilder.AppendLine($"{method.ServiceAssignmentsText}");

            methodBuilder.AppendLine($"return {method.DoMapperMethodCall};");

            methodBuilder.AppendLine("}");
            methodBuilder.AppendLine("");
        }

        private static void AuthMethods(MethodInfo method, StringBuilder methodBuilder)
        {
            foreach (var authMethod in method.AuthMethods)
            {
                var varText = authMethod.UniqueName.ToLower();
                var callText = $"{authMethod.ClassName}.{authMethod.Name}({string.Join(", ", method.Parameters.Where(p => !p.IsTarget).Take(authMethod.Parameters.Count).Select(a => a.Name))})";
                if (authMethod.IsSelfTask)
                {
                    callText = $"await {callText}";
                }

                methodBuilder.AppendLine($"Authorized {varText} = {callText};");
                methodBuilder.AppendLine($"if (!{varText}.HasAccess)");

                methodBuilder.AppendLine("{");
                methodBuilder.AppendLine($"return new {method.ReturnType(includeTask: false)}({varText});");
                methodBuilder.AppendLine("}");
            }

            if (method.HasAuth)
            {
                methodBuilder.AppendLine($"if (checkAuthOnly)");
                methodBuilder.AppendLine("{");
                methodBuilder.AppendLine($"return new Authorized<{method.ClassType}>(true);");
                methodBuilder.AppendLine("}");
            }
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

        private static string FindNamespace(SyntaxNode syntaxNode)
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
                return string.Empty;
            }
        }
    }
}