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
public class DataMapperMethodAttribute : Attribute
{
    public DataMapperMethod Operation { get; }

    public DataMapperMethodAttribute(DataMapperMethod operation)
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

public sealed class CreateAttribute : DataMapperMethodAttribute
{
    public CreateAttribute() : base(DataMapperMethod.Create)
    {
    }
}

public sealed class FetchAttribute : DataMapperMethodAttribute
{
    public FetchAttribute() : base(DataMapperMethod.Fetch)
    {
    }
}

public sealed class InsertAttribute : DataMapperMethodAttribute
{
    public InsertAttribute() : base(DataMapperMethod.Insert)
    {
    }
}

public sealed class UpdateAttribute : DataMapperMethodAttribute
{
    public UpdateAttribute() : base(DataMapperMethod.Update)
    {
    }
}

public sealed class DeleteAttribute : DataMapperMethodAttribute
{
    public DeleteAttribute() : base(DataMapperMethod.Delete)
    {
    }
}

public sealed class  ExecuteAttribute<D> : DataMapperMethodAttribute
    where D : Delegate
{
    public ExecuteAttribute() : base(DataMapperMethod.Execute)
    {
    }
}

