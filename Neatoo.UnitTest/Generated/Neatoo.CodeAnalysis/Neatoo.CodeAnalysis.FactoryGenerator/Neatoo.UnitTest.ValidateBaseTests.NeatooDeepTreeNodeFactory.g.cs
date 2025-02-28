using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
Debugging Messages:
: ValidateBase<NeatooDeepTreeNode>
*/
namespace Neatoo.UnitTest.ValidateBaseTests
{
    public interface INeatooDeepTreeNodeFactory
    {
    }

    internal class NeatooDeepTreeNodeFactory : FactoryBase, INeatooDeepTreeNodeFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        // Delegates
        // Delegate Properties to provide Local or Remote fork in execution
        public NeatooDeepTreeNodeFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public NeatooDeepTreeNodeFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<NeatooDeepTreeNode>();
            services.AddScoped<NeatooDeepTreeNodeFactory>();
            services.AddScoped<INeatooDeepTreeNodeFactory, NeatooDeepTreeNodeFactory>();
        }
    }
}