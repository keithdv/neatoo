using Autofac;
using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Moq;
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
            
            var criteria = Mock.Of<IHorseCriteria>();

            criteria.Name = "Heavy Horse A";
            criteria.Breed = Breed.Clydesdale;
            criteria.BirthDay = DateOnly.FromDateTime(DateTime.Now);

            var heavyHorse = (IHeavyHorse) await horseBarn.AddNewHorse(criteria);

            Assert.IsFalse(horseBarn.IsValid);

            Assert.IsInstanceOfType<IHeavyHorse>(heavyHorse);

            criteria.Name = "Light Horse B"; 
            criteria.Breed = Breed.Thoroughbred;

            var lightHorse = (ILightHorse) await horseBarn.AddNewHorse(criteria);

            Assert.IsInstanceOfType<ILightHorse>(lightHorse);

            Assert.IsTrue(horseBarn.IsValid);

            var racingChariot = await horseBarn.AddRacingChariot();

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

            criteria.Name = "Heavy Horse B";
            criteria.Breed = Breed.Clydesdale;

            heavyHorse = (IHeavyHorse) await horseBarn.AddNewHorse(criteria);

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
            var racingChariot = await horseBarn.AddRacingChariot();

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
            var criteria = Mock.Of<IHorseCriteria>();
            criteria.Breed = Breed.Thoroughbred;
            var lightHorse = await horseBarn.AddNewHorse(criteria);

            Assert.IsNotNull(lightHorse);
            Assert.IsInstanceOfType<ILightHorse>(lightHorse);
            Assert.IsTrue(horseBarn.Pasture.Horses.Contains(lightHorse));
        }

        [TestMethod]
        public async Task HorseBarn_AddNewHorse_HeavyHorse()
        {
            var horseBarn = await portal.Create();
            var criteria = Mock.Of<IHorseCriteria>();
            criteria.Breed = Breed.Clydesdale;
            var heavyHorse = await horseBarn.AddNewHorse(criteria);

            Assert.IsNotNull(heavyHorse);
            Assert.IsInstanceOfType<IHeavyHorse>(heavyHorse);
            Assert.IsTrue(horseBarn.Pasture.Horses.Contains(heavyHorse));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task HorseBarn_AddNewHorse_InvalidBreed()
        {
            var horseBarn = await portal.Create();
            var criteria = Mock.Of<IHorseCriteria>();
            criteria.Breed = (Breed)999; // Invalid breed
            await horseBarn.AddNewHorse(criteria); // Invalid breed
        }

        [TestMethod]
        public async Task HorseBarn_MoveHorseToCart()
        {
            var horseBarn = await portal.Create();
            var criteria = Mock.Of<IHorseCriteria>();
            criteria.Breed = Breed.Clydesdale;
            var heavyHorse = (IHeavyHorse)await horseBarn.AddNewHorse(criteria);
            var wagon = await horseBarn.AddWagon();

            await horseBarn.MoveHorseToCart(heavyHorse, wagon);

            Assert.IsFalse(horseBarn.Pasture.Horses.Contains(heavyHorse));
            Assert.IsTrue(wagon.Horses.Contains(heavyHorse));
        }
    }
}
