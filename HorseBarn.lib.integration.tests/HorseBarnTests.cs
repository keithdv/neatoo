using Autofac;
using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Neatoo.Portal;

namespace HorseBarn.lib.integration.tests
{
    [TestClass]
    public sealed class HorseBarnTests
    {
        private ILifetimeScope scope;
        private ISendReceivePortal<IHorseBarn> portal;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            portal = scope.Resolve<ISendReceivePortal<IHorseBarn>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            scope.Dispose();
        }

        [TestMethod]
        public async Task HorseBarn_Create()
        {
            var horseBarn = await portal.Create();

            horseBarn.Pasture.Name = "Pasture";

            var heavyHorse = (IHeavyHorse) await horseBarn.AddNewHorse(Breed.Clydesdale);

            Assert.IsFalse(horseBarn.IsValid);

            heavyHorse.Name = "Heavy Horse A";

            Assert.IsInstanceOfType<IHeavyHorse>(heavyHorse);

            var lightHorse = (ILightHorse) await horseBarn.AddNewHorse(Breed.Thoroughbred);

            lightHorse.Name = "Light Horse B";

            Assert.IsInstanceOfType<ILightHorse>(lightHorse);

            Assert.IsTrue(horseBarn.IsValid);

            var racingChariot = await horseBarn.AddRacingCart();

            var wagon = await horseBarn.AddWagon();

            Assert.IsFalse(horseBarn.IsValid);

            racingChariot.Name = "Racing Chariot";
            wagon.Name = "Wagon";

            Assert.IsTrue(horseBarn.IsValid);

            wagon.NumberOfHorses = 2;

            // Key: Cannot add an ILightHorse to the wagon 
            // No validation, no if statements
            await horseBarn.MoveHorseToCart(heavyHorse, wagon);

            Assert.IsFalse(horseBarn.IsValid);

            heavyHorse = (IHeavyHorse) await horseBarn.AddNewHorse(Breed.Clydesdale);

            heavyHorse.Name = "Heavy Horse B";

            await horseBarn.MoveHorseToCart(heavyHorse, wagon);

            Assert.IsTrue(horseBarn.IsValid);
        }
    }
}
