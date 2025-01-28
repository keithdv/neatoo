using System;
using System.Collections.Generic;
using System.Text;

namespace Neatoo.UnitTest.BaseTests
{

    public interface IBaseObjectList : IListBase<IBaseObject>
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }

    }
    public class BaseObjectList : ListBase<BaseObjectList, IBaseObject>, IBaseObjectList
    {

        public BaseObjectList(ListBaseServices<BaseObjectList, IBaseObject> services) : base(services) { }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


    }
}
