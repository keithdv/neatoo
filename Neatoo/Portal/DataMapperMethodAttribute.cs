using System;

namespace Neatoo.Portal;

[System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class FactoryAttribute : Attribute
{
    public FactoryAttribute()
    {
    }
}

[System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class SuppressFactoryAttribute : Attribute
{
    public SuppressFactoryAttribute()
    {
    }
}

[System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class FactoryAttribute<T> : Attribute
{
    public FactoryAttribute()
    {
    }
}

[System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class RemoteAttribute : Attribute
{
    public RemoteAttribute()
    {
    }
}

[System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class DataMapperOperation : Attribute
{
    public DataMapperMethod Operation { get; }

    public DataMapperOperation(DataMapperMethod operation)
    {
        this.Operation = operation;
    }
}

[System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
public sealed class ServiceAttribute : Attribute
{
    public ServiceAttribute()
    {
    }
}

public sealed class CreateAttribute : DataMapperOperation
{
    public CreateAttribute() : base(DataMapperMethod.Create)
    {
    }
}

public sealed class FetchAttribute : DataMapperOperation
{
    public FetchAttribute() : base(DataMapperMethod.Fetch)
    {
    }
}

public sealed class InsertAttribute : DataMapperOperation
{
    public InsertAttribute() : base(DataMapperMethod.Insert)
    {
    }
}

public sealed class UpdateAttribute : DataMapperOperation
{
    public UpdateAttribute() : base(DataMapperMethod.Update)
    {
    }
}

public sealed class DeleteAttribute : DataMapperOperation
{
    public DeleteAttribute() : base(DataMapperMethod.Delete)
    {
    }
}

public sealed class  ExecuteAttribute<D> : DataMapperOperation
    where D : Delegate
{
    public ExecuteAttribute() : base(DataMapperMethod.Execute)
    {
    }
}

[System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class LocalAttribute<D> : Attribute where D : Delegate
{
    public LocalAttribute()
    {
    }
}
