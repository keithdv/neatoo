using Neatoo.Portal;
using Neatoo.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.EditBaseTests;


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

}
