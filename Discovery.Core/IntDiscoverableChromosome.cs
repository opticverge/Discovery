using Discovery.Generator;
using OneOf;

namespace Discovery.Core
{
    public class IntDiscoverableBluePrint
    {
        public OneOf<int?, IIntDiscoverable> Seed { get; set; }
        public IGenerator Generator { get; set; }
        public int? LowerBound { get; set; }
        public int? UpperBound { get; set; }
        public int? Value { get; set; }
    }

    public class IntDiscoverableChromosome : IIntDiscoverable
    {
        private readonly IGenerator _generator;
        private readonly IntDiscoverableBluePrint _bluePrint;
        public IntDiscoverable Genotype { get; internal set; }
        public IntDiscoverableArgs Arguments { get; internal set; }
        public int? Phenotype { get; private set; }

        public IntDiscoverableChromosome(IntDiscoverableBluePrint blueprint = null)
        {
            _bluePrint = blueprint;
            _generator = blueprint?.Generator ?? new XorShiftPlusGenerator(
                _bluePrint?.Seed.Match(
                    value => value,
                    discoverable => discoverable.Generate()
                )
            );

            Arguments = new IntDiscoverableArgs
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

            Genotype = new IntDiscoverable(Arguments);

            Phenotype = blueprint?.Value;
        }


        public int? Generate()
        {
            return Phenotype = Genotype.Generate();
        }

        public int? Mutate(double? probability = 0.05)
        {
            if (probability.HasValue && _generator.NextDouble() < probability)
            {
                return Phenotype = Genotype.Mutate(probability);
            }

            return Phenotype;
        }
    }
}
