using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
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
            public MethodInfo(MethodDeclarationSyntax methodDeclaration)
            {
                Name = methodDeclaration.Identifier.Text;
                UniqueName = methodDeclaration.Identifier.Text;
                IsBoolReturn = methodDeclaration.ReturnType.ToString().Contains("bool");
                IsAsync = methodDeclaration.ReturnType.ToString().Contains("Task");
                IsRemote = methodDeclaration.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "Remote");
                IsSave = methodDeclaration.AttributeLists.SelectMany(a => a.Attributes).Any(a => dataMapperSaveAttributes.Contains(a.Name.ToString()));
                Attributes = methodDeclaration.AttributeLists.SelectMany(a => a.Attributes).Select(a => a.Name.ToString()).ToList();
                Parameters = methodDeclaration.ParameterList.Parameters.Select(p => new ParameterInfo()
                {
                    Name = p.Identifier.ToString(),
                    Type = p.Type.ToString(),
                    IsService = p.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "Service")
                }).ToList();
            }

            public string ClassName { get; set; }
            public string ClassDeclarationType { get; set; }
            public string Name { get; set; }
            public string UniqueName { get; set; }
            public int? UniqueNumber { get; set; }
            public string ReturnType { get; set; }
            public bool IsBoolReturn { get; set; }
            public List<ParameterInfo> Parameters { get; set; } = new List<ParameterInfo>();
            public bool IsAsync { get; set; }
            public bool IsRemote { get; set; }
            public bool IsSave { get; set; }
            public List<string> Attributes { get; set; }
            public DataMapper.DataMapperMethod? DataMapperMethod { get; set; }
            public DataMapper.DataMapperMethodType? DataMapperMethodType { get; set; }
            public string DelegateName => $"{UniqueName}Delegate";
            public string MethodReturn
            {
                get
                {
                    var returnType = $"{ReturnType}";

                    if (IsBoolReturn)
                    {
                        returnType = $"{returnType}?";
                    }

                    if (IsAsync || IsRemote)
                    {
                        returnType = $"Task<{returnType}>";
                    }

                    return returnType;
                }
            }

            public string DeserializeDelegateTo { get; set; }

            public string SaveMethodReturn
            {
                get
                {
                    if (MethodReturn.Contains(">"))
                    {
                        return MethodReturn.Replace(">", "?>");
                    }
                    return $"{MethodReturn}?";
                }
            }

            public string AttributeName => Attributes.FirstOrDefault(a => dataMapperAttributes.Contains(a)) ?? "";
            public string ParameterIdentifiersNoServicesText => string.Join(", ", Parameters.Where(p => !p.IsService).Select(p => p.Name));
            public string ParameterDeclarationsNoServicesText => string.Join(", ", Parameters.Where(p => !p.IsService).Select(p => $"{p.Type} {p.Name}"));
            public string ParameterIdentifiersText => string.Join(", ", Parameters.Select(p => p.Name));
            public string DoMapperMethodName
            {
                get
                {

                    var methodName = "DoMapperMethodCall";

                    if (IsBoolReturn)
                    {
                        methodName += "Bool";
                    }
                    if (IsAsync || IsRemote)
                    {
                        methodName += "Async";
                    }

                    methodName += $"<{ReturnType}>";

                    return methodName;
                }
            }

            public string ParameterTypesNoServicesText => string.Join(", ", Parameters.Where(p => !p.IsService).Select(p => p.Type));
            public List<MethodInfo> AuthMethods { get; } = new List<MethodInfo>();
            public string ServiceAssignmentsText => string.Join("\n", Parameters.Where(p => p.IsService).Select(p => $"\t\t\tvar {p.Name} = ServiceProvider.GetService<{p.Type}>();"));
        }

        public class ParameterInfo
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public bool IsService { get; set; }
        }


        private static void Execute(SourceProductionContext context, ClassDeclarationSyntax classDeclarationSyntax, SemanticModel semanticModel)
        {
            var classNamedSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax) ?? throw new Exception($"Cannot get named symbol for {classDeclarationSyntax.ToString()}");
            var usingDirectives = new List<string>() { "using Neatoo;", "using Neatoo.Portal;" };
            var messages = new List<string>();
            var methodNames = new List<string>();
            var className = classDeclarationSyntax.Identifier.Text;
            var returnType = $"{className}";
            var hasInterface = false;

            var delegates = new StringBuilder();
            var constructorPropertyAssignmentsLocal = new StringBuilder();
            var constructorPropertyAssignmentsRemote = new StringBuilder();

            var propertyDeclarations = new StringBuilder();
            var publicMethods = new StringBuilder();
            var localMethods = new StringBuilder();
            var remoteMethods = new StringBuilder();
            var saveMethods = new StringBuilder();
            var interfaceMethods = new StringBuilder();
            var serviceRegistrations = new StringBuilder();
            var constructorParametersLocal = new List<string>();
            var constructorParametersRemote = new List<string>();

            if (classNamedSymbol.Interfaces.Any(i => i.Name == $"I{classNamedSymbol.Name}"))
            {
                returnType = $"I{classNamedSymbol.Name}";
                serviceRegistrations.AppendLine($@"services.AddTransient<I{classNamedSymbol.Name}, {classNamedSymbol.Name}>();");
                hasInterface = true;
            }

            // Generate the source code for the found method
            String namespaceName = "FAILURE";

            if (classDeclarationSyntax.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax)
            {
                namespaceName = namespaceDeclarationSyntax.Name.ToString();
            }
            else if (classDeclarationSyntax.Parent is FileScopedNamespaceDeclarationSyntax parentClassDeclarationSyntax)
            {
                namespaceName = parentClassDeclarationSyntax.Name.ToString();
            }

            var authorizeAttribute = ClassOrBaseClassHasAttribute(semanticModel.GetDeclaredSymbol(classDeclarationSyntax), "AuthorizeAttribute");


            List<MethodInfo> authClassMethods = new List<MethodInfo>();

            if (authorizeAttribute != null)
            {
                ITypeSymbol? authorizationRuleType = authorizeAttribute.AttributeClass?.TypeArguments[0];

                var syntax = authorizationRuleType?.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as InterfaceDeclarationSyntax;

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
                            var dmType = dataMapperMethodTypes.Aggregate((a, b) => a | b);

                            var methodInfo = new MethodInfo(method)
                            {
                                ClassName = syntax.Identifier.Text,
                                ClassDeclarationType = syntax?.Identifier.Text ?? syntax.Identifier.Text,
                                ReturnType = "IAuthorizationRuleResult",
                                DataMapperMethodType = dmType,
                                IsAsync = method.ReturnType.ToFullString().Contains("Task")
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
                    messages.Add($"No concrete classes for {authorizeAttribute.ToString()}");
                }
            }



            List<MethodInfo> classMethods = new List<MethodInfo>();

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

                foreach (var methodDeclaration in classDeclarationSyntax.Members.OfType<MethodDeclarationSyntax>())
                {
                    var attribute = methodDeclaration.AttributeLists.SelectMany(a => a.Attributes).Where(a => dataMapperAttributes.Contains(a.Name.ToString())).FirstOrDefault();

                    if (attribute != null && (Enum.TryParse<DataMapper.DataMapperMethod>(attribute.Name.ToString(), out var dmm)))
                    {
                        var method = new MethodInfo(methodDeclaration)
                        {
                            ClassName = className,
                            ReturnType = returnType,
                            DataMapperMethod = dmm,
                            DeserializeDelegateTo = className,
                            IsAsync = methodDeclaration.ReturnType.ToFullString().Contains("Task"),
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
                var constructorParameter = $"{authMethod.ClassDeclarationType} {authMethod.ClassName.ToLower()}";
                var propertyAssignment = $"this.{authMethod.ClassName} = {authMethod.ClassName.ToLower()};";

                if (authMethods.Any(a => !a.IsRemote))
                {
                    constructorParametersRemote.Add(constructorParameter);
                    constructorPropertyAssignmentsRemote.AppendLine(propertyAssignment);
                }
                constructorParametersLocal.Add(constructorParameter);
                constructorPropertyAssignmentsLocal.AppendLine(propertyAssignment);

                propertyDeclarations.AppendLine($"public {authMethod.ClassDeclarationType} {authMethod.ClassName} {{ get; }}");
            }

            //foreach (var authMethod in authClassMethods)
            //{
            //    var asyncReturn = authMethod.IsAsync ? "async Task<bool>" : "bool";
            //    localMethods.AppendLine($"public {asyncReturn} {authMethod.UniqueName}Authorized({authMethod.ParameterDeclarationsNoServicesText}, out string message)");
            //    localMethods.AppendLine("{");
            //    localMethods.AppendLine(authMethod.ServiceAssignmentsText.ToString());
            //    localMethods.AppendLine($"var authorized = {(authMethod.IsAsync ? "await" : "")} {authMethod.ClassName}.{authMethod.Name}({authMethod.ParameterIdentifiersText});");
            //    localMethods.AppendLine("message = authorized.Message;");
            //    localMethods.AppendLine("return authorized.HasAccess;");
            //    localMethods.AppendLine("}");
            //}

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
                            method.IsAsync = method.IsAsync || authMethod.IsAsync;
                            method.IsRemote = method.IsRemote || authMethod.IsRemote;
                        }
                        method.AuthMethods.Add(authMethod);
                    }
                }
                // Consumer Delegates

                var awaitText = method.IsAsync ? "await" : "";
                var asyncText = method.IsAsync ? "async" : "";

                // Local method implementations
                if (!method.IsSave)
                {
                    interfaceMethods.AppendLine($"{method.MethodReturn} {method.Name}({method.ParameterDeclarationsNoServicesText});");

                    if (method.IsRemote)
                    {
                        delegates.AppendLine($"public delegate {method.MethodReturn} {method.UniqueName}Delegate({method.ParameterDeclarationsNoServicesText});");
                        propertyDeclarations.AppendLine($"public {method.DelegateName} {method.UniqueName}Property {{ get; }}");
                        constructorPropertyAssignmentsLocal.AppendLine($"{method.UniqueName}Property = Local{method.UniqueName};");
                        constructorPropertyAssignmentsRemote.AppendLine($"{method.UniqueName}Property = Remote{method.UniqueName};");

                        publicMethods.AppendLine($"public virtual {method.MethodReturn} {method.Name}({method.ParameterDeclarationsNoServicesText})");
                        publicMethods.AppendLine("{");
                        publicMethods.AppendLine($"return {method.UniqueName}Property({method.ParameterIdentifiersNoServicesText});");
                        publicMethods.AppendLine("}");

                        remoteMethods.AppendLine($"public virtual async {method.MethodReturn} Remote{method.UniqueName}({method.ParameterDeclarationsNoServicesText})");
                        remoteMethods.AppendLine("{");
                        remoteMethods.AppendLine($" return await DoRemoteRequest.ForDelegate<{method.DeserializeDelegateTo}>(typeof({method.DelegateName}), [{method.ParameterIdentifiersNoServicesText}]);");
                        remoteMethods.AppendLine("}");
                        remoteMethods.AppendLine("");

                        serviceRegistrations.AppendLine($@"services.AddScoped<{method.DelegateName}>(cc => {{
                                        var factory = cc.GetRequiredService<{className}Factory>();
                                        return ({method.ParameterDeclarationsNoServicesText}) => factory.Local{method.UniqueName}({method.ParameterIdentifiersNoServicesText});
                                    }});");

                        localMethods.AppendLine($"public {asyncText} {method.MethodReturn} Local{method.UniqueName}({method.ParameterDeclarationsNoServicesText})");
                    }
                    else
                    {
                        // Assume we can just call local if there are no services
                        // Don't need the delegate if there aren't any services
                        localMethods.AppendLine($"public {asyncText} {method.MethodReturn} {method.Name}({method.ParameterDeclarationsNoServicesText})"); // Now a publicMethod...
                    }


                    localMethods.AppendLine("{");

                    foreach (var authMethod in method.AuthMethods)
                    {
                        var varText = authMethod.UniqueName.ToLower();
                        localMethods.AppendLine($"var {varText} = {(authMethod.IsAsync ? awaitText : "")} {authMethod.ClassName}.{authMethod.Name}({string.Join(", ", method.Parameters.Take(authMethod.Parameters.Count).Select(a => a.Name))});");
                        localMethods.AppendLine($"if (!{varText}.HasAccess)");
                        localMethods.AppendLine("{");
                        localMethods.AppendLine($"throw new NotAuthorizedException({varText}.Message);");
                        localMethods.AppendLine("}");
                    }

                    localMethods.AppendLine($"var target = ServiceProvider.GetRequiredService<{className}>();");
                    localMethods.AppendLine($"{method.ServiceAssignmentsText.ToString()}");

                    localMethods.AppendLine($"return {awaitText} {method.DoMapperMethodName}(target, DataMapperMethod.{method.AttributeName}, () => target.{method.Name}({method.ParameterIdentifiersText}));");

                    localMethods.AppendLine("}");
                    localMethods.AppendLine("");
                }
                else
                {
                    // Save (Insert, Update, Delete) methods are always local
                    localMethods.AppendLine($"public virtual {asyncText} {method.SaveMethodReturn} Local{method.UniqueName}({returnType} itarget, {method.ParameterDeclarationsNoServicesText})");
                    localMethods.AppendLine("{");
                    localMethods.AppendLine($"var target = ({className})itarget ?? throw new Exception(\"{className} must implement {returnType}\");");
                    localMethods.AppendLine($"{method.ServiceAssignmentsText.ToString()}");
                    localMethods.AppendLine($"return {awaitText} {method.DoMapperMethodName}(target, DataMapperMethod.{method.AttributeName}, () => target.{method.Name}({method.ParameterIdentifiersText}));");

                    localMethods.AppendLine("}");
                    localMethods.AppendLine("");
                }

            }

            var saveMethodSets = classMethods.Where(m => m.IsSave).GroupBy(m => m.ParameterTypesNoServicesText)
                .Select(m =>
                new
                {
                    SaveMethodName = $"Save",
                    ParameterIdentifiersText = m.First().ParameterIdentifiersNoServicesText,
                    ParameterDeclarationText = m.First().ParameterDeclarationsNoServicesText,
                    IsAsync_ = m.Any(m => m.IsAsync),
                    InsertMethod = m.FirstOrDefault(m => m.AttributeName == "Insert"),
                    UpdateMethod = m.FirstOrDefault(m => m.AttributeName == "Update"),
                    DeleteMethod = m.FirstOrDefault(m => m.AttributeName == "Delete"),
                    AuthMethods = m.SelectMany(m => m.AuthMethods).Distinct().ToList(),
                    Parameters = m.First().Parameters
                });

            var isEdit = false;

            foreach (var saveMethodSet in saveMethodSets)
            {
                isEdit = true;
                var saveMethodName = saveMethodSet.SaveMethodName;

                if (methodNames.Contains(saveMethodName))
                {
                    var count = 1;
                    while (methodNames.Contains($"{saveMethodName}{count}"))
                    {
                        count += 1;
                    }
                    saveMethodName = $"{saveMethodName}{count}";
                }
                methodNames.Add(saveMethodName);

                var isDefault = string.IsNullOrWhiteSpace(saveMethodSet.ParameterIdentifiersText);

                var delegateName = $"{saveMethodName}Delegate";
                var saveMethodReturn = $"{returnType}?";
                var isSaveMethodAsync = saveMethodSet.IsAsync_;
                var isRemote = false;
                var authMethods = new StringBuilder();

                foreach (var authMethod in saveMethodSet.AuthMethods)
                {
                    var varText = authMethod.UniqueName.ToLower();

                    var saveMethodParameter = saveMethodSet.Parameters.GetEnumerator();
                    var authMethodParameter = authMethod.Parameters.GetEnumerator();
                    var parameterText = new List<string>();

                    var matchFail = false;

                    saveMethodParameter.MoveNext();
                    authMethodParameter.MoveNext();

                    if (authMethodParameter.Current != null)
                    {
                        do
                        {
                            if (authMethodParameter.Current.Type == returnType || authMethodParameter.Current.Type == $"I{returnType}")
                            {
                                parameterText.Add("target");
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
                                parameterText.Add(saveMethodParameter.Current.Name);
                            }
                        } while (authMethodParameter.MoveNext() && saveMethodParameter.MoveNext());
                    }
                    if (matchFail)
                    {
                        saveMethods.AppendLine($"// Not able to call {authMethod.Name} due to parameter mismatch;");
                        continue;
                    }

                    isRemote = isRemote || authMethod.IsRemote;
                    isSaveMethodAsync = isSaveMethodAsync || authMethod.IsAsync;
                    authMethods.AppendLine($"var {varText} = {(authMethod.IsAsync ? "await" : "")} {authMethod.ClassName}.{authMethod.Name}({string.Join(", ", parameterText)});");
                    authMethods.AppendLine($"if (!{varText}.HasAccess)");
                    authMethods.AppendLine("{");
                    authMethods.AppendLine($"throw new NotAuthorizedException({varText}.Message);");
                    authMethods.AppendLine("}");
                }

                if (isRemote)
                {
                    constructorPropertyAssignmentsLocal.AppendLine($"{saveMethodName}Property = Local{saveMethodName};");
                    constructorPropertyAssignmentsRemote.AppendLine($"{saveMethodName}Property = Remote{saveMethodName};");
                    delegates.AppendLine($"public delegate Task<{returnType}?> {saveMethodName}Delegate({returnType} target, {saveMethodSet.ParameterDeclarationText});");
                    propertyDeclarations.AppendLine($"public {delegateName} {saveMethodName}Property {{ get; set; }}");
                }

                if (isSaveMethodAsync || isRemote)
                {
                    saveMethodReturn = $"Task<{returnType}?>";
                }

                interfaceMethods.AppendLine($"{saveMethodReturn} Save({returnType} target, {saveMethodSet.ParameterDeclarationText});");

                if (isDefault)
                {
                    saveMethods.AppendLine($"public override async Task<IEditBase?> Save({className} target)");
                    saveMethods.AppendLine("{");
                    if (isRemote)
                    {
                        saveMethods.AppendLine($"return (IEditBase?) (await {saveMethodName}Property(target));");
                    }
                    else if (isRemote)
                    {
                        saveMethods.AppendLine($"return (IEditBase?) await Save(target);");
                    }
                    else
                    {
                        saveMethods.AppendLine($"return await Task.FromResult((IEditBase?) Save(target));");
                    }
                    saveMethods.AppendLine("}");
                }

                if (isRemote)
                {
                    saveMethods.AppendLine($"public Task<{returnType}?> Save({returnType} target, {saveMethodSet.ParameterDeclarationText})");
                    saveMethods.AppendLine("{");
                    saveMethods.AppendLine($"return {saveMethodName}Property(target, {saveMethodSet.ParameterIdentifiersText});");
                    saveMethods.AppendLine("}");

                    saveMethods.AppendLine($@"public async Task<{returnType}?> Remote{saveMethodName}({returnType} target, {saveMethodSet.ParameterDeclarationText}){{");
                    saveMethods.AppendLine($@"return await DoRemoteRequest.ForDelegate<{className}?>(typeof({delegateName}), [target, {saveMethodSet.ParameterIdentifiersText}]);");
                    saveMethods.AppendLine("}");

                    serviceRegistrations.AppendLine($@"services.AddScoped<{delegateName}>(cc => {{
                                        var factory = cc.GetRequiredService<{className}Factory>();
                                        return (target, {saveMethodSet.ParameterIdentifiersText}) => factory.Local{saveMethodName}(target, {saveMethodSet.ParameterIdentifiersText});
                                    }});");

                    saveMethods.AppendLine($@"public virtual {(isSaveMethodAsync ? "async" : "")} Task<{returnType}?> Local{saveMethodName}({returnType} target, {saveMethodSet.ParameterDeclarationText})
{{");
                }
                else
                {
                    saveMethods.AppendLine($@"public virtual {(isSaveMethodAsync ? "async" : "")} {saveMethodReturn} Save({returnType} target, {saveMethodSet.ParameterDeclarationText})
{{");
                }


                saveMethods.Append(authMethods);

                string CallMethod(MethodInfo? method)
                {
                    if (method == null)
                    {
                        return $"throw new NotImplementedException()";
                    }

                    var methodCall = $"Local{method.UniqueName}(target, {saveMethodSet.ParameterIdentifiersText})";

                    if (method.IsAsync)
                    {
                        methodCall = $"await {methodCall}";
                    }
                    else if (isRemote && !isSaveMethodAsync)
                    {
                        methodCall = $"Task.FromResult({methodCall})";
                    }

                    return $"return {methodCall};";
                }

                saveMethods.AppendLine($@"

                if (target.IsDeleted)
        {{
            if (target.IsNew)
            {{
                return null;
            }}
            {CallMethod(saveMethodSet.DeleteMethod)};
        }}
        else if (target.IsNew)
        {{
            {CallMethod(saveMethodSet.InsertMethod)};
        }}
        else
        {{
             {CallMethod(saveMethodSet.UpdateMethod)};
        }}
");

                saveMethods.AppendLine("}");



            }


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
    }}
    
    internal class {className}Factory : Factory{editText}Base{(isEdit ? $"<{className}>" : "")}, I{className}Factory
    {{
    
        private readonly IServiceProvider ServiceProvider;  
        private readonly IDoRemoteRequest DoRemoteRequest;


{propertyDeclarations.ToString()}
{delegates.ToString()}

        public {className}Factory(IServiceProvider serviceProvider, {string.Join("\n,", constructorParametersLocal)})
        {{
                this.ServiceProvider = serviceProvider;
                {constructorPropertyAssignmentsLocal.ToString()}
        }}

        public {className}Factory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate, {string.Join("\n,", constructorParametersLocal)})
        {{
                this.ServiceProvider = serviceProvider;
                this.DoRemoteRequest = remoteMethodDelegate;
                {constructorPropertyAssignmentsRemote.ToString()}
        }}

{publicMethods.ToString()}
{localMethods.ToString()}
{remoteMethods.ToString()}
{saveMethods.ToString()}

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {{
            services.AddTransient<{className}>();
            services.AddScoped<{className}Factory>();
            services.AddScoped<I{className}Factory, {className}Factory>();
{serviceRegistrations.ToString()}
        }}

    }}
}}";
            source = source.Replace("(, ", "(");
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

        public static List<ClassDeclarationSyntax> FindImplementingClasses(InterfaceDeclarationSyntax interfaceSyntax, Compilation compilation)
        {
            var interfaceName = interfaceSyntax.Identifier.Text;
            var implementingClasses = new List<ClassDeclarationSyntax>();

            foreach (var tree in compilation.SyntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(tree);
                var root = tree.GetRoot();

                var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (var classDeclaration in classDeclarations)
                {
                    var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

                    if (classSymbol != null && classSymbol.Interfaces.Any(i => i.Name == interfaceName))
                    {
                        implementingClasses.Add(classDeclaration);
                    }
                }
            }

            return implementingClasses;
        }
    }
}