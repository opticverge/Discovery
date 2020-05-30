using AutoFixture.Xunit2;
using Discovery.Generator;
using Moq;
using Xunit;

namespace Discovery.Core.Tests
{
    public class BooleanDiscoverableTests
    {
        [Theory]
        [InlineAutoData(null, null, null, false, null)]
        [InlineAutoData(null, null, false, false, null)]
        [InlineAutoData(null, null, true, true, null)]
        [InlineAutoData(0.5, null, null, false, null)]
        [InlineAutoData(0.5, null, false, false, null)]
        [InlineAutoData(0.5, null, true, true, null)]
        [InlineAutoData(null, 42, null, false, null)]
        [InlineAutoData(null, 42, false, false, null)]
        [InlineAutoData(null, 42, true, true, null)]
        [InlineAutoData(0.5, 42, null, false, null)]
        [InlineAutoData(0.5, 42, false, false, null)]
        [InlineAutoData(0.5, 42, true, true, null)]
        [InlineAutoData(0.5, 42, null, false)]
        [InlineAutoData(0.5, 42, false, false)]
        [InlineAutoData(0.5, 42, true, true)]
        public void Constructor_Should_Initialise_With_Args(
            double? location,
            int? seed,
            bool? value,
            bool expectedValue,
            Mock<IGenerator> generator
        )
        {
            // arrange
            // act
            var actual = new BooleanDiscoverable(
                new BooleanDiscoverableArgs
                {
                    Location = location,
                    Seed = seed,
                    Generator = generator?.Object,
                    Value = value
                });

            // assert
            Assert.NotNull(actual);
            Assert.Equal(location, actual.Arguments.Location);
            Assert.Equal(seed, actual.Arguments.Seed);
            Assert.Equal(generator?.Object, actual.Arguments.Generator);
            Assert.Equal(value, actual.Arguments.Value);
            Assert.Equal(expectedValue, actual.Value);
        }

        [Theory]
        [InlineAutoData(null, null, null, false, null)]
        [InlineAutoData(null, null, false, false, null)]
        [InlineAutoData(null, null, true, true, null)]
        [InlineAutoData(0.5, null, null, false, null)]
        [InlineAutoData(0.5, null, false, false, null)]
        [InlineAutoData(0.5, null, true, true, null)]
        [InlineAutoData(null, 42, null, false, null)]
        [InlineAutoData(null, 42, false, false, null)]
        [InlineAutoData(null, 42, true, true, null)]
        [InlineAutoData(0.5, 42, null, false, null)]
        [InlineAutoData(0.5, 42, false, false, null)]
        [InlineAutoData(0.5, 42, true, true, null)]
        [InlineAutoData(0.5, 42, null, false)]
        [InlineAutoData(0.5, 42, false, false)]
        [InlineAutoData(0.5, 42, true, true)]
        public void Generate_Should_Follow_Process(
            double? location,
            int? seed,
            bool? value,
            bool expectedValue,
            Mock<IGenerator> generator
        )
        {
            // arrange
            var target = new BooleanDiscoverable(
                new BooleanDiscoverableArgs
                {
                    Location = location,
                    Seed = seed,
                    Generator = generator?.Object,
                    Value = value
                });

            // act
            var actual = target.Generate();

            // assert
            Assert.IsType<bool>(actual);
            Assert.Equal(target.Value, actual);
            Assert.Equal(location, target.Arguments.Location);
            Assert.Equal(seed, target.Arguments.Seed);
            Assert.Equal(generator?.Object, target.Arguments.Generator);
        }

        [Theory]
        [InlineAutoData(null, null, null, false, null)]
        [InlineAutoData(null, null, false, false, null)]
        [InlineAutoData(null, null, true, true, null)]
        [InlineAutoData(0.5, null, null, false, null)]
        [InlineAutoData(0.5, null, false, false, null)]
        [InlineAutoData(0.5, null, true, true, null)]
        [InlineAutoData(null, 42, null, false, null)]
        [InlineAutoData(null, 42, false, false, null)]
        [InlineAutoData(null, 42, true, true, null)]
        [InlineAutoData(0.5, 42, null, false, null)]
        [InlineAutoData(0.5, 42, false, false, null)]
        [InlineAutoData(0.5, 42, true, true, null)]
        [InlineAutoData(0.5, 42, null, false)]
        [InlineAutoData(0.5, 42, false, false)]
        [InlineAutoData(0.5, 42, true, true)]
        public void Mutate_Should_Follow_Process(
            double? location,
            int? seed,
            bool? value,
            bool expectedValue,
            Mock<IGenerator> generator
        )
        {
            // arrange
            var target = new BooleanDiscoverable(
                new BooleanDiscoverableArgs
                {
                    Location = location,
                    Seed = seed,
                    Generator = generator?.Object,
                    Value = value
                });

            // act
            var actual = target.Mutate();

            // assert
            Assert.IsType<bool>(actual);
            Assert.Equal(target.Value, actual);
            Assert.Equal(location, target.Arguments.Location);
            Assert.Equal(seed, target.Arguments.Seed);
            Assert.Equal(generator?.Object, target.Arguments.Generator);
        }

        [Fact]
        public void Constructor_Should_Initialise()
        {
            // arrange
            // act
            var actual = new BooleanDiscoverable();

            // assert
            Assert.NotNull(actual);
        }
    }
}