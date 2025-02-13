using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;

namespace Neatoo.UnitTest.ObjectPortal;

public class EditObject : EditBase<EditObject>, IEditObject
{

    public EditObject(EditBaseServices<EditObject> baseServices) : base(baseServices)
    {

    }

    public Guid? ID { get => Getter<Guid?>(); set => Setter(value); }
    public Guid GuidCriteria { get => Getter<Guid>(); set => Setter(value); }
    public int IntCriteria { get => Getter<int>(); set => Setter(value); }

    public bool CreateCalled { get; set; } = false;

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
    public void Create(int criteria)
    {
        IntCriteria = criteria;
        CreateCalled = true;
    }

    [Create]
    public void Create(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        CreateCalled = true;
    }

    public bool CreateChildCalled { get; set; } = false;

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
    public void CreateChild(Guid criteria, IDisposableDependency dependency)
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
    public void Fetch(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        FetchCalled = true;
    }

    public bool FetchChildCalled { get; set; } = false;

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
    public void FetchChild(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        FetchChildCalled = true;
    }

    public bool InsertCalled { get; set; } = false;

    [Insert]
    public void Insert()
    {
        ID = Guid.NewGuid();
        InsertCalled = true;
    }

    [Insert]
    public void Insert(int criteria)
    {
        InsertCalled = true;
        IntCriteria = criteria;
    }

    [Insert]
    public void Insert(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        InsertCalled = true;
        GuidCriteria = criteria;
    }

    public bool InsertChildCalled { get; set; } = false;

    [InsertChild]
    public void InsertChild()
    {
        ID = Guid.NewGuid();
        InsertChildCalled = true;
    }

    [InsertChild]
    public void InsertChild(int criteria)
    {
        IntCriteria = criteria;
        InsertChildCalled = true;
    }

    [InsertChild]
    public void InsertChild(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        InsertChildCalled = true;
    }

    public bool UpdateCalled { get; set; } = false;

    [Update]
    public void Update()
    {
        ID = Guid.NewGuid();
        UpdateCalled = true;
    }


    [Update]
    public void Update(int criteria)
    {
        IntCriteria = criteria;
        UpdateCalled = true;
    }


    [Update]
    public void Update(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        UpdateCalled = true;
    }

    public bool UpdateChildCalled { get; set; } = false;

    [UpdateChild]
    public void UpdateChild()
    {
        ID = Guid.NewGuid();
        UpdateChildCalled = true;
    }

    [UpdateChild]
    public void UpdateChild(int criteria)
    {
        IntCriteria = criteria;
        UpdateChildCalled = true;
    }


    [UpdateChild]
    public void UpdateChild(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        UpdateChildCalled = true;
    }

    public bool DeleteCalled { get; set; } = false;

    [Delete]
    public void Delete_()
    {
        DeleteCalled = true;
    }

    [Delete]
    public void Delete(int criteria)
    {
        IntCriteria = criteria;
        DeleteCalled = true;
    }

    [Delete]
    public void Delete(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        DeleteCalled = true;
    }

    public bool DeleteChildCalled { get; set; } = false;

    [DeleteChild]
    public void DeleteChild()
    {
        DeleteChildCalled = true;
    }

    [DeleteChild]
    public void DeleteChild(int criteria)
    {
        IntCriteria = criteria;
        DeleteChildCalled = true;
    }

    [DeleteChild]
    public void DeleteChild(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        GuidCriteria = criteria;
        DeleteChildCalled = true;
    }
}
