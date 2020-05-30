using System;
using AutoFixture.Xunit2;
using Discovery.Generator;
using Moq;
using Xunit;

namespace Discovery.Core.Tests
{
    public class IntDiscoverableTests
    {
        [Theory]
        [InlineAutoData(null, null, null, false)]
        [InlineAutoData(null, null, 10, false)]
        [InlineAutoData(null, 10, 10, false)]
        [InlineAutoData(0, null, 10, false)]
        [InlineAutoData(10, null, 10, false)]
        [InlineAutoData(0, 10, 10, true)]
        public void Constructor_Should_Initialise_With_Args(
            int? lowerBound,
            int? upperBound,
            int? value,
            bool isBounded,
            Mock<IGenerator> mockGenerator
        )
        {
            // arrange
            // act
            var actual = new IntDiscoverable(new IntDiscoverableArgs
            {
                Generator = mockGenerator?.Object,
                Value = value,
                LowerBound = lowerBound,
                UpperBound = upperBound
            });

            // assert
            Assert.Equal(value, actual.Arguments.Value);
            Assert.Equal(isBounded, actual.IsBounded);
        }

        [Theory]
        [InlineAutoData(10, 0, 10, typeof(ArgumentException))]
        public void Constructor_Should_Throw_Exception(
            int? lowerBound,
            int? upperBound,
            int? value,
            Type exception,
            Mock<IGenerator> mockGenerator
        )
        {
            // arrange
            // act
            // assert
            Assert.Throws(exception, () =>
            {
                return new IntDiscoverable(new IntDiscoverableArgs
                {
                    Generator = mockGenerator.Object,
                    Value = value,
                    LowerBound = lowerBound,
                    UpperBound = upperBound
                });
            });
        }

        [Theory]
        [InlineAutoData(null, null, null, 5, false, 1, 0)]
        [InlineAutoData(0, null, null, 5, false, 1, 0)]
        [InlineAutoData(null, 10, null, 5, false, 1, 0)]
        [InlineAutoData(0, 10, null, 5, true, 0, 1)]
        public void Generate_Should_Match_Expected(
            int? lowerBound,
            int? upperBound,
            int? value,
            int generatedOutput,
            bool expectedIsBounded,
            int expectedTimesNextIntCalled,
            int expectedTimesNextIntBoundedCalled,
            Mock<IGenerator> mockGenerator
        )
        {
            // arrange
            mockGenerator
                .Setup(m => m.NextInt())
                .Returns(generatedOutput);

            mockGenerator
                .Setup(m => m.NextInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(generatedOutput);

            var arguments = new IntDiscoverableArgs
            {
                Generator = mockGenerator.Object,
                Value = value,
                LowerBound = lowerBound,
                UpperBound = upperBound
            };

            var target = new IntDiscoverable(arguments);

            // act
            var actual = target.Generate();

            // assert
            Assert.Equal(target.Value, actual);
            Assert.Equal(expectedIsBounded, target.IsBounded);

            mockGenerator
                .Verify(v =>
                        v.NextInt(),
                    Times.Exactly(expectedTimesNextIntCalled));

            mockGenerator
                .Verify(v =>
                        v.NextInt(
                            It.IsAny<int>(),
                            It.IsAny<int>()),
                    Times.Exactly(expectedTimesNextIntBoundedCalled));
        }

        [Theory]
        [InlineAutoData(null, null, null, 0, 0.0, false, null, 0, 0, 0)]
        [InlineAutoData(null, null, null, 0, 0.0, false, 0.0, 0, 0, 1)]
        [InlineAutoData(null, null, null, 0, 0.0, false, 1.0, 1, 0, 1)]
        [InlineAutoData(0, 10, null, 0, 0.0, true, 1.0, 0, 1, 1)]
        [InlineAutoData(null, 10, null, 0, 0.0, false, 1.0, 1, 0, 1)]
        [InlineAutoData(0, null, null, 0, 0.0, false, 1.0, 1, 0, 1)]
        [InlineAutoData(0, null, null, 0, 0.0, false, 0.0, 0, 0, 1)]
        [InlineAutoData(0, 10, null, 0, 0.0, true, 0.0, 0, 0, 1)]
        public void Mutate_Should_Match_Expected(
            int? lowerBound,
            int? upperBound,
            int? value,
            int nextIntOutput,
            double nextDoubleOutput,
            bool expectedIsBounded,
            double? mutationProbability,
            int expectedTimesNextIntCalled,
            int expectedTimesNextIntArgsCalled,
            int expectedTimesNextDoubleCalled,
            Mock<IGenerator> mockGenerator
        )
        {
            // arrange
            mockGenerator
                .Setup(m => m.NextInt())
                .Returns(nextIntOutput);

            mockGenerator
                .Setup(m => m.NextInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(nextIntOutput);

            mockGenerator
                .Setup(m => m.NextDouble())
                .Returns(nextDoubleOutput);

            var args = new IntDiscoverableArgs
            {
                Generator = mockGenerator.Object,
                Value = value,
                LowerBound = lowerBound,
                UpperBound = upperBound
            };

            var target = new IntDiscoverable(args);

            // act
            var actual = target.Mutate(mutationProbability);

            // assert
            Assert.Equal(target.Value, actual);
            Assert.Equal(target.IsBounded, expectedIsBounded);

            mockGenerator
                .Verify(v =>
                        v.NextInt(),
                    Times.Exactly(expectedTimesNextIntCalled));

            mockGenerator
                .Verify(v =>
                        v.NextInt(
                            It.IsAny<int>(),
                            It.IsAny<int>()),
                    Times.Exactly(expectedTimesNextIntArgsCalled));

            mockGenerator
                .Verify(v =>
                        v.NextDouble(),
                    Times.Exactly(expectedTimesNextDoubleCalled));
        }

        [Fact]
        public void Constructor_Should_Initialise()
        {
            // arrange
            // act
            var actual = new IntDiscoverable();

            // assert
            Assert.NotNull(actual);
        }
    }
}
