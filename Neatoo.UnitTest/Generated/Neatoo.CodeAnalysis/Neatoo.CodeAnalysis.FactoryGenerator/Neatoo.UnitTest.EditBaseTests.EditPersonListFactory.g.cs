﻿using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Neatoo.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
Debugging Messages:
: EditListBase<EditPersonList, IEditPerson>, IEditPersonList
*/
namespace Neatoo.UnitTest.EditBaseTests
{
    public interface IEditPersonListFactory
    {
    }

    internal class EditPersonListFactory : FactoryBase, IEditPersonListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public EditPersonListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public EditPersonListFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<EditPersonList>();
            services.AddTransient<IEditPersonList, EditPersonList>();
            services.AddScoped<EditPersonListFactory>();
            services.AddScoped<IEditPersonListFactory, EditPersonListFactory>();
        }
    }
}