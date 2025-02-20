using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using System.Diagnostics;
using System.ComponentModel;

/*
Debugging Messages:
: CustomEditBase<Pasture>, IPasture
: EditBase<T>
*/
namespace HorseBarn.lib
{
    public interface IPastureFactory
    {
    }

    [Factory<IPasture>]
    internal class PastureFactory : FactoryEditBase<Pasture>, IPastureFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        public PastureFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public PastureFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }
    }
}