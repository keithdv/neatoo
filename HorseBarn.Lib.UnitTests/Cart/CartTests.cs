using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Moq;
using Neatoo;
using Neatoo.Portal.Internal;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xunit;

namespace HorseBarn.lib.integration.tests
{

    internal class CartTestHarness : Cart<CartTestHarness, IHorse>
    {
        public CartTestHarness(IEditBaseServices<CartTestHarness> services,
                               ICartNumberOfHorsesRule cartNumberOfHorsesRule) : base(services, cartNumberOfHorsesRule)
        {
        }
    }

        public class CartTests
    {
        private readonly Mock<ICartNumberOfHorsesRule> _cartNumberOfHorsesRuleMock;
        private readonly Mock<IHorseList> _horseListMock;
        private readonly Mock<IHorseListFactory> _horseListFactoryMock;
        private readonly CartTestHarness _cart;

        public CartTests()
        {
            _cartNumberOfHorsesRuleMock = new Mock<ICartNumberOfHorsesRule>();
            _horseListMock = new Mock<IHorseList>();
            _horseListFactoryMock = new Mock<IHorseListFactory>();

            _cart = new CartTestHarness(new EditBaseServices<CartTestHarness>(Mock.Of<IFactoryEditBase<CartTestHarness>>()), _cartNumberOfHorsesRuleMock.Object)
            {
                NumberOfHorses = 2
            };

            _cart.Create(_horseListFactoryMock.Object);
        }

        [Fact]
        public async Task AddHorse_ShouldAddHorse_WhenHorseIsOfTypeH()
        {
            // Arrange
            var horseMock = new Mock<IHorse>();
            var horse = horseMock.Object;

            // Act
            await _cart.AddHorse(horse);

            // Assert
            _horseListMock.Verify(h => h.Add(It.IsAny<IHorse>()), Times.Once);
        }

        [Fact]
        public async Task AddHorse_ShouldThrowArgumentException_WhenHorseIsNotOfTypeH()
        {
            // Arrange
            var horseMock = new Mock<IHorse>();
            var horse = horseMock.Object;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _cart.AddHorse(horse));
        }

        [Fact]
        public async Task RemoveHorse_ShouldRemoveHorse()
        {
            // Arrange
            var horseMock = new Mock<IHorse>();
            var horse = horseMock.Object;

            // Act
            await _cart.RemoveHorse(horse);

            // Assert
            _horseListMock.Verify(h => h.RemoveHorse(horse), Times.Once);
        }

        [Fact]
        public void CanAddHorse_ShouldReturnTrue_WhenHorseCanBeAdded()
        {
            // Arrange
            var horseMock = new Mock<IHorse>();
            var horse = horseMock.Object;
            _horseListMock.Setup(h => h.Count).Returns(1);

            // Act
            var result = _cart.CanAddHorse(horse);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanAddHorse_ShouldReturnFalse_WhenHorseCannotBeAdded()
        {
            // Arrange
            var horseMock = new Mock<IHorse>();
            var horse = horseMock.Object;
            _horseListMock.Setup(h => h.Count).Returns(2);

            // Act
            var result = _cart.CanAddHorse(horse);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Fetch_ShouldFetchCartDetails()
        {
            // Arrange
            var cartMock = new Mock<Dal.Ef.Cart>();
            cartMock.Setup(c => c.Id).Returns(1);
            cartMock.Setup(c => c.Name).Returns("Test Cart");
            cartMock.Setup(c => c.NumberOfHorses).Returns(2);
            var cart = cartMock.Object;

            _horseListFactoryMock.Setup(h => h.Fetch(It.IsAny<ICollection<Dal.Ef.Horse>>()))
                .ReturnsAsync(_horseListMock.Object);

            // Act
            await _cart.Fetch(cart, _horseListFactoryMock.Object);

            // Assert
            Assert.Equal(1, _cart.Id);
            Assert.Equal("Test Cart", _cart.Name);
            Assert.Equal(2, _cart.NumberOfHorses);
            _horseListFactoryMock.Verify(h => h.Fetch(It.IsAny<ICollection<Dal.Ef.Horse>>()), Times.Once);
        }

        [Fact]
        public async Task Insert_ShouldInsertCartDetails()
        {
            // Arrange
            var horseBarnMock = new Mock<Dal.Ef.HorseBarn>();
            var horseBarn = horseBarnMock.Object;

            _horseListFactoryMock.Setup(h => h.Save(It.IsAny<IHorseList>(), It.IsAny<Dal.Ef.Cart>()))
                .ReturnsAsync(_horseListMock.Object);

            // Act
            await _cart.Insert(horseBarn, _horseListFactoryMock.Object);

            // Assert
            horseBarnMock.Verify(h => h.Carts.Add(It.IsAny<Dal.Ef.Cart>()), Times.Once);
            _horseListFactoryMock.Verify(h => h.Save(It.IsAny<IHorseList>(), It.IsAny<Dal.Ef.Cart>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldUpdateCartDetails()
        {
            // Arrange
            var horseBarnMock = new Mock<Dal.Ef.HorseBarn>();
            var cartMock = new Mock<Dal.Ef.Cart>();
            cartMock.Setup(c => c.Id).Returns(1);
            cartMock.Setup(c => c.CartType).Returns((int)CartType.RacingChariot);
            horseBarnMock.Setup(h => h.Carts.First(It.IsAny<Func<Dal.Ef.Cart, bool>>())).Returns(cartMock.Object);
            var horseBarn = horseBarnMock.Object;

            _horseListFactoryMock.Setup(h => h.Save(It.IsAny<IHorseList>(), It.IsAny<Dal.Ef.Cart>()))
                .ReturnsAsync(_horseListMock.Object);

            // Act
            await _cart.Update(horseBarn, _horseListFactoryMock.Object);

            // Assert
            cartMock.VerifySet(c => c.Name = _cart.Name, Times.Once);
            cartMock.VerifySet(c => c.NumberOfHorses = _cart.NumberOfHorses, Times.Once);
            _horseListFactoryMock.Verify(h => h.Save(It.IsAny<IHorseList>(), It.IsAny<Dal.Ef.Cart>()), Times.Once);
        }
    }
}
