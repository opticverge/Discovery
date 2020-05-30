using System.Linq;
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

            // assert
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

            // assert
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

            // assert
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

            // assert
            Assert.IsType<double>(actual);
            Assert.InRange(actual, lowerBound, upperBound);
        }

        [Theory]
        [InlineAutoData(0.0, false)]
        [InlineAutoData(1.0, true)]
        public void NextBool_Should_Match_Expected(
            double loc,
            bool expected
        )
        {
            // arrange
            var target = new RandomGenerator(42);

            // act
            var actual = target.NextBool(loc);

            // assert
            Assert.Equal(expected, actual);
            Assert.IsType<bool>(actual);
        }

        [Theory]
        [InlineAutoData(0.0, 10, false)]
        [InlineAutoData(0.1, 10, false)]
        [InlineAutoData(0.2, 10, false)]
        [InlineAutoData(0.3, 10, false)]
        [InlineAutoData(1.0, 10, true)]
        [InlineAutoData(0.9, 10, true)]
        [InlineAutoData(0.8, 10, true)]
        [InlineAutoData(0.7, 10, true)]
        [InlineAutoData(0.0, 100, false)]
        [InlineAutoData(0.1, 100, false)]
        [InlineAutoData(0.2, 100, false)]
        [InlineAutoData(0.3, 100, false)]
        [InlineAutoData(1.0, 100, true)]
        [InlineAutoData(0.9, 100, true)]
        [InlineAutoData(0.8, 100, true)]
        [InlineAutoData(0.7, 100, true)]
        public void NextBool_Collection_Should_Match_Expected(
            double loc,
            int amount,
            bool expected
        )
        {
            // arrange
            var target = new RandomGenerator(42);

            // act
            var actual = Enumerable
                .Range(0, amount)
                .Select(v => target.NextBool(loc));

            // assert
            var trueCount = actual.Count(v => v);
            var falseCount = actual.Count(v => v == false);
            Assert.Equal(expected, falseCount <= trueCount);
        }

        [Theory]
        [InlineAutoData(0)]
        [InlineAutoData(123)]
        [InlineAutoData(int.MinValue)]
        [InlineAutoData(int.MaxValue)]
        public void NextInt_Should_Match_Expected(
            int? seed
        )
        {
            // arrange
            var target = new RandomGenerator(seed);

            // act
            var actual = target.NextInt();

            // assert
            Assert.IsType<int>(actual);
            Assert.InRange(actual, int.MinValue, int.MaxValue);
        }

        [Theory]
        [InlineAutoData(0, 10)]
        [InlineAutoData(-10, 10)]
        [InlineAutoData(-10, 0)]
        [InlineAutoData(0, int.MaxValue)]
        [InlineAutoData(int.MinValue, 0)]
        public void NextInt_With_Bounds_Should_Match_Expected(
            int lowerBound,
            int upperBound,
            int? seed
        )
        {
            // arrange
            var target = new RandomGenerator(seed);

            // act
            var actual = target.NextInt(lowerBound, upperBound);

            // assert
            Assert.IsType<int>(actual);
            Assert.InRange(actual, lowerBound, upperBound);
        }
    }
}
