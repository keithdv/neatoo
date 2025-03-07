using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neatoo.Portal;
using System;
using System.Collections;
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
            try
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
            }
            catch (Exception ex)
            {
                return null;
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

        internal class CallMethodInfo
        {
            public CallMethodInfo(string targetType, INamedTypeSymbol classSymbol, IMethodSymbol methodSymbol, MethodDeclarationSyntax methodDeclaration)
            {


                var attributes = methodSymbol.GetAttributes().Select(a => a.AttributeClass?.Name.Replace("Attribute", "")).Where(a => a != null).ToList();

                Name = methodSymbol.Name;
                ClassName = methodSymbol.ContainingType.Name;
                IsBool = methodSymbol.ReturnType.ToString().Contains("bool");
                IsTask = methodSymbol.ReturnType.ToString().Contains("Task");
                IsRemote = attributes.Any(a => a == "Remote");
                IsSave = attributes.Any(a => dataMapperSaveAttributes.Contains(a));

                foreach (var attribute in attributes)
                {
                    if (Enum.TryParse<DataMapperMethod>(attribute, out var dmm))
                    {
                        DataMapperMethod = dmm;
                        break;
                    }
                    if (attribute == "Authorize")
                    {
                        var attr = methodDeclaration.AttributeLists.SelectMany(a => a.Attributes)
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

                        DataMapperMethodType = dataMapperMethodTypes.Aggregate((a, b) => a | b);
                    }
                }

                //if (methodSymbol.IsGenericMethod)
                //{
                //    Parameters = methodSymbol.Parameters.Select(p => new ParameterInfo()
                //    {
                //        Name = p.Name,
                //        Type = p.Type.ToString(),
                //        IsService = p.GetAttributes().Any(a => a.AttributeClass?.Name.ToString() == "ServiceAttribute"),
                //    }).ToList();

                //    Parameters.ForEach(p =>
                //    {
                //        p.Type = Regex.Replace(p.Type, @"\w+\.", "");
                //        p.IsTarget = p.Type.ToString() == targetType;
                //    });
                //}
                //else
                //{
                if (methodDeclaration != null)
                {
                    Parameters = methodDeclaration.ParameterList.Parameters.Select(p => new ParameterInfo()
                    {
                        Name = p.Identifier.Text,
                        Type = p.Type.ToFullString(),
                        IsService = p.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.ToFullString() == "Service"),
                        //IsTarget = p.Type.ToFullString() == targetType
                    }).ToList();

                } else
                {
                    Parameters = new List<ParameterInfo>();
                }

                foreach(var targetParam in methodSymbol.Parameters.Where(p => p.Type == classSymbol))
                {
                    Parameters.Where(p => p.Name == targetParam.Name).ToList().ForEach(p => p.IsTarget = true);
                }
                //}
            }

            public string Name { get; set; }
            public string ClassName { get; set; }
            public string NamePostfix => Name.Replace(DataMapperMethod?.ToString() ?? "", "");
            public bool IsBool { get; private set; }
            public bool IsTask { get; private set; }
            public bool IsRemote { get; private set; }
            public bool IsSave { get; private set; }
            public DataMapperMethod? DataMapperMethod { get; private set; }
            public DataMapperMethodType? DataMapperMethodType { get; private set; }
            public List<ParameterInfo> Parameters { get; private set; }

            public void MakeAuthCall(FactoryMethod inMethod, StringBuilder methodBuilder)
            {
                var parameters = inMethod.Parameters.ToList();

                if (!Parameters.Any(p => p.IsTarget))
                {
                    parameters.RemoveAll(p => p.IsTarget);
                }

                var parameterText = string.Join(", ", parameters.Select(a => a.Name).Take(this.Parameters.Count));

                var callText = $"{this.ClassName}.{this.Name}({parameterText})";

                if (this.IsTask)
                {
                    callText = $"await {callText}";
                }

                methodBuilder.AppendLine($"authorized = {callText};");
                methodBuilder.AppendLine($"if (!authorized.HasAccess)");
                methodBuilder.AppendLine("{");

                var returnText = $"authorized";
                if (inMethod is not CanFactoryMethod)
                {
                    returnText = $"new {inMethod.ReturnType(includeTask: false)}(authorized)";
                }

                if (!this.IsTask && inMethod.IsTask && !inMethod.IsAsync)
                {
                    returnText = $"Task.FromResult({returnText})";
                }

                methodBuilder.AppendLine($"return {returnText};");
                methodBuilder.AppendLine("}");
            }
        }

        /// <summary>
        /// Insert, Update and Delete
        /// </summary>
        internal class WriteFactoryMethod : ReadFactoryMethod
        {
            public WriteFactoryMethod(string targetType, string concreteType, CallMethodInfo callMethodInfo) : base(targetType, concreteType, callMethodInfo)
            {
                Parameters.Insert(0, new ParameterInfo() { Name = "target", Type = $"{targetType}", IsService = false, IsTarget = true });
            }

            public override StringBuilder PublicMethod(ClassText classText)
            {
                return new StringBuilder();
            }

            public override StringBuilder RemoteMethod(ClassText classText)
            {
                return new StringBuilder();
            }

            public override StringBuilder LocalMethod()
            {
                var methodBuilder = base.LocalMethodStart();

                methodBuilder.AppendLine($"var cTarget = ({ConcreteType}) target ?? throw new Exception(\"{TargetType} must implement {ConcreteType}\");");
                methodBuilder.AppendLine($"{ServiceAssignmentsText}");
                methodBuilder.AppendLine($"return {DoMapperMethodCall.Replace("target", "cTarget")};");
                methodBuilder.AppendLine("}");
                methodBuilder.AppendLine("");

                return methodBuilder;
            }
        }

        internal class SaveFactoryMethod : FactoryMethod
        {
            public SaveFactoryMethod(string targetType, string concreteType, List<WriteFactoryMethod> dataMapperSaveMethods) : base(targetType, concreteType)
            {
                var dataMapperSaveMethod = dataMapperSaveMethods.First();
                Name = $"Save{dataMapperSaveMethod.NamePostfix}";
                UniqueName = Name;
                DataMapperSaveMethods = dataMapperSaveMethods;
                this.Parameters = dataMapperSaveMethods.First().Parameters;
            }

            public bool IsDefault { get; set; } = false;
            public override bool IsSave => true;
            public override bool IsBool => true;
            public override bool IsRemote => DataMapperSaveMethods.Any(m => m.IsRemote);
            public override bool IsTask => IsRemote || DataMapperSaveMethods.Any(m => m.IsTask);
            public override bool IsAsync => DataMapperSaveMethods.Any(m => m.IsTask) && DataMapperSaveMethods.Any(m => !m.IsTask);
            public override bool HasAuth => DataMapperSaveMethods.Any(m => m.HasAuth);

            public List<WriteFactoryMethod> DataMapperSaveMethods { get; }

            public override StringBuilder PublicMethod(ClassText classText)
            {
                if (!HasAuth)
                {
                    return base.PublicMethod(classText);
                }

                var methodBuilder = new StringBuilder();

                var asyncKeyword = IsTask && HasAuth ? "async" : "";
                var awaitKeyword = IsTask && HasAuth ? "await" : "";

                classText.InterfaceMethods.AppendLine($"{ReturnType(includeAuth: false)} {Name}({ParameterDeclarationsText()});");

                methodBuilder.AppendLine($"public virtual {asyncKeyword} {ReturnType(includeAuth: false)} {Name}({ParameterDeclarationsText()})");
                methodBuilder.AppendLine("{");

                methodBuilder.AppendLine($"var authorized = ({awaitKeyword} Local{UniqueName}({ParameterIdentifiersText()}));");

                methodBuilder.AppendLine("if (!authorized.HasAccess)");
                methodBuilder.AppendLine("{");
                methodBuilder.AppendLine("throw new NotAuthorizedException(authorized.Message);");
                methodBuilder.AppendLine("}");
                methodBuilder.AppendLine("return authorized.Result;");
                methodBuilder.AppendLine("}");

                classText.InterfaceMethods.AppendLine($"{ReturnType()} Try{Name}({ParameterDeclarationsText()});");

                methodBuilder.AppendLine($"public virtual {AsyncKeyword} {ReturnType()} Try{Name}({ParameterDeclarationsText()})");
                methodBuilder.AppendLine("{");
                methodBuilder.AppendLine($"return {AwaitKeyword} Local{UniqueName}({ParameterIdentifiersText()});");
                methodBuilder.AppendLine("}");

                if (IsRemote)
                {
                    methodBuilder.Replace($"Local{UniqueName}", $"{UniqueName}Property");
                }

                return methodBuilder;
            }

            public override StringBuilder LocalMethod()
            {
                var methodBuilder = new StringBuilder();

                if (IsDefault)
                {
                    methodBuilder.AppendLine($"async Task<IEditBase?> IFactoryEditBase<{ConcreteType}>.Save({ConcreteType} target)");
                    methodBuilder.AppendLine("{");

                    if (IsTask)
                    {
                        methodBuilder.AppendLine($"return (IEditBase?) await {Name}(target);");
                    }
                    else
                    {
                        methodBuilder.AppendLine($"return await Task.FromResult((IEditBase?) {Name}(target));");
                    }
                    methodBuilder.AppendLine("}");
                }

                methodBuilder.AppendLine($@"public virtual {AsyncKeyword} {ReturnType()} Local{UniqueName}({ParameterDeclarationsText()})
                                            {{");

                string DoInsertUpdateDeleteMethodCall(FactoryMethod? method)
                {

                    if (method == null)
                    {
                        return $"throw new NotImplementedException()";
                    }

                    var methodCall = $"Local{method.UniqueName}({ParameterIdentifiersText()})";

                    if (method.IsTask && IsAsync)
                    {
                        methodCall = $"await {methodCall}";
                    }

                    if (HasAuth && !method.HasAuth)
                    {
                        methodCall = $"new Authorized<{TargetType}>({methodCall})";
                    }

                    if (!method.IsTask && IsTask && !IsAsync)
                    {
                        methodCall = $"Task.FromResult({methodCall})";
                    }

                    return $"return {methodCall}";
                }

                methodBuilder.AppendLine($@"
                                            if (target.IsDeleted)
                                    {{
                                        if (target.IsNew)
                                        {{
                                            return null;
                                        }}
                                        {DoInsertUpdateDeleteMethodCall(DataMapperSaveMethods.Where(s => s.DataMapperMethod == DataMapper.DataMapperMethod.Delete).FirstOrDefault())};
                                    }}
                                    else if (target.IsNew)
                                    {{
                                        {DoInsertUpdateDeleteMethodCall(DataMapperSaveMethods.Where(s => s.DataMapperMethod == DataMapper.DataMapperMethod.Insert).FirstOrDefault())};
                                    }}
                                    else
                                    {{
                                         {DoInsertUpdateDeleteMethodCall(DataMapperSaveMethods.Where(s => s.DataMapperMethod == DataMapper.DataMapperMethod.Update).FirstOrDefault())};
                                    }}
                            ");

                methodBuilder.AppendLine("}");

                return methodBuilder;
            }
        }

        internal class ReadFactoryMethod : FactoryMethod
        {
            public ReadFactoryMethod(string targetType, string concreteType, CallMethodInfo callMethod) : base(targetType, concreteType)
            {
                this.ConcreteType = concreteType;
                this.CallMethod = callMethod;
                this.Name = callMethod.Name;
                this.UniqueName = callMethod.Name;
                this.NamePostfix = callMethod.NamePostfix;
                this.DataMapperMethod = callMethod.DataMapperMethod;
                this.DataMapperMethodType = callMethod.DataMapperMethodType;
                this.Parameters = callMethod.Parameters;
            }

            public override bool IsSave => CallMethod.IsSave;
            public override bool IsBool => CallMethod.IsBool;
            public override bool IsTask => IsRemote || CallMethod.IsTask || AuthCallMethods.Any(m => m.IsTask);
            public override bool IsRemote => CallMethod.IsRemote || AuthCallMethods.Any(m => m.IsRemote);
            public override bool IsAsync => (HasAuth && CallMethod.IsTask) || AuthCallMethods.Any(m => m.IsTask);

            public CallMethodInfo CallMethod { get; set; }

            public string DoMapperMethodCall
            {
                get
                {
                    var methodCall = "DoMapperMethodCall";

                    if (CallMethod.IsBool)
                    {
                        methodCall += "Bool";
                    }

                    if (CallMethod.IsTask)
                    {
                        methodCall += "Async";
                    }

                    methodCall += $"<{TargetType}>";

                    methodCall = $"{methodCall}(target, DataMapperMethod.{DataMapperMethod}, () => target.{Name} ({ParameterIdentifiersText(includeServices: true, includeTarget: false)}))";

                    if (IsAsync && CallMethod.IsTask)
                    {
                        methodCall = $"await {methodCall}";
                    }

                    if (HasAuth)
                    {
                        methodCall = $"new Authorized<{TargetType}>({methodCall})";
                    }

                    if (!CallMethod.IsTask && IsTask && !IsAsync)
                    {
                        methodCall = $"Task.FromResult({methodCall})";
                    }

                    return methodCall;
                }
            }

            public override StringBuilder LocalMethod()
            {
                var methodBuilder = base.LocalMethodStart();

                methodBuilder.AppendLine($"var target = ServiceProvider.GetRequiredService<{ConcreteType}>();");
                methodBuilder.AppendLine($"{ServiceAssignmentsText}");
                methodBuilder.AppendLine($"return {DoMapperMethodCall};");
                methodBuilder.AppendLine("}");
                methodBuilder.AppendLine("");

                return methodBuilder;
            }
        }

        internal class CanFactoryMethod : FactoryMethod
        {
            public CanFactoryMethod(string targetType, string concreteType, ReadFactoryMethod readFactoryMethod) : base(targetType, concreteType)
            {
                this.Name = $"Can{readFactoryMethod.Name}";
                this.UniqueName = this.Name;
                this.NamePostfix = readFactoryMethod.NamePostfix;
                this.AuthCallMethods.AddRange(readFactoryMethod.AuthCallMethods);
                this.Parameters = readFactoryMethod.Parameters.Where(p => !p.IsTarget).ToList();
            }

            public override bool IsBool => true;

            public override string ReturnType(bool includeTask = true, bool includeAuth = true, bool includeBool = true)
            {
                var returnType = "Authorized";

                if (includeTask && IsTask)
                {
                    returnType = $"Task<{returnType}>";
                }

                return returnType;
            }

            public override StringBuilder PublicMethod(ClassText classText)
            {

                classText.InterfaceMethods.AppendLine($"{ReturnType()} {Name}({ParameterDeclarationsText(includeServices: false)});");

                var methodBuilder = new StringBuilder();

                methodBuilder.AppendLine($"public virtual {ReturnType()} {Name}({ParameterDeclarationsText(includeServices: false)})");
                methodBuilder.AppendLine("{");

                methodBuilder.AppendLine($"return Local{UniqueName}({ParameterIdentifiersText(includeServices: false)});");

                methodBuilder.AppendLine("}");

                if (IsRemote)
                {
                    methodBuilder.Replace($"Local{UniqueName}", $"{UniqueName}Property");
                }

                return methodBuilder;
            }

            public override StringBuilder LocalMethod()
            {
                var methodBuilder = base.LocalMethodStart();

                var returnText = $"new Authorized(true)";

                if (IsTask && !IsAsync)
                {
                    returnText = $"Task.FromResult({returnText})";
                }

                methodBuilder.AppendLine($"return {returnText};");

                methodBuilder.AppendLine("}");
                methodBuilder.AppendLine("");
                return methodBuilder;
            }
        }

        internal abstract class FactoryMethod
        {
            public FactoryMethod(string targetType, string concreteType)
            {
                this.TargetType = targetType;
                this.ConcreteType = concreteType;
            }

            public string TargetType { get; }
            public string ConcreteType { get; set; }
            public string Name { get; protected set; }
            public string UniqueName { get; set; }
            public string NamePostfix { get; protected set; }
            public string DelegateName => $"{UniqueName}Delegate";

            public List<CallMethodInfo> AuthCallMethods { get; set; } = new List<CallMethodInfo>();
            public virtual bool HasAuth => AuthCallMethods.Count > 0;
            public DataMapperMethod? DataMapperMethod { get; set; }
            public DataMapperMethodType? DataMapperMethodType { get; set; }
            public List<ParameterInfo> Parameters { get; set; }
            public virtual bool IsSave => false;
            public virtual bool IsBool => false;
            public virtual bool IsTask => IsRemote || AuthCallMethods.Any(m => m.IsTask);
            public virtual bool IsRemote => AuthCallMethods.Any(m => m.IsRemote);
            public virtual bool IsAsync => AuthCallMethods.Any(m => m.IsTask);

            public virtual string AsyncKeyword => IsAsync ? "async" : "";
            public virtual string AwaitKeyword => IsAsync ? "await" : "";
            public string ServiceAssignmentsText => string.Join("\n", Parameters.Where(p => p.IsService).Select(p => $"\t\t\tvar {p.Name} = ServiceProvider.GetService<{p.Type}>();"));
            public virtual string ReturnType(bool includeTask = true, bool includeAuth = true, bool includeBool = true)
            {
                var returnType = TargetType;

                if (HasAuth && includeAuth)
                {
                    returnType = $"Authorized<{returnType}>";
                }
                else if (IsBool && includeBool)
                {
                    returnType = $"{returnType}?";
                }

                if (includeTask && IsTask)
                {
                    returnType = $"Task<{returnType}>";
                }

                return returnType;
            }
            public string ParameterDeclarationsText(bool includeServices = false, bool includeTarget = true)
            {
                return string.Join(", ", Parameters.Where(p => (includeServices || !p.IsService) && (includeTarget || !p.IsTarget)).Select(p => $"{p.Type} {p.Name}"));
            }

            public string ParameterIdentifiersText(bool includeServices = false, bool includeTarget = true)
            {
                var result = string.Join(", ", Parameters.Where(p => (includeServices || !p.IsService) && (includeTarget || !p.IsTarget)).Select(p => p.Name));

                return result.TrimStart(',').TrimEnd(',');
            }

            public virtual StringBuilder PublicMethod(ClassText classText)
            {
                var asyncKeyword = IsTask && HasAuth ? "async" : "";
                var awaitKeyword = IsTask && HasAuth ? "await" : "";

                classText.InterfaceMethods.AppendLine($"{ReturnType(includeAuth: false)} {Name}({ParameterDeclarationsText(includeServices: false)});");

                var methodBuilder = new StringBuilder();

                methodBuilder.AppendLine($"public virtual {asyncKeyword} {ReturnType(includeAuth: false)} {Name}({ParameterDeclarationsText(includeServices: false)})");
                methodBuilder.AppendLine("{");

                if (!HasAuth)
                {
                    methodBuilder.AppendLine($"return Local{UniqueName}({ParameterIdentifiersText()});");
                }
                else
                {
                    methodBuilder.AppendLine($"return ({awaitKeyword} Local{UniqueName}({ParameterIdentifiersText(includeServices: false)})).Result;");
                }

                methodBuilder.AppendLine("}");

                if (IsRemote)
                {
                    methodBuilder.Replace($"Local{UniqueName}", $"{UniqueName}Property");
                }

                return methodBuilder;
            }

            public virtual StringBuilder RemoteMethod(ClassText classText)
            {
                var methodBuilder = new StringBuilder();
                if (IsRemote)
                {

                    classText.Delegates.AppendLine($"public delegate {ReturnType()} {DelegateName}({ParameterDeclarationsText()});");
                    classText.PropertyDeclarations.AppendLine($"public {DelegateName} {UniqueName}Property {{ get; }}");
                    classText.ConstructorPropertyAssignmentsLocal.AppendLine($"{UniqueName}Property = Local{UniqueName};");
                    classText.ConstructorPropertyAssignmentsRemote.AppendLine($"{UniqueName}Property = Remote{UniqueName};");

                    methodBuilder.AppendLine($"public virtual async {ReturnType()} Remote{UniqueName}({ParameterDeclarationsText()})");
                    methodBuilder.AppendLine("{");
                    methodBuilder.AppendLine($" return await DoRemoteRequest.ForDelegate<{ReturnType(includeTask: false)}>(typeof({DelegateName}), [{ParameterIdentifiersText()}]);");
                    methodBuilder.AppendLine("}");
                    methodBuilder.AppendLine("");

                    classText.ServiceRegistrations.AppendLine($@"services.AddScoped<{DelegateName}>(cc => {{
                                                    var factory = cc.GetRequiredService<{ConcreteType}Factory>();
                                                    return ({ParameterDeclarationsText()}) => factory.Local{UniqueName}({ParameterIdentifiersText()});
                                                }});");
                }
                return methodBuilder;
            }

            protected virtual StringBuilder LocalMethodStart()
            {
                var methodBuilder = new StringBuilder();

                methodBuilder.AppendLine($"public {AsyncKeyword} {ReturnType()} Local{UniqueName}({ParameterDeclarationsText()})");
                methodBuilder.AppendLine("{");

                if (AuthCallMethods.Count > 0)
                {
                    methodBuilder.AppendLine("Authorized authorized;");
                    foreach (var authMethod in AuthCallMethods)
                    {
                        authMethod.MakeAuthCall(this, methodBuilder);
                    }
                }

                return methodBuilder;
            }

            public abstract StringBuilder LocalMethod();
        }

        internal class MethodInfo
        {
            public MethodInfo()
            {

            }
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
                IsRemote = methodDeclarations.Any(m => m.IsRemote);
                IsTask = IsSelfTask || IsRemote;
                IsAsync = IsRemote;
            }

            public MethodInfo(string classType, IMethodSymbol methodDeclaration)
            {
                var attributes = methodDeclaration.GetAttributes().Select(a => a.AttributeClass?.Name).Where(a => a != null).ToList();
                Name = methodDeclaration.Name;
                ClassType = classType;
                UniqueName = methodDeclaration.Name;
                IsSelfBool = methodDeclaration.ReturnType.ToString().Contains("bool");
                IsSelfTask = methodDeclaration.ReturnType.ToString().Contains("Task");
                IsRemote = attributes.Any(a => a == "RemoteAttribute");
                IsSave = attributes.Any(a => dataMapperSaveAttributes.Contains(a.Replace("Attribute", "")));
                DataMapperAttribute = dataMapperAttributes.FirstOrDefault(a => attributes.Contains($"{a}Attribute"));
                Parameters = methodDeclaration.Parameters.Select(p => new ParameterInfo()
                {
                    Name = p.Name,
                    Type = p.Type.ToString(),
                    IsService = p.GetAttributes().Any(a => a.AttributeClass?.Name.ToString() == "ServiceAttribute"),
                }).ToList();

                Parameters.ForEach(p =>
                {
                    p.Type = Regex.Replace(p.Type, @"\w+\.", "");
                    p.IsTarget = p.Type.ToString() == classType;
                });

                if (IsSave)
                {
                    Parameters.Insert(0, new ParameterInfo() { Name = "target", Type = $"{ClassType}", IsService = false, IsTarget = true });
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
            public string ClassType { get; set; }
            public bool IsSelfBool { get; set; }
            public List<ParameterInfo> Parameters { get; set; } = new List<ParameterInfo>();
            public bool IsAsync { get; set; }
            public string AsyncKeyword => IsAsync ? "async" : "";
            public string AwaitKeyword => IsAsync ? "await" : "";
            public bool IsTask { get; set; }
            public bool IsSelfTask { get; set; }
            public bool IsRemote { get; set; }
            public bool IsSave { get; }
            public bool IsCan { get; set; }
            public bool HasAuth => AuthMethods.Count > 0;
            public List<string> Attributes { get; set; }
            public DataMapper.DataMapperMethod? DataMapperMethod { get; set; }
            public DataMapper.DataMapperMethodType? DataMapperMethodType { get; set; }
            public string DataMapperMethodTypeText { get; set; }
            public string FactoryMethodDelegateName => $"{UniqueName}Delegate";
            public string AuthResult => HasAuth ? $".Result" : "";



            public string DataMapperAttribute { get; set; }
            public string ParameterDeclarationsText(bool includeServices = false, bool includeAuth = false, bool includeTarget = true)
            {
                return string.Join(", ", Parameters.Where(p => (includeServices || !p.IsService) && (includeAuth) && (includeTarget || !p.IsTarget)).Select(p => $"{p.Type} {p.Name}"));
            }

            public string ParameterIdentifiersText(bool includeServices = false, bool includeAuth = false, bool includeTarget = true)
            {
                var result = string.Join(", ", Parameters.Where(p => (includeServices || !p.IsService) && (includeAuth) && (includeTarget || !p.IsTarget)).Select(p => p.Name));

                return result.TrimStart(',').TrimEnd(',');
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

                    methodCall = $"{methodCall}(target, DataMapperMethod.{DataMapperAttribute}, () => target.{Name} ({ParameterIdentifiersText(includeServices: true, includeTarget: false)}))";

                    if (AuthMethods.Count > 0)
                    {
                        if (IsSelfTask)
                        {
                            methodCall = $"await {methodCall}";
                        }
                        methodCall = $"new Authorized<{ClassType}>({methodCall})";
                    }
                    else if (!IsSelfTask && IsTask)
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
            public bool IsTarget { get; set; }
        }

        internal class ClassText
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
                        classDeclarationSyntax = GetBaseClassDeclarationSyntax(semanticModel, classDeclarationSyntax, messages);
                    }


                    List<CallMethodInfo> dataMapperCallMethods = FindMethods(returnType, classNamedSymbol, messages);
                    List<CallMethodInfo> authCallMethods = FindAuthMethods(semanticModel, returnType, classNamedSymbol, messages);

                    List<FactoryMethod> factoryMethods = new List<FactoryMethod>();

                    foreach (var dataMapperMethod in dataMapperCallMethods)
                    {
                        if (dataMapperMethod.IsSave)
                        {
                            factoryMethods.Add(new WriteFactoryMethod(returnType, concreteType, dataMapperMethod));
                        }
                        else
                        {
                            factoryMethods.Add(new ReadFactoryMethod(returnType, concreteType, dataMapperMethod));
                        }
                    }

                    MatchAuthMethods(factoryMethods, authCallMethods, messages);

                    foreach (var factoryMethod in factoryMethods.OfType<ReadFactoryMethod>().ToList())
                    {
                        if (factoryMethod.HasAuth)
                        {
                            if (factoryMethod.AuthCallMethods.Any(m => m.Parameters.Any(p => p.IsTarget)))
                            {
                                messages.Add($"Factory Can{factoryMethod.Name} not created because it matches to an auth method with a {returnType} parameter");
                            }
                            else
                            {
                                factoryMethods.Add(new CanFactoryMethod(returnType, concreteType, factoryMethod));
                            }
                        }
                    }


                    var saveMethods = factoryMethods
                                    .OfType<WriteFactoryMethod>()
                                    .Where(m => m.IsSave)
                                    .GroupBy(m => string.Join(",", m.Parameters.Where(m => !m.IsTarget && !m.IsService)
                                                            .Select(m => m.Type.ToString())))
                                    .ToList();

                    foreach (var saveMethod in saveMethods)
                    {
                        if (saveMethod.Count(m => m.DataMapperMethod == DataMapperMethod.Insert) > 1
                            || saveMethod.Count(m => m.DataMapperMethod == DataMapperMethod.Update) > 1
                            || saveMethod.Count(m => m.DataMapperMethod == DataMapperMethod.Delete) > 1)
                        {
                            var byName = saveMethod.GroupBy(m => m.NamePostfix).ToList();

                            foreach (var byNameMethod in byName)
                            {
                                if (byNameMethod.Count(m => m.DataMapperMethod == DataMapperMethod.Insert) > 1
                                        || byNameMethod.Count(m => m.DataMapperMethod == DataMapperMethod.Update) > 1
                                        || byNameMethod.Count(m => m.DataMapperMethod == DataMapperMethod.Delete) > 1)
                                {
                                    messages.Add($"Multiple Insert/Update/Delete methods with the same name: {saveMethod.First().Name}");
                                    break;
                                }

                                factoryMethods.Add(new SaveFactoryMethod(returnType, concreteType, byNameMethod.ToList()));
                            }
                        }
                        else
                        {
                            factoryMethods.Add(new SaveFactoryMethod(returnType, concreteType, saveMethod.ToList()));
                        }
                    }

                    var defaultSaveMethod = factoryMethods.OfType<SaveFactoryMethod>()
                                    .Where(s => s.Parameters.Where(p => !p.IsTarget && !p.IsService).Count() == 0 && s.Parameters.First().IsTarget)
                                    .FirstOrDefault();
                    if (defaultSaveMethod != null) { defaultSaveMethod.IsDefault = true; }

                    foreach (var method in factoryMethods.OrderBy(m => m.Parameters.Count).ToList())
                    {
                        if (methodNames.Contains(method.Name))
                        {
                            var count = 1;
                            while (methodNames.Contains($"{method.UniqueName}{count}"))
                            {
                                count += 1;
                            }
                            method.UniqueName = $"{method.UniqueName}{count}";
                        }
                        methodNames.Add(method.UniqueName);
                    }

                    foreach (var authMethods in authCallMethods.GroupBy(a => a.ClassName))
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

                    foreach (var factoryMethod in factoryMethods)
                    {
                        var methodBuilder = new StringBuilder();
                        methodBuilder.Append(factoryMethod.PublicMethod(classText));
                        methodBuilder.Append(factoryMethod.RemoteMethod(classText));
                        methodBuilder.Append(factoryMethod.LocalMethod());
                        classText.MethodsBuilder.Append(methodBuilder);
                    }

                    var isEdit = saveMethods.Any();
                    var editText = isEdit ? "Edit" : "";
                    if (isEdit)
                    {
                        classText.ServiceRegistrations.AppendLine($@"services.AddScoped<IFactoryEditBase<{className}>, {className}Factory>();");
                    }

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
                    source = source.Replace("[, ", "[");
                    source = source.Replace("(, ", "(");
                    source = source.Replace(", )", ")");
                    source = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().SyntaxTree.GetText().ToString();
                }
                catch (Exception ex)
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

        private static List<IMethodSymbol> GetMethodsRecursive(INamedTypeSymbol classNamedSymbol)
        {
            var methods = classNamedSymbol.GetMembers().OfType<IMethodSymbol>().ToList();
            if (classNamedSymbol.BaseType != null)
            {
                methods.AddRange(GetMethodsRecursive(classNamedSymbol.BaseType));
            }
            return methods;
        }

        private static List<CallMethodInfo> FindMethods(string targetType, INamedTypeSymbol namedTypeSymbol, List<string> messages)
        {
            var methods = GetMethodsRecursive(namedTypeSymbol);
            var callMethodInfos = new List<CallMethodInfo>();

            foreach (var method in methods.Where(m => m.GetAttributes().Any()).ToList())
            {
                var methodDeclaration = method.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as MethodDeclarationSyntax;

                if (methodDeclaration == null)
                {
                    messages.Add($"No MethodDeclarationSyntax for {method.Name}");
                    continue;
                }

                var methodInfo = new CallMethodInfo(targetType, namedTypeSymbol, method, methodDeclaration);
                if (methodInfo.DataMapperMethod != null || methodInfo.DataMapperMethodType != null)
                {
                    callMethodInfos.Add(methodInfo);
                }
                else
                {
                    messages.Add($"No DataMapperMethod or Authorized attribute for {methodInfo.Name}");
                }
            }
            return callMethodInfos;
        }

        private static List<CallMethodInfo> FindAuthMethods(SemanticModel semanticModel, string returnType, INamedTypeSymbol classNamedSymbol, List<string> messages)
        {
            var authorizeAttribute = ClassOrBaseClassHasAttribute(classNamedSymbol, "AuthorizeAttribute");
            var callMethodInfos = new List<CallMethodInfo>();

            if (authorizeAttribute != null)
            {
                ITypeSymbol? authorizationRuleType = authorizeAttribute.AttributeClass?.TypeArguments[0];

                TypeDeclarationSyntax? syntax = authorizationRuleType?.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as TypeDeclarationSyntax;

                if (syntax != null)
                {

                    var authSemanticModel = semanticModel.Compilation.GetSemanticModel(syntax.SyntaxTree);
                    var authSymbol = authSemanticModel.GetDeclaredSymbol(syntax);

                    var methods = GetMethodsRecursive(authSymbol);

                    foreach (var method in methods)
                    {
                        var methodDeclaration = method.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as MethodDeclarationSyntax;

                        if (methodDeclaration == null)
                        {
                            messages.Add($"No MethodDeclarationSyntax for {method.Name}");
                            continue;
                        }

                        var callMethodInfo = new CallMethodInfo(returnType, classNamedSymbol, method, methodDeclaration);

                        if(callMethodInfo.DataMapperMethodType != null)
                        {
                            callMethodInfos.Add(callMethodInfo);
                        }
                        else
                        {
                            messages.Add($"No DataMapperMethodType for {authorizeAttribute}");
                        }
                    }    
                }
                else
                {
                    messages.Add($"No TypeDeclarationSyntax for {authorizeAttribute}");
                }
            }
            else
            {
                messages.Add("No AuthorizeAttribute");
            }

            return callMethodInfos;
        }

        private static void MatchAuthMethods(IEnumerable<FactoryMethod> factoryMethods, List<CallMethodInfo> authCallMethods, List<string> messages)
        {
            foreach (var method in factoryMethods)
            {
                foreach (var authMethod in authCallMethods)
                {
                    var assignAuthMethod = false;

                    if (method.DataMapperMethod != null)
                    {
                        if (((int)authMethod.DataMapperMethodType & (int)method.DataMapperMethod) != 0)
                        {
                            assignAuthMethod = true;
                        }
                    }

                    if (method.DataMapperMethodType != null)
                    {
                        if (((int)authMethod.DataMapperMethodType & (int)method.DataMapperMethodType) != 0)
                        {
                            assignAuthMethod = true;
                        }
                    }

                    var methodParameter = method.Parameters.GetEnumerator();
                    var authMethodParameter = authMethod.Parameters.GetEnumerator();

                    methodParameter.MoveNext();
                    authMethodParameter.MoveNext();

                    // Don't disqualify an auth method we're in a write method and the first parameter is the target
                    // But also accept auth methods that have a first parameter of target
                    if (methodParameter.Current?.IsTarget ?? false)
                    {
                        methodParameter.MoveNext();
                        if (method.IsSave && authMethodParameter.Current != null && authMethodParameter.Current.IsTarget)
                        {
                            authMethodParameter.MoveNext();
                        }
                    }

                    if (authMethodParameter.Current != null)
                    {
                        do
                        {
                            if (authMethodParameter.Current.Type != methodParameter.Current?.Type)
                            {
                                assignAuthMethod = false;
                                break;
                            }
                        } while (authMethodParameter.MoveNext() && methodParameter.MoveNext());
                    }

                    if (assignAuthMethod)
                    {
                        method.AuthCallMethods.Add(authMethod);
                    }
                }
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
    }
}