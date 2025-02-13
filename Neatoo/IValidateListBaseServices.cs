using Neatoo.Portal;

namespace Neatoo;

/// <summary>
/// Wrap the NeatooBase services into an interface so that 
/// the inheriting classes don't need to list all services
/// and services can be added
/// </summary>
public interface IValidateListBaseServices<T, I> : IListBaseServices<T, I>
    where T : ValidateListBase<T, I>
    where I : IValidateBase
{
}



public class ValidateListBaseServices<T, I> : ListBaseServices<T, I>, IValidateListBaseServices<T, I>
    where T : ValidateListBase<T, I>
    where I : IValidateBase
{

    public ValidateListBaseServices(IReadPortalChild<I> portal) : base(portal)
    {
    }
}
