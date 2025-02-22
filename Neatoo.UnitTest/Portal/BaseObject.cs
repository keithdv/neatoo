using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal;

public interface IBaseObject : IBase
{
    int IntCriteria { get; }
    Guid GuidCriteria { get; }
    object[] MultipleCriteria { get; }
    bool CreateCalled { get; set; }
    bool FetchCalled { get; set; }
}

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
    public async Task CreateAsync()
    {
        CreateCalled = true;
        await Task.CompletedTask;
    }

    [Create]
    public void Create(int criteria)
    {
        IntCriteria = criteria;
    }

    [Create]
    public void Create(int i, string s)
    {
        MultipleCriteria = new object[] { i, s };
    }

    [Create]
    public void Create(int i, double d, [Service] IDisposableDependency dep)
    {
        Assert.IsNotNull(dep);
        MultipleCriteria = new object[] { i, d };
    }

    [Create]
    public Task Create(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        return Task.CompletedTask;
    }

    public bool FetchCalled { get; set; } = false;

    [Fetch]
    public void Fetch()
    {
        FetchCalled = true;
    }

    [Fetch]
    public void Fetch(int criteria)
    {
        IntCriteria = criteria;
    }
    
    [Fetch]
    public Task Fetch(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        return Task.CompletedTask;
    }
}
