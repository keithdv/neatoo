using System;

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

        public BaseObjectList(IListBaseServices<BaseObjectList, IBaseObject> services) : base(services) { }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


    }
}
