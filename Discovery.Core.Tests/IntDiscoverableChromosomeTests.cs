using AutoFixture.Xunit2;
using Discovery.Generator;
using Moq;
using OneOf;
using Xunit;

namespace Discovery.Core.Tests
{
    public class IntDiscoverableChromosomeTests
    {
        [Fact]
        public void Constructor_Should_Initialise()
        {
            // arrange
            // act
            var target = new IntDiscoverableChromosome();

            // assert
            Assert.NotNull(target);
            Assert.NotNull(target.Genotype);
            Assert.NotNull(target.Arguments);
            Assert.Null(target.Phenotype);
        }

        [Theory]
        [InlineAutoData(null, null, null, null)]
        [InlineAutoData(null, null, null, null, null)]
        public void Constructor_Should_Initialise_With_Arguments(
            int? lowerBound,
            int? upperBound,
            int? value,
            OneOf<int?, IIntDiscoverable> seed,
            Mock<IGenerator> generatorMock
        )
        {
            // arrange
            // act
            var target = new IntDiscoverableChromosome(
                new IntDiscoverableBluePrint
                {
                    LowerBound = lowerBound,
                    UpperBound = upperBound,
                    Value = value,
                    Seed = seed,
                    Generator = generatorMock?.Object
                }
            );

            // assert
            Assert.NotNull(target);
            Assert.NotNull(target.Genotype);
            Assert.NotNull(target.Arguments);
            Assert.Null(target.Phenotype);
            Assert.Equal(value, target.Phenotype);
        }

        [Fact]
        public void Generate_DefaultArgs_Should_Follow_Process()
        {
            // arrange
            var target = new IntDiscoverableChromosome();

            // act
            var actual = target.Generate();

            // assert
            Assert.IsType<int>(actual);
            Assert.NotNull(target.Arguments);
            Assert.NotNull(target.Genotype);
            Assert.Equal(target.Phenotype, actual);
        }

        [Theory]
        [InlineAutoData(null, null, null, null, null)]
        [InlineAutoData(0, null, null, null, null)]
        [InlineAutoData(0, 10, null, null, null)]
        [InlineAutoData(null, 10, null, null, null)]
        [InlineAutoData(null, null, 5, null, null)]
        [InlineAutoData(null, null, null, 1, null)]
        public void Generate_Args_Should_Follow_Process(
            int? lowerBound,
            int? upperBound,
            int? value,
            OneOf<int?, IIntDiscoverable> seed,
            Mock<IGenerator> generatorMock
        )
        {
            // arrange
            var target = new IntDiscoverableChromosome(
                new IntDiscoverableBluePrint
                {
                    LowerBound = lowerBound,
                    UpperBound = upperBound,
                    Seed = seed,
                    Value = value,
                    Generator = generatorMock?.Object
                }
            );

            // act
            var actual = target.Generate();

            // assert
            Assert.IsType<int>(actual);
            Assert.NotNull(target.Arguments);
            Assert.NotNull(target.Genotype);
            Assert.Equal(lowerBound, target.Arguments?.LowerBound);
            Assert.Equal(upperBound, target.Arguments?.UpperBound);
            Assert.Equal(value, target.Arguments?.Value);
            Assert.Equal(seed, target.Arguments?.Seed);
            Assert.Equal(generatorMock?.Object, target.Arguments?.Generator);
            Assert.Equal(target.Phenotype, actual);
        }

        [Theory]
        [InlineAutoData(null, null, null, null, null)]
        [InlineAutoData(0, null, null, null, null)]
        [InlineAutoData(0, 10, null, null, null)]
        [InlineAutoData(null, 10, null, null, null)]
        [InlineAutoData(null, null, 5, null, null)]
        [InlineAutoData(null, null, null, 1, null)]
        public void Mutate_Args_Should_Follow_Process(
            int? lowerBound,
            int? upperBound,
            int? value,
            OneOf<int?, IIntDiscoverable> seed,
            Mock<IGenerator> generatorMock
        )
        {
            // arrange
            var target = new IntDiscoverableChromosome(
                new IntDiscoverableBluePrint
                {
                    LowerBound = lowerBound,
                    UpperBound = upperBound,
                    Seed = seed,
                    Value = value,
                    Generator = generatorMock?.Object
                }
            );

            // act
            var actual = target.Mutate(1.0);

            // assert
            Assert.IsType<int>(actual);
            Assert.NotNull(target.Arguments);
            Assert.NotNull(target.Genotype);
            Assert.Equal(lowerBound, target.Arguments?.LowerBound);
            Assert.Equal(upperBound, target.Arguments?.UpperBound);
            Assert.Equal(value, target.Arguments?.Value);
            Assert.Equal(seed, target.Arguments?.Seed);
            Assert.Equal(generatorMock?.Object, target.Arguments?.Generator);
            Assert.Equal(target.Phenotype, actual);
        }
    }
}
