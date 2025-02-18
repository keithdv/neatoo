using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal;

public class EditObjectList : EditListBase<EditObjectList, IEditObject>, IEditObjectList
{

    public EditObjectList(IEditListBaseServices<EditObjectList, IEditObject> baseServices) : base(baseServices)
    {
    }

    public bool CreateCalled { get; set; } = false;

    [Create]
    public void Create()
    {
        CreateCalled = true;
    }

    [Create]
    public void Create(int criteria)
    {
        CreateCalled = true;
    }


    [Create]
    public void Create(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        CreateCalled = true;
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
        CreateChildCalled = true;
    }

    [CreateChild]
    public void CreateChild(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        CreateChildCalled = true;
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
        FetchCalled = true;
    }


    [Fetch]
    public void Fetch(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        FetchCalled = true;
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
        FetchChildCalled = true;
    }

    [FetchChild]
    public void FetchChild(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        FetchChildCalled = true;
    }

    public bool InsertCalled { get; set; } = false;

    [Insert]
    public Task Insert()
    {
        InsertCalled = true;
        return Task.CompletedTask;
    }


    [Insert]
    public Task Insert(int criteria)
    {
        InsertCalled = true;
        return Task.CompletedTask;
    }


    [Insert]
    public Task Insert(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        InsertCalled = true;
        return Task.CompletedTask;
    }

    public bool InsertChildCalled { get; set; } = false;

    [InsertChild]
    public Task InsertChild()
    {
        InsertChildCalled = true;
        return Task.CompletedTask;
    }

    [InsertChild]
    public Task InsertChild(int criteria)
    {
        InsertChildCalled = true;
        return Task.CompletedTask;
    }


    [InsertChild]
    public Task InsertChild(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        InsertChildCalled = true;
        return Task.CompletedTask;
    }

    public bool UpdateCalled { get; set; } = false;

    protected override async Task Update()
    {
        UpdateCalled = true;
        await UpdateList();
    }

    [Update]
    public Task Update(int criteria)
    {
        UpdateCalled = true;
        return Task.CompletedTask;
    }


    [Update]
    public Task Update(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        UpdateCalled = true;
        return Task.CompletedTask;
    }

    public bool UpdateChildCalled { get; set; } = false;

    [UpdateChild]
    public Task UpdateChild()
    {
        UpdateChildCalled = true;
        return Task.CompletedTask;
    }

    [UpdateChild]
    public Task UpdateChild(int criteria)
    {
        UpdateChildCalled = true;
        return Task.CompletedTask;
    }


    [UpdateChild]
    public Task UpdateChild(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        UpdateChildCalled = true;
        return Task.CompletedTask;
    }

    public bool DeleteCalled { get; set; } = false;

    [Delete]
    public Task Delete_()
    {
        DeleteCalled = true;
        return Task.CompletedTask;
    }

    [Delete]
    public Task Delete(int criteria)
    {
        DeleteCalled = true;
        return Task.CompletedTask;
    }

    [Delete]
    public Task Delete(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        DeleteCalled = true;
        return Task.CompletedTask;
    }

    public bool DeleteChildCalled { get; set; } = false;

    [DeleteChild]
    public Task DeleteChild()
    {
        DeleteChildCalled = true;
        return Task.CompletedTask;
    }

    [DeleteChild]
    public Task DeleteChild(int criteria)
    {
        DeleteChildCalled = true;
        return Task.CompletedTask;
    }

    [DeleteChild]
    public Task DeleteChild(Guid criteria, IDisposableDependency dependency)
    {
        Assert.IsNotNull(dependency);
        DeleteChildCalled = true;
        return Task.CompletedTask;
    }
}
