using Xunit;

namespace Discovery.Core.Tests
{
    public class BooleanDiscoverableChromosomeTests
    {
        [Fact]
        public void Constructor_Should_Initialise()
        {
            // arrange
            // act
            var target = new BooleanDiscoverableChromosome();

            // assert
            Assert.NotNull(target);
            Assert.NotNull(target.Arguments);
            Assert.NotNull(target.Genotype);
        }

        [Fact]
        public void Generate_DefaultArgs_Should_Follow_Process()
        {
            // arrange
            var target = new BooleanDiscoverableChromosome();

            // act
            var actual = target.Generate();

            // assert
            Assert.IsType<bool>(actual);
        }

        [Fact]
        public void Generate_Discoverable_Location_Should_Follow_Process()
        {
            // arrange
            var target = new BooleanDiscoverableChromosome(
                new BooleanDiscoverableBluePrint
                {
                    Location = new DoubleDiscoverable()
                }
            );

            // act
            var generated = target.Generate();
            var mutated = target.Mutate(1.0);

            // assert
            Assert.IsType<bool>(generated);
            Assert.IsType<bool>(mutated);
            Assert.NotNull(target.Arguments);
            Assert.NotNull(target.Genotype);
        }

        [Fact]
        public void Mutate_DefaultArgs_Should_Follow_Process()
        {
            // arrange
            var target = new BooleanDiscoverableChromosome();

            // act
            var generated = target.Generate();
            var mutated = target.Mutate(1.0);

            // assert
            Assert.IsType<bool>(generated);
            Assert.IsType<bool>(mutated);
            Assert.NotNull(target.Arguments);
            Assert.NotNull(target.Genotype);
        }
    }
}