using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal;

public class BaseObject : Base<BaseObject>, IBaseObject
{

    public BaseObject(IBaseServices<BaseObject> baseServices) : base(baseServices)
    {
    }

    public Guid GuidCriteria { get => Getter<Guid>(); set => Setter(value); }
    public int IntCriteria { get => Getter<int>(); set => Setter(value); }
    public object[] MultipleCriteria { get => Getter<object[]>(); set => Setter(value); }

    public bool CreateCalled { get => Getter<bool>(); set => Setter(value); }

    [Create]
    public async Task Create()
    {
        CreateCalled = true;
        await Task.CompletedTask;
    }

    [Create]
    public void CreateInt(int criteria)
    {
        IntCriteria = criteria;
    }

    [Create]
    public void CreateIntString(int i, string s)
    {
        MultipleCriteria = new object[] { i, s };
    }

    [Create]
    public void CreateDependency(int i, double d, [Service] IDisposableDependency dep)
    {
        Assert.IsNotNull(dep);
        MultipleCriteria = new object[] { i, d };
    }

    [Create]
    public void CreateInferType(ICollection collection)
    {
        Assert.IsNotNull(collection);
        CreateCalled = true;
    }

    [Create]
    public void CreateNullCriteria(List<int> a, List<int> b, [Service] IDisposableDependency dep)
    {
        Assert.IsNotNull(dep);
        CreateCalled = true;
        MultipleCriteria = new object[] { a, b };
    }

    [Create]
    public void CreateGuid(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
    }

    public bool CreateChildCalled { get; set; } = false;

    [CreateChild]
    public void CreateChild()
    {
        CreateChildCalled = true;
    }

    [CreateChild]
    public void CreateChild(int criteria)
    {
        IntCriteria = criteria;
    }

    [CreateChild]
    public void CreateChild(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
    }

    public bool FetchCalled { get; set; } = false;

    [Fetch]
    public void Fetch()
    {
        FetchCalled = true;
    }

    [Fetch]
    public void FetchInt(int criteria)
    {
        IntCriteria = criteria;
    }

    
    [Fetch]
    public void FetchGuidDependency(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
    }

    public bool FetchChildCalled { get; set; } = false;

    [FetchChild]
    public void FetchChild()
    {
        FetchChildCalled = true;
    }

    [FetchChild]
    public void FetchChild(int criteria)
    {
        IntCriteria = criteria;
    }

    [FetchChild]
    public void FetchChild(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
    }

}
