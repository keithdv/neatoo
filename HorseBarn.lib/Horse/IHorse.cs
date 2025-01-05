using Neatoo;

namespace HorseBarn.lib.Horse
{
    public interface IHorse : IEditBase
    {
        string? Name { get; set; }
        DateOnly? BirthDate { get; set; }
        Breed Breed { get; }
    }
}