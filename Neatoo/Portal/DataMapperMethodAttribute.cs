using System;

namespace Neatoo.Portal;


[System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class DataMapperMethodAttribute : Attribute
{
    public DataMapperMethod Operation { get; }

    public DataMapperMethodAttribute(DataMapperMethod operation)
    {
        this.Operation = operation;
    }
}

public sealed class CreateAttribute : DataMapperMethodAttribute
{
    public CreateAttribute() : base(DataMapperMethod.Create)
    {
    }
}

public sealed class CreateChildAttribute : DataMapperMethodAttribute
{
    public CreateChildAttribute() : base(DataMapperMethod.CreateChild)
    {
    }
}

public sealed class FetchAttribute : DataMapperMethodAttribute
{
    public FetchAttribute() : base(DataMapperMethod.Fetch)
    {
    }
}

public sealed class FetchChildAttribute : DataMapperMethodAttribute
{
    public FetchChildAttribute() : base(DataMapperMethod.FetchChild)
    {
    }
}
public sealed class InsertAttribute : DataMapperMethodAttribute
{
    public InsertAttribute() : base(DataMapperMethod.Insert)
    {
    }
}

public sealed class InsertChildAttribute : DataMapperMethodAttribute
{    public InsertChildAttribute() : base(DataMapperMethod.InsertChild)
    {
    }
}


public sealed class UpdateAttribute : DataMapperMethodAttribute
{
    public UpdateAttribute() : base(DataMapperMethod.Update)
    {
    }
}

public sealed class UpdateChildAttribute : DataMapperMethodAttribute
{
    public UpdateChildAttribute() : base(DataMapperMethod.UpdateChild)
    {
    }
}

public sealed class DeleteAttribute : DataMapperMethodAttribute
{
    public DeleteAttribute() : base(DataMapperMethod.Delete)
    {
    }
}

public sealed class DeleteChildAttribute : DataMapperMethodAttribute
{
    public DeleteChildAttribute() : base(DataMapperMethod.DeleteChild)
    {
    }
}
