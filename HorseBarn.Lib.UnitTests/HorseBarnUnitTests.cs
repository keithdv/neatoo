using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Moq;
using Neatoo;
using Neatoo.Portal.Internal;
using Xunit;

namespace HorseBarn.lib.UnitTests
{
    public sealed class HorseBarnTests : IDisposable
    {
        private Mock<ILightHorseFactory> mockLightHorsePortal;
        private Mock<IHeavyHorseFactory> mockHeavyHorsePortal;
        private Mock<IRacingChariotFactory> mockRacingChariotPortal;
        private Mock<IWagonFactory> mockWagonPortal;
        private Mock<IPastureFactory> mockPasturePortal;
        private Mock<ICartListFactory> mockCartListPortal;
        private Mock<IFactoryEditBase<HorseBarn>> mockHorseBarnPortal;
        private HorseBarn horseBarn;
        private Mock<IPasture> mockPasture;
        private Mock<ICartList> mockCartList;

        public HorseBarnTests()
        {
            mockLightHorsePortal = new Mock<ILightHorseFactory>(MockBehavior.Strict);
            mockHeavyHorsePortal = new Mock<IHeavyHorseFactory>(MockBehavior.Strict);
            mockRacingChariotPortal = new Mock<IRacingChariotFactory>(MockBehavior.Strict);
            mockWagonPortal = new Mock<IWagonFactory>(MockBehavior.Strict);
            mockPasturePortal = new Mock<IPastureFactory>(MockBehavior.Strict);
            mockCartListPortal = new Mock<ICartListFactory>(MockBehavior.Strict);
            mockHorseBarnPortal = new Mock<IFactoryEditBase<HorseBarn>>(MockBehavior.Strict);

            horseBarn = new HorseBarn(new EditBaseServices<HorseBarn>(mockHorseBarnPortal.Object), mockLightHorsePortal.Object, mockHeavyHorsePortal.Object, mockRacingChariotPortal.Object, mockWagonPortal.Object);

            mockPasture = new Mock<IPasture>();
            mockCartList = new Mock<ICartList>();

            mockPasturePortal.Setup(p => p.Create()).ReturnsAsync(mockPasture.Object);
            mockCartListPortal.Setup(c => c.Create()).ReturnsAsync(mockCartList.Object);

            horseBarn.Create(mockPasturePortal.Object, mockCartListPortal.Object).Wait();
        }

        [Fact]
        public void HorseBarn_Create()
        {
            Assert.Equal(mockPasture.Object, horseBarn.Pasture);
            Assert.Equal(mockCartList.Object, horseBarn.Carts);
        }

        [Fact]
        public async Task HorseBarn_AddRacingCart()
        {
            var mockRacingChariot = new Mock<IRacingChariot>();

            mockRacingChariotPortal.Setup(p => p.Create()).ReturnsAsync(mockRacingChariot.Object);

            var racingChariot = await horseBarn.AddRacingChariot();

            Assert.Equal(mockRacingChariot.Object, racingChariot);
            mockCartListPortal.Verify(c => c.Create(), Times.Once);
        }

        [Fact]
        public async Task HorseBarn_AddWagon()
        {
            var mockWagon = new Mock<IWagon>();

            mockWagonPortal.Setup(p => p.Create()).ReturnsAsync(mockWagon.Object);

            var wagon = await horseBarn.AddWagon();

            Assert.Equal(mockWagon.Object, wagon);
            mockCartListPortal.Verify(c => c.Create(), Times.Once);
        }

        [Fact]
        public async Task HorseBarn_AddNewHorse_LightHorse()
        {
            var mockLightHorse = new Mock<ILightHorse>();
            var criteria = Mock.Of<IHorseCriteria>();
            criteria.Breed = Breed.Thoroughbred;

            mockLightHorsePortal.Setup(p => p.Create(criteria)).ReturnsAsync(mockLightHorse.Object);
            mockPasture.Setup(p => p.HorseList.Add(It.IsAny<IHorse>()));

            var lightHorse = await horseBarn.AddNewHorse(criteria);

            Assert.Equal(mockLightHorse.Object, lightHorse);
            mockPasture.Verify(p => p.HorseList.Add(It.IsAny<IHorse>()), Times.Once);
        }

        [Fact]
        public async Task HorseBarn_AddNewHorse_HeavyHorse()
        {
            var mockHeavyHorse = new Mock<IHeavyHorse>();
            var criteria = Mock.Of<IHorseCriteria>();

            criteria.Breed = Breed.Clydesdale;
            mockHeavyHorsePortal.Setup(p => p.Create(criteria)).ReturnsAsync(mockHeavyHorse.Object);
            mockPasture.Setup(p => p.HorseList.Add(It.IsAny<IHorse>()));

            var heavyHorse = await horseBarn.AddNewHorse(criteria);

            Assert.Same(mockHeavyHorse.Object, heavyHorse);
            mockPasture.Verify(p => p.HorseList.Add(It.IsAny<IHorse>()), Times.Once);
        }

        [Fact]
        public async Task HorseBarn_AddNewHorse_InvalidBreed()
        {
            var criteria = Mock.Of<IHorseCriteria>();
            criteria.Breed = (Breed)999;

            await Assert.ThrowsAsync<Exception>(() => horseBarn.AddNewHorse(criteria)); // Invalid breed
        }


        [Fact]
        public async Task HorseBarn_MoveHorseToCart()
        {
            var mockHeavyHorse = new Mock<IHeavyHorse>();
            var mockWagon = new Mock<ICart>();

            mockWagon.Setup(w => w.AddHorse(mockHeavyHorse.Object)).Returns(Task.CompletedTask);
            mockWagon.Setup(w => w.RunAllRules(CancellationToken.None)).Returns(Task.CompletedTask);
            mockPasture.Setup(p => p.RemoveHorse(mockHeavyHorse.Object)).Verifiable();
            mockCartList.Setup(c => c.RemoveHorse(mockHeavyHorse.Object)).Returns(Task.CompletedTask).Verifiable();

            await horseBarn.MoveHorseToCart(mockHeavyHorse.Object, mockWagon.Object);

            mockPasture.Verify(p => p.RemoveHorse(mockHeavyHorse.Object), Times.Once);
            mockCartList.Verify(c => c.RemoveHorse(mockHeavyHorse.Object), Times.Once);
            mockWagon.Verify(w => w.AddHorse(mockHeavyHorse.Object), Times.Once);
        }

        [Fact]
        public async Task HorseBarn_MoveHorseToPasture()
        {
            var mockHeavyHorse = new Mock<IHeavyHorse>();

            mockCartList.Setup(c => c.RemoveHorse(mockHeavyHorse.Object)).Returns(Task.CompletedTask).Verifiable();
            mockPasture.Setup(p => p.HorseList.Contains(mockHeavyHorse.Object)).Returns(false);
            mockPasture.Setup(p => p.HorseList.Add(mockHeavyHorse.Object)).Verifiable();

            await horseBarn.MoveHorseToPasture(mockHeavyHorse.Object);

            mockCartList.Verify(c => c.RemoveHorse(mockHeavyHorse.Object), Times.Once);
            mockPasture.Verify(p => p.HorseList.Add(mockHeavyHorse.Object), Times.Once);
        }

        public void Dispose()
        {
            // Cleanup code, if needed
        }
    }
}