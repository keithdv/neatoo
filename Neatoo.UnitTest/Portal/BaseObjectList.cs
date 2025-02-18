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

    public BaseObjectList(IListBaseServices<BaseObjectList, IBaseObject> baseServices) : base(baseServices)
    {
    }

    public Guid GuidCriteria { get; set; } = Guid.Empty;
    public int IntCriteria { get; set; } = -1;

    public bool CreateCalled { get; set; } = false;

    [Create]
    public async Task Create()
    {
        CreateCalled = true;
        Add(await ItemPortal.CreateChild());
    }

    [Create]
    public async Task Create(int criteria)
    {
        IntCriteria = criteria;
        Add(await ItemPortal.CreateChild(criteria));
    }

    [Create]
    public async Task Create(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        Add(await ItemPortal.CreateChild(criteria));
    }

    public bool CreateChildCalled { get; set; } = false;

    [CreateChild]
    public async Task CreateChild()
    {
        CreateChildCalled = true;
        Add(await ItemPortal.CreateChild());
    }

    [CreateChild]
    public async Task CreateChild(int criteria)
    {
        IntCriteria = criteria;
        Add(await ItemPortal.CreateChild(criteria));
    }

    [CreateChild]
    public async Task CreateChild(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        Add(await ItemPortal.CreateChild(criteria));
    }

    public bool FetchCalled { get; set; } = false;

    [Fetch]
    public async Task Fetch()
    {
        FetchCalled = true;
        Add(await ItemPortal.FetchChild());
    }

    [Fetch]
    public async Task Fetch(int criteria)
    {
        IntCriteria = criteria;
        Add(await ItemPortal.FetchChild(criteria));
    }

    [Fetch]
    public async Task Fetch(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        Add(await ItemPortal.FetchChild(criteria));
    }

    public bool FetchChildCalled { get; set; } = false;

    [FetchChild]
    public async Task FetchChild()
    {
        FetchChildCalled = true;
        Add(await ItemPortal.FetchChild());
    }

    [FetchChild]
    public async Task FetchChild(int criteria)
    {
        IntCriteria = criteria;
        Add(await ItemPortal.FetchChild(criteria));
    }


    [FetchChild]
    public async Task FetchChild(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        Add(await ItemPortal.FetchChild(criteria));

    }

}
