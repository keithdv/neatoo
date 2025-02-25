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
No DataMapperMethod attribute for MarkAsChild
No DataMapperMethod attribute for MarkDeleted
No DataMapperMethod attribute for MarkNew
No DataMapperMethod attribute for MarkOld
No DataMapperMethod attribute for MarkUnmodified
: EditBase<T>, IPersonBase
*/
namespace Neatoo.UnitTest.EditBaseTests
{
    public interface IEditPersonFactory
    {
        IEditPerson FillFromDto(PersonDto dto);
        IEditPerson? Save(IEditPerson target);
    }

    internal class EditPersonFactory : FactoryEditBase<EditPerson>, IFactoryEditBase<EditPerson>, IEditPersonFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public EditPersonFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public EditPersonFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
        }

        public IEditPerson FillFromDto(PersonDto dto)
        {
            var target = ServiceProvider.GetRequiredService<EditPerson>();
            return DoMapperMethodCall<IEditPerson>(target, DataMapperMethod.Fetch, () => target.FillFromDto(dto));
        }

        public virtual IEditPerson? LocalInsert(IEditPerson itarget)
        {
            var target = (EditPerson)itarget ?? throw new Exception("EditPerson must implement IEditPerson");
            return DoMapperMethodCall<IEditPerson>(target, DataMapperMethod.Insert, () => target.Insert());
        }

        async Task<IEditBase?> IFactoryEditBase<EditPerson>.Save(EditPerson target)
        {
            return await Task.FromResult((IEditBase? )Save(target));
        }

        public virtual IEditPerson? Save(IEditPerson target)
        {
            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return LocalInsert(target);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<EditPerson>();
            services.AddScoped<EditPersonFactory>();
            services.AddScoped<IEditPersonFactory, EditPersonFactory>();
            services.AddTransient<IEditPerson, EditPerson>();
            services.AddScoped<IFactoryEditBase<EditPerson>, EditPersonFactory>();
        }
    }
}