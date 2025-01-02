using Neatoo;

namespace HorseBarn.lib
{
    public interface IHorseBarn : IEditBase
    {
        Task<IHorse> AddNewHorse(Breed breed);
    }
}