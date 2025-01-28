using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Neatoo.Netwonsoft.Json.Test.BaseTests
{
    public interface IBaseObject : IBase
    {
        Guid ID { get; set; }
        string Name { get; set; }

        IBaseObject Child { get; set; }
    }

    public class BaseObject : Base<BaseObject>, IBaseObject
    {
        public BaseObject(BaseServices<BaseObject> services) : base(services)
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

    public class BaseObjectList : ListBase<IBaseObject>, IBaseObjectList
    {
        public BaseObjectList(ListBaseServices<IBaseObject> services) : base(services)
        {
        }

    }
}
