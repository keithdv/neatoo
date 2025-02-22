using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal;

public class EditObjectList : EditListBase<EditObjectList, IEditObject>, IEditObjectList
{

    public EditObjectList() : base()
    {
    }

}
