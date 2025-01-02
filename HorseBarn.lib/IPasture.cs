
using Neatoo;

namespace HorseBarn.lib
{
    public interface IPasture : IEditBase
    {
        Guid? Id { get; }
        string Name { get; set; }

        IHorseList Horses { get; }
    }
}