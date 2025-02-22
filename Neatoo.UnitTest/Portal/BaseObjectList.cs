using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal;

/// <summary>
/// I don't know that this is really neccessary
/// Testing that the portal logic works on Base<> should be enough
/// </summary>
public class BaseObjectList : ListBase<BaseObjectList, IBaseObject>, IBaseObjectList
{

    public BaseObjectList() : base()
    {
    }
}
