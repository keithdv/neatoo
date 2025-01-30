using Neatoo.Portal;
using Neatoo.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.EditBaseTests
{

    public interface IEditPersonList : IEditListBase<IEditPerson>
    {
        int DeletedCount { get; }

    }

    public class EditPersonList : EditListBase<EditPersonList, IEditPerson>, IEditPersonList
    {
        public EditPersonList(IEditListBaseServices<EditPersonList, IEditPerson> services) : base(services)
        {
        }

        public int DeletedCount => DeletedList.Count;
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ShortName { get; set; }

        public string Title { get; set; }

        public string FullName { get; set; }

        public uint? Age { get; set; }

        public void FillFromDto(PersonDto dto)
        {
            Id = dto.PersonId;
            // These will not mark IsModified to true
            // as long as within ObjectPortal operation
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Title = dto.Title;
        }

        [Fetch]
        private async Task FillFromDto(PersonDto dto, IReadOnlyList<PersonDto> personTable)
        {
            Id = dto.PersonId;
            // These will not mark IsModified to true
            // as long as within ObjectPortal operation
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Title = dto.Title;

            var children = personTable.Where(p => p.FatherId == Id);

            foreach (var child in children)
            {
                Add(await ItemPortal.FetchChild(child));
            }
        }

    }
}
