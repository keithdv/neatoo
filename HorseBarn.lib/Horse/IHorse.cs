using Neatoo;

namespace HorseBarn.lib.Horse
{
    public interface IHorse : IEditBase
    {
        internal int? Id { get; }
        string? Name { get; set; }
        DateOnly? BirthDate { get; set; }
        Breed Breed { get; }

        private static IEnumerable<Breed> LightHorses = [Horse.Breed.QuarterHorse, Horse.Breed.Thoroughbred, Horse.Breed.Mustang];

        private static IEnumerable<Breed> HeavyHorses = [Horse.Breed.Clydesdale, Horse.Breed.Shire];

        internal static bool IsLightHorse(Breed breed)
        {
            return LightHorses.Contains(breed);
        }

        internal static bool IsHeavyHorse(Breed breed)
        {
            return HeavyHorses.Contains(breed);
        }
    }
}