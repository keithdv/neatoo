using Moq;
using Neatoo;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HorseBarn.lib.Horse.Tests
{
    public class HorseCriteriaTests
    {
        private readonly HorseCriteria _horseCriteria;

        public HorseCriteriaTests()
        {
            _horseCriteria = new HorseCriteria(new ValidateBaseServices<HorseCriteria>(), Mock.Of<IHorseNameUniqueRule>());
        }

        [Fact]
        public void Name_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            var expectedName = "Thunder";

            // Act
            _horseCriteria.Name = expectedName;
            var actualName = _horseCriteria.Name;

            // Assert
            Assert.Equal(expectedName, actualName);
        }

        [Fact]
        public void Breed_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            var expectedBreed = Breed.Thoroughbred;

            // Act
            _horseCriteria.Breed = expectedBreed;
            var actualBreed = _horseCriteria.Breed;

            // Assert
            Assert.Equal(expectedBreed, actualBreed);
        }

        [Fact]
        public void BirthDay_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            var expectedBirthDay = new DateOnly(2020, 1, 1);

            // Act
            _horseCriteria.BirthDay = expectedBirthDay;
            var actualBirthDay = _horseCriteria.BirthDay;

            // Assert
            Assert.Equal(expectedBirthDay, actualBirthDay);
        }

        [Fact]
        public void Fetch_CallsFetchMethod()
        {
            var horseCriteria = new HorseCriteria(new ValidateBaseServices<HorseCriteria>(), Mock.Of<IHorseNameUniqueRule>());

            horseCriteria.Fetch(new string[] { });

            Assert.False(horseCriteria.IsValid);
        }

        [Fact]
        public void OnDeserialized_CallsClearAllErrors()
        {
            // Arrange
            var mockHorseCriteria = new Mock<HorseCriteria>(new ValidateBaseServices<HorseCriteria>()) { CallBase = true };

            // Act
            mockHorseCriteria.Object.OnDeserialized();

            // Assert
            mockHorseCriteria.Verify(hc => hc.ClearAllErrors(), Times.Once);
        }
    }
}
