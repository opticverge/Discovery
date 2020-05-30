using Discovery.Generator;

namespace Discovery.Core
{
    public class BooleanDiscoverableArgs
    {
        public int? Seed { get; set; }
        public double? Location { get; set; }
        public bool? Value { get; set; }

        public IGenerator Generator { get; set; }
    }

    public class BooleanDiscoverable : IBooleanDiscoverable
    {
        private readonly IGenerator _generator;

        public BooleanDiscoverable(BooleanDiscoverableArgs arguments = null)
        {
            Arguments = arguments;
            Value = arguments?.Value ?? default;
            _generator = arguments?.Generator ?? new XorShiftPlusGenerator(arguments?.Seed);
        }

        public BooleanDiscoverableArgs Arguments { get; set; }

        public bool Value { get; private set; }

        public bool Generate()
        {
            return Value = _generator.NextBool(Arguments?.Location);
        }

        public bool Mutate(double? probability = 0.05)
        {
            if (probability.HasValue && _generator.NextDouble() < probability) return Generate();

            return Value;
        }
    }
}