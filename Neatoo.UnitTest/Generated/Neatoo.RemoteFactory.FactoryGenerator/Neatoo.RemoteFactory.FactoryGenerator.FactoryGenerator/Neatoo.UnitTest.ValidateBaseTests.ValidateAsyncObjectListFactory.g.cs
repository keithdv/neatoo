﻿#nullable enable
using Neatoo.RemoteFactory;
using Neatoo.RemoteFactory.Internal;
using Microsoft.Extensions.DependencyInjection;
using Neatoo.Core;
using Neatoo.UnitTest.PersonObjects;

/*
Interface Found. TargetType: IValidateAsyncObjectList ConcreteType: ValidateAsyncObjectList
Class: Neatoo.UnitTest.ValidateBaseTests.ValidateAsyncObjectList Name: ValidateAsyncObjectList
No MethodDeclarationSyntax for get_IsPaused
No MethodDeclarationSyntax for set_IsPaused
No MethodDeclarationSyntax for get_MetaState
No MethodDeclarationSyntax for HandleNeatooPropertyChanged
No MethodDeclarationSyntax for RunAllRules
No MethodDeclarationSyntax for RunAllRules
No MethodDeclarationSyntax for RunAllRules
No MethodDeclarationSyntax for RunSelfRules
No MethodDeclarationSyntax for get_Parent
No MethodDeclarationSyntax for get_Parent
No MethodDeclarationSyntax for set_Parent
No MethodDeclarationSyntax for set_Parent
No MethodDeclarationSyntax for add_NeatooPropertyChanged
No MethodDeclarationSyntax for add_NeatooPropertyChanged
No MethodDeclarationSyntax for remove_NeatooPropertyChanged
No MethodDeclarationSyntax for remove_NeatooPropertyChanged
No MethodDeclarationSyntax for Neatoo.Core.ISetParent.SetParent
No MethodDeclarationSyntax for InsertItem
No MethodDeclarationSyntax for PostPortalConstruct
No MethodDeclarationSyntax for RaiseNeatooPropertyChanged
No MethodDeclarationSyntax for HandleNeatooPropertyChanged
No MethodDeclarationSyntax for HandlePropertyChanged
No MethodDeclarationSyntax for WaitForTasks
No MethodDeclarationSyntax for WaitForTasks
No MethodDeclarationSyntax for WaitForTasks
No MethodDeclarationSyntax for .ctor
No MethodDeclarationSyntax for .ctor
No MethodDeclarationSyntax for BlockReentrancy
No MethodDeclarationSyntax for InsertItem
No MethodDeclarationSyntax for OnCollectionChanged
No MethodDeclarationSyntax for OnPropertyChanged
No MethodDeclarationSyntax for SetItem
No MethodDeclarationSyntax for GetType
No MethodDeclarationSyntax for MemberwiseClone
No AuthorizeAttribute

*/
namespace Neatoo.UnitTest.ValidateBaseTests
{
    public interface IValidateAsyncObjectListFactory
    {
    }

    internal class ValidateAsyncObjectListFactory : FactoryBase<IValidateAsyncObjectList>, IValidateAsyncObjectListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IMakeRemoteDelegateRequest? MakeRemoteDelegateRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public ValidateAsyncObjectListFactory(IServiceProvider serviceProvider, IFactoryCore<IValidateAsyncObjectList> factoryCore) : base(factoryCore)
        {
            this.ServiceProvider = serviceProvider;
        }

        public ValidateAsyncObjectListFactory(IServiceProvider serviceProvider, IMakeRemoteDelegateRequest remoteMethodDelegate, IFactoryCore<IValidateAsyncObjectList> factoryCore) : base(factoryCore)
        {
            this.ServiceProvider = serviceProvider;
            this.MakeRemoteDelegateRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddScoped<ValidateAsyncObjectListFactory>();
            services.AddScoped<IValidateAsyncObjectListFactory, ValidateAsyncObjectListFactory>();
        }
    }
}