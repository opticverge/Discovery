using System;
using Discovery.Generator;

namespace Discovery.Core
{
    public class DoubleDiscoverableArgs
    {
        public int? Seed { get; set; }

        public double? LowerBound { get; set; }
        public double? UpperBound { get; set; }
        public IDoubleGenerator Generator { get; set; }
        public double? Value { get; set; }
    }

    public class DoubleDiscoverable : IDoubleDiscoverable
    {
        private IDoubleGenerator _generator;

        private readonly DoubleDiscoverableArgs _args;

        public readonly bool IsBounded;

        private readonly double _lowerBound;

        private readonly double _upperBound;

        public double? Value { get; private set; }

        public DoubleDiscoverable(DoubleDiscoverableArgs args = null)
        {
            _args = args;

            Value = args?.Value;

            _generator = args?.Generator ?? new RandomGenerator(args?.Seed);

            var lowerBoundHasValue = Convert.ToBoolean(args?.LowerBound.HasValue);
            var upperBoundHasValue = Convert.ToBoolean(args?.UpperBound.HasValue);
            _lowerBound = lowerBoundHasValue ? args.LowerBound.Value : default;
            _upperBound = upperBoundHasValue ? args.UpperBound.Value : default;

            IsBounded = lowerBoundHasValue && upperBoundHasValue;

            if (lowerBoundHasValue && upperBoundHasValue && _upperBound < _lowerBound)
            {
                throw new ArgumentException(
                    $"{nameof(args.UpperBound)} ({_upperBound}) must be greater than {nameof(args.LowerBound)} ({_lowerBound})");
            }
        }

        public double? Generate()
        {
            return Value = IsBounded ? _generator.NextDouble(_lowerBound, _upperBound) : _generator.NextDouble();
        }

        public double? Mutate(double? probability = 0.05)
        {
            return Value = probability.HasValue && _generator.NextDouble() < probability ? Generate() : Value;
        }
    }
}
