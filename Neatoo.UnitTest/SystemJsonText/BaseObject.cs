using System;

namespace Neatoo.UnitTest.SystemTextJson.BaseTests
{
    public interface IBaseObject : IBase
    {
        Guid ID { get; set; }
        string Name { get; set; }

        IBaseObject Child { get; set; }
    }

    public class BaseObject : Base<BaseObject>, IBaseObject
    {
        public BaseObject(IBaseServices<BaseObject> services) : base(services)
        {
        }

        public Guid ID { get => Getter<Guid>(); set => Setter(value); }
        public string Name { get => Getter<string>(); set => Setter(value); }
        public IBaseObject Child { get => Getter<IBaseObject>(); set => Setter(value); }

    }

    public interface IBaseObjectList : IListBase<IBaseObject>
    {
        void Add(IBaseObject obj);

    }

    public class BaseObjectList : ListBase<BaseObjectList, IBaseObject>, IBaseObjectList
    {
        public BaseObjectList(IListBaseServices<BaseObjectList, IBaseObject> services) : base(services)
        {
        }

    }
}
