
using Neatoo;

namespace HorseBarn.lib
{
    public interface IHorse : IEditBase
    {
        Guid? Id { get; }
        string Name { get; set; }
        Breed Breed { get; }
    }
}