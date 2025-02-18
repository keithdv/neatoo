using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;

namespace Neatoo.UnitTest.ObjectPortal;

public class EditObject : EditBase<EditObject>, IEditObject
{

    public EditObject(IEditBaseServices<EditObject> baseServices) : base(baseServices)
    {

    }

    public Guid? ID { get => Getter<Guid?>(); set => Setter(value); }
    public Guid GuidCriteria { get => Getter<Guid>(); set => Setter(value); }
    public int IntCriteria { get => Getter<int>(); set => Setter(value); }
    public object[] MultipleCriteria { get => Getter<object[]>(); set => Setter(value); }

    public bool CreateCalled { get => Getter<bool>(); set => Setter(value); }

    void IEditObject.MarkAsChild()
    {
        this.MarkAsChild();
    }

    void IEditObject.MarkDeleted()
    {
        this.MarkDeleted();
    }

    void IEditObject.MarkNew()
    {
        this.MarkNew();
    }

    void IEditObject.MarkOld()
    {
        this.MarkOld();
    }

    void IEditObject.MarkUnmodified()
    {
        this.MarkUnmodified();
    }

    [Create]
    public void Create()
    {
        ID = Guid.NewGuid();
        CreateCalled = true;
    }

    [Create]
    public void CreateInt(int criteria)
    {
        IntCriteria = criteria;
        CreateCalled = true;
    }


    [Create]
    public void CreateDependency(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        CreateCalled = true;
    }

    public bool CreateChildCalled { get => Getter<bool>(); set => Setter(value); }

    [CreateChild]
    public void CreateChild()
    {
        ID = Guid.NewGuid();
        CreateChildCalled = true;
    }

    [CreateChild]
    public void CreateChild(int criteria)
    {
        IntCriteria = criteria;
        CreateChildCalled = true;
    }
    
    [CreateChild]
    public void CreateChild(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        CreateChildCalled = true;
    }

    public bool FetchCalled { get; set; } = false;

    [Fetch]
    public void Fetch()
    {
        ID = Guid.NewGuid();
        FetchCalled = true;
    }

    [Fetch]
    public void Fetch(int criteria)
    {
        IntCriteria = criteria;
        FetchCalled = true;
    }

    [Fetch]
    public void FetchGuidDependency(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        FetchCalled = true;
    }

    public bool FetchChildCalled { get => Getter<bool>(); set => Setter(value); }

    [FetchChild]
    public void FetchChild()
    {
        ID = Guid.NewGuid();
        FetchChildCalled = true;
    }

    [FetchChild]
    public void FetchChild(int criteria)
    {
        IntCriteria = criteria;
        FetchChildCalled = true;
    }

    [FetchChild]
    public void FetchChild(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        FetchChildCalled = true;
    }

    public bool InsertCalled { get => Getter<bool>(); set => Setter(value); }

    [Insert]
    public Task Insert()
    {
        ID = Guid.NewGuid();
        InsertCalled = true;
        return Task.CompletedTask;
    }

    [Insert]
    public Task Insert(int criteria)
    {
        InsertCalled = true;
        IntCriteria = criteria;
        return Task.CompletedTask;
    }

    [Insert]
    public Task Insert(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        InsertCalled = true;
        GuidCriteria = criteria;
        return Task.CompletedTask;
    }

    public bool InsertChildCalled { get => Getter<bool>(); set => Setter(value); }

    [InsertChild]
    public Task InsertChild()
    {
        ID = Guid.NewGuid();
        InsertChildCalled = true;
        return Task.CompletedTask;
    }

    [InsertChild]
    public Task InsertChild(int criteria)
    {
        IntCriteria = criteria;
        InsertChildCalled = true;
        return Task.CompletedTask;
    }

    [InsertChild]
    public Task InsertChild(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        InsertChildCalled = true;
        return Task.CompletedTask;
    }

    public bool UpdateCalled { get => Getter<bool>(); set => Setter(value); }

    [Update]
    public Task Update()
    {
        ID = Guid.NewGuid();
        UpdateCalled = true;
        return Task.CompletedTask;
    }


    [Update]
    public Task Update(int criteria)
    {
        IntCriteria = criteria;
        UpdateCalled = true;
        return Task.CompletedTask;
    }


    [Update]
    public Task Update(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        UpdateCalled = true;
        return Task.CompletedTask;
    }

    public bool UpdateChildCalled { get => Getter<bool>(); set => Setter(value); }

    [UpdateChild]
    public Task UpdateChild()
    {
        ID = Guid.NewGuid();
        UpdateChildCalled = true;
        return Task.CompletedTask;
    }

    [UpdateChild]
    public Task UpdateChild(int criteria)
    {
        IntCriteria = criteria;
        UpdateChildCalled = true;
        return Task.CompletedTask;
    }


    [UpdateChild]
    public Task UpdateChild(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        UpdateChildCalled = true;
        return Task.CompletedTask;
    }

    public bool DeleteCalled { get => Getter<bool>(); set => Setter(value); }

    [Delete]
    public Task Delete()
    {
        DeleteCalled = true;
        return Task.CompletedTask;
    }

    [Delete]
    public Task Delete(int criteria)
    {
        IntCriteria = criteria;
        DeleteCalled = true;
        return Task.CompletedTask;
    }

    [Delete]
    public Task Delete(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        DeleteCalled = true;
        return Task.CompletedTask;
    }

    public bool DeleteChildCalled { get => Getter<bool>(); set => Setter(value); }

    [DeleteChild]
    public Task DeleteChild()
    {
        DeleteChildCalled = true;
        return Task.CompletedTask;
    }

    [DeleteChild]
    public Task DeleteChild(int criteria)
    {
        IntCriteria = criteria;
        DeleteChildCalled = true;
        return Task.CompletedTask;
    }

    [DeleteChild]
    public Task DeleteChild(Guid criteria, [Service] IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        DeleteChildCalled = true;
        return Task.CompletedTask;
    }
}
