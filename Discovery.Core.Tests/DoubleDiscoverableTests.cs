using System;
using AutoFixture.Xunit2;
using Discovery.Generator;
using Moq;
using Xunit;

namespace Discovery.Core.Tests
{
    public class DoubleDiscoverableTests
    {
        [Fact]
        public void Constructor_Should_Initialise()
        {
            // arrange
            // act
            var actual = new DoubleDiscoverable();

            // assert
            Assert.NotNull(actual);
        }

        [Theory]
        [InlineAutoData(null, null, null, false)]
        [InlineAutoData(null, null, 1.0, false)]
        [InlineAutoData(null, 1.0, 1.0, false)]
        [InlineAutoData(0.0, null, 1.0, false)]
        [InlineAutoData(1.0, null, 1.0, false)]
        [InlineAutoData(0.0, 1.0, 1.0, true)]
        public void Constructor_Should_Initialise_With_Args(
            double? lowerBound,
            double? upperBound,
            double? value,
            bool isBounded,
            Mock<IDoubleGenerator> mockGenerator
        )
        {
            // arrange
            // act
            var actual = new DoubleDiscoverable(new DoubleDiscoverableArgs
            {
                Generator = mockGenerator.Object,
                Value = value,
                LowerBound = lowerBound,
                UpperBound = upperBound
            });

            // assert
            // Assert.IsType(typeof(exception), actual);
            Assert.Equal(value, actual.Value);
            Assert.Equal(isBounded, actual.IsBounded);
        }

        [Theory]
        [InlineAutoData(1.0, 0.0, 1.0, typeof(ArgumentException))]
        public void Constructor_Should_Throw_Exception(
            double? lowerBound,
            double? upperBound,
            double? value,
            Type exception,
            Mock<IDoubleGenerator> mockGenerator
        )
        {
            // arrange
            // act
            // assert
            Assert.Throws(exception, () =>
            {
                return new DoubleDiscoverable(new DoubleDiscoverableArgs
                {
                    Generator = mockGenerator.Object,
                    Value = value,
                    LowerBound = lowerBound,
                    UpperBound = upperBound
                });
            });
        }

        [Theory]
        [InlineAutoData(null, null, null, 0.5, false, 1, 0)]
        [InlineAutoData(0.0, null, null, 0.5, false, 1, 0)]
        [InlineAutoData(null, 1.0, null, 0.5, false, 1, 0)]
        [InlineAutoData(0.0, 1.0, null, 0.5, true, 0, 1)]
        public void Generate_Should_Match_Expected(
            double? lowerBound,
            double? upperBound,
            double? value,
            double generatedOutput,
            bool expectedIsBounded,
            int expectedTimesNextDoubleCalled,
            int expectedTimesNextDoubleBoundedCalled,
            Mock<IDoubleGenerator> mockGenerator
        )
        {
            // arrange
            mockGenerator
                .Setup(m => m.NextDouble())
                .Returns(generatedOutput);

            mockGenerator
                .Setup(m => m.NextDouble(It.IsAny<double>(), It.IsAny<double>()))
                .Returns(generatedOutput);

            var args = new DoubleDiscoverableArgs
            {
                Generator = mockGenerator.Object,
                Value = value,
                LowerBound = lowerBound,
                UpperBound = upperBound
            };

            var target = new DoubleDiscoverable(args);

            // act
            var actual = target.Generate();

            // assert
            Assert.NotNull(actual);
            Assert.Equal(target.Value, actual);
            Assert.Equal(expectedIsBounded, target.IsBounded);

            mockGenerator
                .Verify(v =>
                        v.NextDouble(),
                    Times.Exactly(expectedTimesNextDoubleCalled));

            mockGenerator
                .Verify(v =>
                        v.NextDouble(
                            It.IsAny<double>(),
                            It.IsAny<double>()),
                    Times.Exactly(expectedTimesNextDoubleBoundedCalled));
        }

        [Theory]
        [InlineAutoData(null, null, null, 0.0, false, null, 0, 0)]
        [InlineAutoData(null, null, 0.0, 0.0, false, null, 0, 0)]
        [InlineAutoData(null, null, 0.0, 0.0, false, 0.0, 1, 0)]
        [InlineAutoData(null, null, 0.0, 0.2, false, 0.1, 1, 0)]
        [InlineAutoData(0.0, null, 0.0, 0.2, false, 0.1, 1, 0)]
        [InlineAutoData(null, 1.0, 0.0, 0.2, false, 0.1, 1, 0)]
        [InlineAutoData(0.0, 1.0, 0.0, 0.2, true, 0.1, 1, 0)]
        [InlineAutoData(null, null, 0.0, 0.2, false, 0.3, 2, 0)]
        [InlineAutoData(0.0, 1.0, 0.0, 0.2, true, 0.3, 1, 1)]
        public void Mutate_Should_Match_Expected(
            double? lowerBound,
            double? upperBound,
            double? value,
            double nextDoubleOutput,
            bool expectedIsBounded,
            double? mutationProbability,
            int expectedTimesNextDoubleCalled,
            int expectedTimesNextDoubleArgsCalled,
            Mock<IDoubleGenerator> mockGenerator
        )
        {
            // arrange
            mockGenerator
                .Setup(m => m.NextDouble())
                .Returns(nextDoubleOutput);

            mockGenerator
                .Setup(m => m.NextDouble(It.IsAny<double>(), It.IsAny<double>()))
                .Returns(nextDoubleOutput);

            var args = new DoubleDiscoverableArgs
            {
                Generator = mockGenerator.Object,
                Value = value,
                LowerBound = lowerBound,
                UpperBound = upperBound
            };

            var target = new DoubleDiscoverable(args);

            // act
            var actual = target.Mutate(mutationProbability);

            // assert
            Assert.Equal(target.Value, actual);
            Assert.Equal(target.IsBounded, expectedIsBounded);

            mockGenerator
                .Verify(v =>
                        v.NextDouble(),
                    Times.Exactly(expectedTimesNextDoubleCalled));

            mockGenerator
                .Verify(v =>
                        v.NextDouble(
                            It.IsAny<double>(),
                            It.IsAny<double>()),
                    Times.Exactly(expectedTimesNextDoubleArgsCalled));
        }
    }
}
