using Discovery.Generator;
using OneOf;

namespace Discovery.Core
{
    public class DoubleDiscoverableBluePrint
    {
        public OneOf<int?, IIntDiscoverable> Seed { get; set; }
        public IGenerator Generator { get; set; }
        public double? LowerBound { get; set; }
        public double? UpperBound { get; set; }

        public double? Value { get; set; }
    }

    public class DoubleDiscoverableChromosome : IDoubleDiscoverable
    {
        private readonly IGenerator _generator;
        private readonly DoubleDiscoverableBluePrint _bluePrint;
        public DoubleDiscoverable Genotype { get; internal set; }
        public DoubleDiscoverableArgs Arguments { get; internal set; }
        public double? Phenotype { get; private set; }

        public DoubleDiscoverableChromosome(DoubleDiscoverableBluePrint blueprint = null)
        {
            _bluePrint = blueprint;
            _generator = blueprint?.Generator ?? new XorShiftPlusGenerator(
                _bluePrint?.Seed.Match(
                    value => value,
                    discoverable => discoverable.Generate()
                )
            );

            Phenotype = blueprint?.Value;

            Arguments = new DoubleDiscoverableArgs
            {
                Generator = _bluePrint?.Generator,
                Seed = _bluePrint?.Seed
                    .Match(
                        value => value,
                        discoverable => discoverable.Generate()
                    ),
                LowerBound = _bluePrint?.LowerBound,
                UpperBound = _bluePrint?.UpperBound,
                Value = _bluePrint?.Value
            };

            Genotype = new DoubleDiscoverable(Arguments);
        }


        public double? Generate()
        {
            return Phenotype = Genotype.Generate();
        }

        public double? Mutate(double? probability = 0.05)
        {
            if (probability.HasValue && _generator.NextDouble() < probability)
            {
                return Phenotype = Genotype.Mutate(probability);
            }

            return Phenotype;
        }
    }
}