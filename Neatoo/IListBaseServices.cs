using Neatoo.Portal;

namespace Neatoo;

/// <summary>
/// Wrap the NeatooBase services into an interface so that 
/// the inheriting classes don't need to list all services
/// and services can be added
/// </summary>
public interface IListBaseServices<T, I>
    where T : ListBase<T, I>
    where I : IBase
{
    IReadPortalChild<I> ReadPortal { get; }
}

public class ListBaseServices<T, I> : IListBaseServices<T, I>
    where T : ListBase<T, I>
    where I : IBase 
{

    public ListBaseServices()        {        }

    public ListBaseServices(IReadPortalChild<I> readPortal)
    {
        ReadPortal = readPortal;
    }

    public IReadPortalChild<I> ReadPortal { get; }
}
