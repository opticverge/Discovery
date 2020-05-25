using AutoFixture.Xunit2;
using Xunit;

namespace Discovery.Generator.Tests
{
    public class RandomGeneratorTests
    {
        [Fact]
        public void Constructor_Should_Initialise()
        {
            // arrange
            // act
            var actual = new RandomGenerator();

            // act
            Assert.NotNull(actual);
        }

        [Theory]
        [InlineAutoData(null)]
        [InlineAutoData(0)]
        [InlineAutoData(123)]
        [InlineAutoData(int.MinValue)]
        [InlineAutoData(int.MaxValue)]
        public void Constructor_Should_Initialise_With_Args(
            int? seed
        )
        {
            // arrange
            // act
            var actual = new RandomGenerator(seed);

            // act
            Assert.NotNull(actual);
        }

        [Theory]
        [InlineAutoData(0)]
        [InlineAutoData(123)]
        [InlineAutoData(int.MinValue)]
        [InlineAutoData(int.MaxValue)]
        public void NextDouble_Should_Match_Expected(
            int? seed
        )
        {
            // arrange
            var target = new RandomGenerator(seed);

            // act
            var actual = target.NextDouble();

            // act
            Assert.IsType<double>(actual);
            Assert.InRange(actual, 0.0, 1.0);
        }

        [Theory]
        [InlineAutoData(0.0, 1.0)]
        [InlineAutoData(-1.0, 1.0)]
        [InlineAutoData(-1.0, 0.0)]
        [InlineAutoData(0.0, double.MaxValue)]
        [InlineAutoData(double.MinValue, 0)]
        public void NextDouble_With_Bounds_Should_Match_Expected(
            double lowerBound,
            double upperBound
        )
        {
            // arrange
            var target = new RandomGenerator(42);

            // act
            var actual = target.NextDouble(lowerBound, upperBound);

            // act
            Assert.IsType<double>(actual);
            Assert.InRange(actual, lowerBound, upperBound);
        }
    }
}
