using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Moq;
using Neatoo.Portal;
using Neatoo;

namespace HorseBarn.lib.UnitTests
{
    [TestClass]
    public sealed class HorseBarnTests
    {
        private Mock<IReadWritePortalChild<ILightHorse>> mockLightHorsePortal;
        private Mock<IReadWritePortalChild<IHeavyHorse>> mockHeavyHorsePortal;
        private Mock<IReadWritePortalChild<IRacingChariot>> mockRacingChariotPortal;
        private Mock<IReadWritePortalChild<IWagon>> mockWagonPortal;
        private Mock<IReadWritePortalChild<IPasture>> mockPasturePortal;
        private Mock<IReadWritePortalChild<ICartList>> mockCartListPortal;
        private Mock<IReadWritePortal<HorseBarn>> mockHorseBarnPortal;
        private HorseBarn horseBarn;
        private Mock<IPasture> mockPasture;
        private Mock<ICartList> mockCartList;

        [TestInitialize]
        public async Task TestInitialize()
        {
            mockLightHorsePortal = new Mock<IReadWritePortalChild<ILightHorse>>();
            mockHeavyHorsePortal = new Mock<IReadWritePortalChild<IHeavyHorse>>();
            mockRacingChariotPortal = new Mock<IReadWritePortalChild<IRacingChariot>>();
            mockWagonPortal = new Mock<IReadWritePortalChild<IWagon>>();
            mockPasturePortal = new Mock<IReadWritePortalChild<IPasture>>();
            mockCartListPortal = new Mock<IReadWritePortalChild<ICartList>>();
            mockHorseBarnPortal = new Mock<IReadWritePortal<HorseBarn>>();

            horseBarn = new HorseBarn(new EditBaseServices<HorseBarn>(mockHorseBarnPortal.Object), mockLightHorsePortal.Object, mockHeavyHorsePortal.Object, mockRacingChariotPortal.Object, mockWagonPortal.Object);

            mockPasture = new Mock<IPasture>();
            mockCartList = new Mock<ICartList>();

            mockPasturePortal.Setup(p => p.CreateChild()).ReturnsAsync(mockPasture.Object);
            mockCartListPortal.Setup(c => c.CreateChild()).ReturnsAsync(mockCartList.Object);

            await horseBarn.Create(mockPasturePortal.Object, mockCartListPortal.Object);
        }

        [TestMethod]
        public void HorseBarn_Create()
        {
            Assert.AreEqual(mockPasture.Object, horseBarn.Pasture);
            Assert.AreEqual(mockCartList.Object, horseBarn.Carts);
        }

        [TestMethod]
        public async Task HorseBarn_AddRacingCart()
        {
            var mockRacingChariot = new Mock<IRacingChariot>();

            mockRacingChariotPortal.Setup(p => p.CreateChild()).ReturnsAsync(mockRacingChariot.Object);

            var racingChariot = await horseBarn.AddRacingChariot();

            Assert.AreEqual(mockRacingChariot.Object, racingChariot);
            mockCartListPortal.Verify(c => c.CreateChild(), Times.Once);
        }

        [TestMethod]
        public async Task HorseBarn_AddWagon()
        {
            var mockWagon = new Mock<IWagon>();

            mockWagonPortal.Setup(p => p.CreateChild()).ReturnsAsync(mockWagon.Object);

            var wagon = await horseBarn.AddWagon();

            Assert.AreEqual(mockWagon.Object, wagon);
            mockCartListPortal.Verify(c => c.CreateChild(), Times.Once);
        }

        [TestMethod]
        public async Task HorseBarn_AddNewHorse_LightHorse()
        {
            var mockLightHorse = new Mock<ILightHorse>();
            var criteria = Mock.Of<IHorseCriteria>();
            criteria.Breed = Breed.Thoroughbred;

            mockLightHorsePortal.Setup(p => p.CreateChild(It.IsAny<Breed>())).ReturnsAsync(mockLightHorse.Object);

            var lightHorse = await horseBarn.AddNewHorse(criteria);

            Assert.AreEqual(mockLightHorse.Object, lightHorse);
            mockLightHorse.Verify(h => h.RunAllRules(CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task HorseBarn_AddNewHorse_HeavyHorse()
        {
            var mockHeavyHorse = new Mock<IHeavyHorse>();

            mockHeavyHorsePortal.Setup(p => p.CreateChild(It.IsAny<IHorseCriteria>())).ReturnsAsync(mockHeavyHorse.Object);

            var heavyHorse = await horseBarn.AddNewHorse(Mock.Of<IHorseCriteria>());

            Assert.AreEqual(mockHeavyHorse.Object, heavyHorse);
            mockHeavyHorse.Verify(h => h.RunAllRules(CancellationToken.None), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task HorseBarn_AddNewHorse_InvalidBreed()
        {
            var criteria = Mock.Of<IHorseCriteria>();
            criteria.Breed = (Breed) 999;

            await horseBarn.AddNewHorse(criteria); // Invalid breed
        }

        [TestMethod]
        public async Task HorseBarn_MoveHorseToCart()
        {
            var mockHeavyHorse = new Mock<IHeavyHorse>();
            var mockWagon = new Mock<ICart>();

            await horseBarn.MoveHorseToCart(mockHeavyHorse.Object, mockWagon.Object);

            mockWagon.Verify(w => w.HorseList.Add(mockHeavyHorse.Object), Times.Once);
            mockWagon.Verify(w => w.RunAllRules(CancellationToken.None), Times.Once);
        }
    }
}