using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
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
        IEditPerson FillFromDto(PersonDto dto);
        delegate IEditPerson FillFromDtoDelegate(PersonDto dto);
    }

    internal class EditPersonFactory : FactoryEditBase<EditPerson>, IEditPersonFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public EditPersonFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public EditPersonFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate) : this(serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public IEditPerson FillFromDto(PersonDto dto)
        {
            var target = ServiceProvider.GetRequiredService<EditPerson>();
            return DoMapperMethodCall<IEditPerson>(target, DataMapperMethod.Fetch, () => target.FillFromDto(dto));
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<EditPerson>();
            services.AddScoped<EditPersonFactory>();
            services.AddScoped<IEditPersonFactory, EditPersonFactory>();
            services.AddScoped<IEditPerson, EditPerson>();
            services.AddScoped<IEditPersonFactory.FillFromDtoDelegate>(cc =>
            {
                var factory = cc.GetRequiredService<EditPersonFactory>();
                return (PersonDto dto) => factory.FillFromDto(dto);
            });
            services.AddScoped<IFactoryEditBase<EditPerson>, EditPersonFactory>();
        }
    }
}