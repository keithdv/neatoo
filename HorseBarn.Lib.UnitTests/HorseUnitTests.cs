using Moq;
using Xunit;
using System.Threading.Tasks;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Rules;
using System;
using HorseBarn.Dal.Ef;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Reflection.Metadata;
using Moq.EntityFrameworkCore;

namespace HorseBarn.lib.UnitTests
{

    internal class HorseSubject : Horse<HorseSubject>
    {
        public HorseSubject(INeatooPortal<HorseSubject> readWritePortal) : base(new EditBaseServices<HorseSubject>(readWritePortal))
        {
        }
    }

    public class HorseTests
    {
        Mock<INeatooPortal<HorseSubject>> _mockPortal = new Mock<INeatooPortal<HorseSubject>>();

        private readonly Mock<IRuleManager<HorseSubject>> _mockRuleManager;
        private readonly HorseSubject _horse;
        private readonly IHorseCriteria _horseCriteria;

        public HorseTests()
        {
            _mockRuleManager = new Mock<IRuleManager<HorseSubject>>();

            _horseCriteria = Mock.Of<IHorseCriteria>();
            _horseCriteria.BirthDay = DateOnly.FromDateTime(DateTime.Now.AddYears(-3));
            _horseCriteria.Breed = Breed.Clydesdale;
            _horseCriteria.Name = "Rainbow";

            _horse = new HorseSubject(_mockPortal.Object);
            _horse.Create(_horseCriteria);
        }

        [Fact]
        public void Name_Should_Set_And_Get_Value()
        {
            // Arrange
            var expectedName = "Thunder";

            // Act
            _horse.Name = expectedName;
            var actualName = _horse.Name;

            // Assert
            Assert.Equal(expectedName, actualName);
        }


        [Fact]
        public void Weight_Should_Set_And_Get_Value()
        {
            // Arrange
            var expectedWeight = 500.0;

            // Act
            _horse.Weight = expectedWeight;
            var actualWeight = _horse.Weight;

            // Assert
            Assert.Equal(expectedWeight, actualWeight);
        }

        [Fact]
        public void Create_Should_Set_Properties()
        {
            // Arrange
            var mockCriteria = new Mock<IHorseCriteria>();
            mockCriteria.Setup(c => c.Breed).Returns(Breed.Mustang);
            mockCriteria.Setup(c => c.BirthDay).Returns(DateOnly.FromDateTime(DateTime.Now.AddYears(-3)));
            mockCriteria.Setup(c => c.Name).Returns("Lightning");

            // Act
            _horse.Create(mockCriteria.Object);

            // Assert
            Assert.Equal(Breed.Mustang, _horse.Breed);
            Assert.Equal(mockCriteria.Object.BirthDay, _horse.BirthDate);
            Assert.Equal("Lightning", _horse.Name);
        }

        [Fact]
        public void Fetch_Should_Set_Properties_From_Horse()
        {
            // Arrange
            var efHorse = new Dal.Ef.Horse
            {
                Id = 1,
                BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),
                Breed = (int)Breed.Clydesdale,
                Name = "Storm"
            };

            // Act
            _horse.Fetch(efHorse);

            // Assert
            Assert.Equal(1, _horse.Id);
            Assert.Equal(efHorse.BirthDate, _horse.BirthDate);
            Assert.Equal(Breed.Clydesdale, _horse.Breed);
            Assert.Equal("Storm", _horse.Name);
        }

        [Fact]
        public void Insert_Should_Add_Horse_To_Pasture()
        {
            // Arrange
            var pasture = new Dal.Ef.Pasture();
            pasture.Horses = new List<Dal.Ef.Horse>();

            // Act
            _horse.Insert(pasture);

            // Assert
            var horse = pasture.Horses.Single();
            Assert.Equal(_horse.BirthDate, horse.BirthDate);
            Assert.Equal((int)_horse.Breed, horse.Breed);
            Assert.Equal(_horse.Name, horse.Name);
        }

        [Fact]
        public void Insert_Should_Add_Horse_To_Cart()
        {
            // Arrange
            var cart = new Dal.Ef.Cart();
            cart.Horses = new List<Dal.Ef.Horse>();

            // Act
            _horse.Insert(cart);

            // Assert
            var horse = cart.Horses.Single();
            Assert.Equal(_horse.BirthDate, horse.BirthDate);
            Assert.Equal((int)_horse.Breed, horse.Breed);
            Assert.Equal(_horse.Name, horse.Name);
        }

        [Fact]
        public async Task Update_Should_Update_Horse_In_Pasture()
        {
            // Arrange

            var horseList = new List<Dal.Ef.Horse>
            {
                new Dal.Ef.Horse
                {
                    Id = 1,
                    BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),
                    Breed = (int)Breed.Clydesdale,
                    Name = "Storm"
                }
            };

            _horse.Id = 1;

            var horseBarnContext = new Mock<IHorseBarnContext>();

            horseBarnContext.Setup(c => c.Horses).ReturnsDbSet(horseList);

            var pasture = new Dal.Ef.Pasture
            {
                Horses = new List<Dal.Ef.Horse>()
            };

            // Act
            await _horse.Update(pasture, horseBarnContext.Object);

            var efHorse = pasture.Horses.Single();

            // Assert
            Assert.Equal(_horse.BirthDate, efHorse.BirthDate);
            Assert.Equal((int)_horse.Breed, efHorse.Breed);
            Assert.Equal(_horse.Name, efHorse.Name);
            Assert.Contains(pasture.Horses, h => h.Id == efHorse.Id);
        }

        [Fact]
        public async Task Update_Should_Update_Horse_In_Cart()
        {
            // Arrange
            var horseList = new List<Dal.Ef.Horse>
            {
                new Dal.Ef.Horse
                {
                    Id = 1,
                    BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),
                    Breed = (int)Breed.Clydesdale,
                    Name = "Storm"
                }
            };

            _horse.Id = 1;

            var horseBarnContext = new Mock<IHorseBarnContext>();

            horseBarnContext.Setup(c => c.Horses).ReturnsDbSet(horseList);

            var cart = new Dal.Ef.Cart
            {
                Horses = new List<Dal.Ef.Horse>()
            };

            // Act
            await _horse.Update(cart, horseBarnContext.Object);

            var efHorse = cart.Horses.Single();

            // Assert
            Assert.Equal(_horse.BirthDate, efHorse.BirthDate);
            Assert.Equal((int)_horse.Breed, efHorse.Breed);
            Assert.Equal(_horse.Name, efHorse.Name);
            Assert.Contains(cart.Horses, h => h.Id == efHorse.Id);
        }

        [Fact]
        public void Delete_Should_Remove_Horse_From_Cart()
        {
            // Arrange
            var horse = new Dal.Ef.Horse
            {
                Id = 1,
                BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),
                Breed = (int)Breed.Clydesdale,
                Name = "Storm"
            };

            var cart = new Dal.Ef.Cart
            {
                Horses = new List<Dal.Ef.Horse> { horse }
            };

            _horse.Id = 1;

            // Act
            _horse.Delete(cart);

            // Assert
            Assert.DoesNotContain(cart.Horses, h => h.Id == horse.Id);
        }

        [Fact]
        public void Delete_Should_Remove_Horse_From_Pasture()
        {
            // Arrange
            var horse = new Dal.Ef.Horse
            {
                Id = 1,
                BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),
                Breed = (int)Breed.Clydesdale,
                Name = "Storm"
            };

            var pasture = new Dal.Ef.Pasture
            {
                Horses = new List<Dal.Ef.Horse> { horse }
            };

            _horse.Id = 1;

            // Act
            _horse.Delete(pasture);

            // Assert
            Assert.DoesNotContain(pasture.Horses, h => h.Id == horse.Id);
        }


    }
}