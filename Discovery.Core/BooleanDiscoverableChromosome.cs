using System;
using Discovery.Generator;
using OneOf;

namespace Discovery.Core
{
    public class BooleanDiscoverableBluePrint
    {
        public OneOf<int?, IIntDiscoverable> Seed { get; set; }
        public OneOf<double?, IDoubleDiscoverable> Location { get; set; }
        public IGenerator Generator { get; set; }
        public bool? Value { get; set; }
    }

    public class BooleanDiscoverableChromosome : IBooleanDiscoverable
    {
        private readonly IGenerator _generator;
        private readonly BooleanDiscoverableBluePrint _bluePrint;
        public BooleanDiscoverable Genotype { get; internal set; }
        public BooleanDiscoverableArgs Arguments { get; internal set; }
        public bool Phenotype { get; private set; }

        public BooleanDiscoverableChromosome(BooleanDiscoverableBluePrint bluePrint = null)
        {
            _bluePrint = bluePrint;
            _generator = bluePrint?.Generator ?? new XorShiftPlusGenerator(
                _bluePrint?.Seed.Match(
                    value => value,
                    discoverable => discoverable.Generate()
                )
            );

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

            Phenotype = Convert.ToBoolean(bluePrint?.Value);
        }

        public bool Generate()
        {
            return Phenotype = Genotype.Generate();
        }

        public bool Mutate(double? probability = 0.05)
        {
            if (probability.HasValue && _generator.NextDouble() < probability)
            {
                Arguments.Location = _bluePrint?.Location.Match(
                    value => value,
                    discoverable => discoverable.Mutate(probability)
                ) ?? Arguments.Location;

                Genotype.Arguments = Arguments;

                Phenotype = Genotype.Mutate(probability);
            }

            return Phenotype;
        }
    }
}
