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
        public async Task HorseBarn_FullRun()
        {
            var horseBarn = await portal.Create();
            
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

        [TestMethod]
        public async Task HorseBarn_Create()
        {
            var horseBarn = await portal.Create();

            Assert.IsNotNull(horseBarn.Pasture);
            Assert.IsNotNull(horseBarn.Carts);
        }

        [TestMethod]
        public async Task HorseBarn_AddRacingCart()
        {
            var horseBarn = await portal.Create();
            var racingChariot = await horseBarn.AddRacingCart();

            Assert.IsNotNull(racingChariot);
            Assert.IsTrue(horseBarn.Carts.Contains(racingChariot));
        }

        [TestMethod]
        public async Task HorseBarn_AddWagon()
        {
            var horseBarn = await portal.Create();
            var wagon = await horseBarn.AddWagon();

            Assert.IsNotNull(wagon);
            Assert.IsTrue(horseBarn.Carts.Contains(wagon));
        }

        [TestMethod]
        public async Task HorseBarn_AddNewHorse_LightHorse()
        {
            var horseBarn = await portal.Create();
            var lightHorse = await horseBarn.AddNewHorse(Breed.Thoroughbred);

            Assert.IsNotNull(lightHorse);
            Assert.IsInstanceOfType<ILightHorse>(lightHorse);
            Assert.IsTrue(horseBarn.Pasture.Horses.Contains(lightHorse));
        }

        [TestMethod]
        public async Task HorseBarn_AddNewHorse_HeavyHorse()
        {
            var horseBarn = await portal.Create();
            var heavyHorse = await horseBarn.AddNewHorse(Breed.Clydesdale);

            Assert.IsNotNull(heavyHorse);
            Assert.IsInstanceOfType<IHeavyHorse>(heavyHorse);
            Assert.IsTrue(horseBarn.Pasture.Horses.Contains(heavyHorse));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task HorseBarn_AddNewHorse_InvalidBreed()
        {
            var horseBarn = await portal.Create();
            await horseBarn.AddNewHorse((Breed)999); // Invalid breed
        }

        [TestMethod]
        public async Task HorseBarn_MoveHorseToCart()
        {
            var horseBarn = await portal.Create();
            var heavyHorse = (IHeavyHorse)await horseBarn.AddNewHorse(Breed.Clydesdale);
            var wagon = await horseBarn.AddWagon();

            await horseBarn.MoveHorseToCart(heavyHorse, wagon);

            Assert.IsFalse(horseBarn.Pasture.Horses.Contains(heavyHorse));
            Assert.IsTrue(wagon.Horses.Contains(heavyHorse));
        }
    }
}
