using Neatoo.Portal;

namespace Neatoo;

/// <summary>
/// Wrap the NeatooBase services into an interface so that 
/// the inheriting classes don't need to list all services
/// and services can be added
/// </summary>
public interface IEditListBaseServices<T, I> : IValidateListBaseServices<T, I>
    where T : EditListBase<T, I>
    where I : IEditBase
{
    IReadWritePortalChild<I> ReadWritePortalChild { get; }
}

public class EditListBaseServices<T, I> : ValidateListBaseServices<T, I>, IEditListBaseServices<T, I>
    where T : EditListBase<T, I>
    where I : IEditBase
{

    public IReadWritePortalChild<I> ReadWritePortalChild { get; }

    public EditListBaseServices(IReadWritePortalChild<I> readWritePortalChild)
        : base(readWritePortalChild)
    {
        ReadWritePortalChild = readWritePortalChild;
    }
}
