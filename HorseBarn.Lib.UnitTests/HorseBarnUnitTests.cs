using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Moq;
using Neatoo.Portal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Neatoo;

namespace HorseBarn.lib.UnitTests
{
    [TestClass]
    public sealed class HorseBarnTests
    {
        private Mock<ISendReceivePortalChild<ILightHorse>> mockLightHorsePortal;
        private Mock<ISendReceivePortalChild<IHeavyHorse>> mockHeavyHorsePortal;
        private Mock<ISendReceivePortalChild<IRacingChariot>> mockRacingChariotPortal;
        private Mock<ISendReceivePortalChild<IWagon>> mockWagonPortal;
        private Mock<ISendReceivePortalChild<IPasture>> mockPasturePortal;
        private Mock<ISendReceivePortalChild<ICartList>> mockCartListPortal;
        private Mock<ISendReceivePortal<HorseBarn>> mockHorseBarnPortal;
        private HorseBarn horseBarn;
        private Mock<IPasture> mockPasture;
        private Mock<ICartList> mockCartList;

        [TestInitialize]
        public async Task TestInitialize()
        {
            mockLightHorsePortal = new Mock<ISendReceivePortalChild<ILightHorse>>();
            mockHeavyHorsePortal = new Mock<ISendReceivePortalChild<IHeavyHorse>>();
            mockRacingChariotPortal = new Mock<ISendReceivePortalChild<IRacingChariot>>();
            mockWagonPortal = new Mock<ISendReceivePortalChild<IWagon>>();
            mockPasturePortal = new Mock<ISendReceivePortalChild<IPasture>>();
            mockCartListPortal = new Mock<ISendReceivePortalChild<ICartList>>();
            mockHorseBarnPortal = new Mock<ISendReceivePortal<HorseBarn>>();

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

            var racingChariot = await horseBarn.AddRacingCart();

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

            mockLightHorsePortal.Setup(p => p.CreateChild(It.IsAny<Breed>())).ReturnsAsync(mockLightHorse.Object);

            var lightHorse = await horseBarn.AddNewHorse(Breed.Thoroughbred);

            Assert.AreEqual(mockLightHorse.Object, lightHorse);
            mockLightHorse.Verify(h => h.CheckAllRules(CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task HorseBarn_AddNewHorse_HeavyHorse()
        {
            var mockHeavyHorse = new Mock<IHeavyHorse>();

            mockHeavyHorsePortal.Setup(p => p.CreateChild(It.IsAny<Breed>())).ReturnsAsync(mockHeavyHorse.Object);

            var heavyHorse = await horseBarn.AddNewHorse(Breed.Clydesdale);

            Assert.AreEqual(mockHeavyHorse.Object, heavyHorse);
            mockHeavyHorse.Verify(h => h.CheckAllRules(CancellationToken.None), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task HorseBarn_AddNewHorse_InvalidBreed()
        {
            await horseBarn.AddNewHorse((Breed)999); // Invalid breed
        }

        [TestMethod]
        public async Task HorseBarn_MoveHorseToCart()
        {
            var mockHeavyHorse = new Mock<IHeavyHorse>();
            var mockWagon = new Mock<ICart<IHeavyHorse>>();

            await horseBarn.MoveHorseToCart(mockHeavyHorse.Object, mockWagon.Object);

            mockWagon.Verify(w => w.HorseList.Add(mockHeavyHorse.Object), Times.Once);
            mockWagon.Verify(w => w.CheckAllRules(CancellationToken.None), Times.Once);
        }
    }
}