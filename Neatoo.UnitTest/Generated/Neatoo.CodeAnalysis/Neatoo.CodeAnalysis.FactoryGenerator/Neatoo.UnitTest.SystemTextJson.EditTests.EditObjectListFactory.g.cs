using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo.Portal;
using System;

/*
Debugging Messages:
: EditListBase<EditObjectList, IEditObject>, IEditObjectList
*/
namespace Neatoo.UnitTest.SystemTextJson.EditTests
{
    public interface IEditObjectListFactory
    {
    }

    [Factory<IEditObjectList>]
    internal class EditObjectListFactory : FactoryBase<EditObjectList>, IEditObjectListFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        public EditObjectListFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public EditObjectListFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }
    }
}