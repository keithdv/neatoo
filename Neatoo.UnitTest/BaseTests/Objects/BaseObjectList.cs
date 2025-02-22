using System;

namespace Neatoo.UnitTest.BaseTests.Objects;


public interface IBaseObjectList : IListBase<IBaseObject>
{ 

}
public class BaseObjectList : ListBase<BaseObjectList, IBaseObject>, IBaseObjectList
{

    public BaseObjectList() : base() { }

}
