﻿using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Core;
using System;

/*
Debugging Messages:
: ValidateBase<SimpleValidateObject>, ISimpleValidateObject
*/
namespace Neatoo.UnitTest.Example.SimpleValidate
{
    public interface ISimpleValidateObjectFactory
    {
    }

    internal class SimpleValidateObjectFactory : FactoryBase, ISimpleValidateObjectFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public SimpleValidateObjectFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public SimpleValidateObjectFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<SimpleValidateObject>();
            services.AddScoped<SimpleValidateObjectFactory>();
            services.AddScoped<ISimpleValidateObjectFactory, SimpleValidateObjectFactory>();
            services.AddTransient<ISimpleValidateObject, SimpleValidateObject>();
        }
    }
}