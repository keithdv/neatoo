using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo.Portal;
using Neatoo.UnitTest.PersonObjects;
using System.Collections.Generic;
using System;

/*
Debugging Messages:
: PersonEditBase<EditPerson>, IEditPerson
: EditBase<T>, IPersonBase
*/
namespace Neatoo.UnitTest.EditBaseTests
{
    public interface IEditPersonFactory
    {
        Task<IEditPerson> FillFromDto(PersonDto dto);
    }

    [Factory<IEditPerson>]
    internal class EditPersonFactory : FactoryEditBase<EditPerson>, IEditPersonFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly DoRemoteRequest DoRemoteRequest;
        public EditPersonFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public EditPersonFactory(IServiceProvider serviceProvider, DoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public async Task<IEditPerson> FillFromDto(PersonDto dto)
        {
            var target = ServiceProvider.GetRequiredService<EditPerson>();
            await DoMapperMethodCall(target, DataMapperMethod.Fetch, () =>
            {
                target.FillFromDto(dto);
                return Task.CompletedTask;
            });
            return target;
        }
    }
}