using Microsoft.Extensions.DependencyInjection;
using HorseBarn.Dal.Ef;
using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Neatoo.Portal;
using System.Data;

namespace HorseBarn.lib.integration.tests
{
    [TestClass]
    public sealed class HorseBarnTests
    {
        private IServiceScope scope;
        private IReadWritePortal<IHorseBarn> portal;
        private HorseBarnContext horseBarnContext;
        private IDbContextTransaction transaction;

        [TestInitialize]
        public async Task TestInitialize()
        {

            scope = UnitTestContainer.GetLifetimeScope();
            portal = scope.ServiceProvider.GetRequiredService<IReadWritePortal<IHorseBarn>>();
            horseBarnContext = scope.ServiceProvider.GetRequiredService<HorseBarnContext>();

            await horseBarnContext.Horses.ExecuteDeleteAsync();
            await horseBarnContext.Carts.ExecuteDeleteAsync();
            await horseBarnContext.Pastures.ExecuteDeleteAsync();
            await horseBarnContext.HorseBarns.ExecuteDeleteAsync();

            transaction = await horseBarnContext.Database.BeginTransactionAsync();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            transaction.Rollback();
            scope.Dispose();
        }

        [TestMethod]
        public async Task HorseBarn_FullRun()
        {
            var horseBarn = await portal.Create();

            Assert.IsTrue(horseBarn.IsValid);
            Assert.IsTrue(horseBarn.IsNew);
            Assert.IsTrue(horseBarn.IsModified);

            async Task AddCartToHorseBarn()
            {
                var criteria = Mock.Of<IHorseCriteria>();

                criteria.Name = "Heavy Horse A";
                criteria.Breed = Breed.Clydesdale;
                criteria.BirthDay = DateOnly.FromDateTime(DateTime.Now);

                var heavyHorse = (IHeavyHorse)await horseBarn.AddNewHorse(criteria);

                Assert.IsTrue(horseBarn.IsValid);

                Assert.IsInstanceOfType<IHeavyHorse>(heavyHorse);

                criteria.Name = "Light Horse B";
                criteria.Breed = Breed.Thoroughbred;

                var lightHorse = (ILightHorse)await horseBarn.AddNewHorse(criteria);

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

                heavyHorse = (IHeavyHorse)await horseBarn.AddNewHorse(criteria);

                await horseBarn.MoveHorseToCart(heavyHorse, wagon);

                Assert.IsTrue(horseBarn.IsValid);
            }

            await AddCartToHorseBarn();

            horseBarn = (IHorseBarn) await horseBarn.Save();

            var horseBarnContext = scope.ServiceProvider.GetRequiredService<IHorseBarnContext>();

            var horses = await horseBarnContext.Horses.ToListAsync();

            Assert.AreEqual(3, horses.Count);
            CollectionAssert.AreEquivalent(horseBarn.Horses.Select(h => h.Id).ToList(), horses.Select(h => h.Id).ToList());

            var carts = await horseBarnContext.Carts.ToListAsync();
            CollectionAssert.AreEquivalent(horseBarn.Carts.Select(c => c.Id).ToList(), carts.Select(c => c.Id).ToList());

            var pasture = await horseBarnContext.Pastures.ToListAsync();
            Assert.AreEqual(pasture.Single().Id, horseBarn.Pasture.Id);

            horseBarn = await portal.Fetch();

            await AddCartToHorseBarn();

            foreach (var item in horseBarn.Horses)
            {
                item.Name = Guid.NewGuid().ToString();
            }

            var horseNames = horseBarn.Horses.Select(h => h.Name).ToList();

            // Mix of Inserts and Updates
            horseBarn = (IHorseBarn) await horseBarn.Save();

            Assert.IsFalse(horseBarn.IsModified); // TODO
            CollectionAssert.AreEquivalent(horseNames, horseBarnContext.Horses.Select(h => h.Name).ToList());
            CollectionAssert.AreEquivalent(horseBarn.Carts.Select(c => c.Id).ToList(), horseBarnContext.Carts.Select(c => c.Id).ToList());
            Assert.AreEqual(horseBarn.Pasture.Id, horseBarnContext.Pastures.Single().Id);
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
