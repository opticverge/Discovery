using Discovery.Generator;
using OneOf;

namespace Discovery.Core
{
    public class BooleanDiscoverableBluePrint
    {
        public OneOf<int?, IIntDiscoverable> Seed { get; set; }
        public OneOf<double?, IDoubleDiscoverable> Location { get; set; }
        public IGenerator Generator { get; set; }
    }

    public class BooleanDiscoverableChromosome : IBooleanDiscoverable
    {
        private readonly IGenerator _generator;
        private readonly BooleanDiscoverableBluePrint _bluePrint;

        public BooleanDiscoverableChromosome(BooleanDiscoverableBluePrint bluePrint = null)
        {
            _bluePrint = bluePrint;
            _generator = bluePrint?.Generator ?? new XorShiftPlusGenerator(
                _bluePrint?.Seed.Match(
                    value => value,
                    discoverable => discoverable.Generate()
                )
            );
        }

        public BooleanDiscoverable Genotype { get; internal set; }
        public BooleanDiscoverableArgs Arguments { get; internal set; }
        public bool Phenotype { get; private set; }


        public bool Generate()
        {
            Arguments = new BooleanDiscoverableArgs
            {
                Location = _bluePrint?.Location
                    .Match(
                        value => value,
                        discoverable => discoverable.Generate()
                    ),
                Generator = _bluePrint?.Generator,
                Seed = _bluePrint?.Seed
                    .Match(
                        value => value,
                        discoverable => discoverable.Generate()
                    )
            };

            Genotype = new BooleanDiscoverable(Arguments);

            return Phenotype = Genotype.Generate();
        }

        public bool Mutate(double? probability = 0.05)
        {
            if (probability.HasValue)
            {
                if (_generator.NextDouble() < probability)
                    Arguments.Location = _bluePrint?.Location.Match(
                        value => value,
                        discoverable => discoverable.Mutate(probability)
                    ) ?? Arguments.Location;

                if (_generator.NextDouble() < probability)
                    Arguments.Seed = _bluePrint?.Seed.Match(
                        value => value,
                        discoverable => discoverable.Mutate(probability)
                    ) ?? Arguments.Seed;

                Genotype.Arguments = Arguments;

                Phenotype = Genotype.Mutate(probability);
            }

            return Phenotype;
        }
    }
}