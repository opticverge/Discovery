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
        private readonly DoubleDiscoverableArgs _args;

        private readonly double _lowerBound;

        private readonly double _upperBound;

        public readonly bool IsBounded;
        private readonly IDoubleGenerator _generator;

        public DoubleDiscoverable(DoubleDiscoverableArgs args = null)
        {
            _args = args;

            Value = args?.Value;

            _generator = args?.Generator ?? new XorShiftPlusGenerator(args?.Seed);

            var lowerBoundHasValue = Convert.ToBoolean(args?.LowerBound.HasValue);
            var upperBoundHasValue = Convert.ToBoolean(args?.UpperBound.HasValue);
            _lowerBound = lowerBoundHasValue ? args.LowerBound.Value : default;
            _upperBound = upperBoundHasValue ? args.UpperBound.Value : default;

            IsBounded = lowerBoundHasValue && upperBoundHasValue;

            if (lowerBoundHasValue && upperBoundHasValue && _upperBound < _lowerBound)
                throw new ArgumentException(
                    $"{nameof(args.UpperBound)} ({_upperBound}) must be greater than {nameof(args.LowerBound)} ({_lowerBound})");
        }

        public double? Value { get; private set; }

        public virtual double? Generate()
        {
            return Value = IsBounded ? _generator.NextDouble(_lowerBound, _upperBound) : _generator.NextDouble();
        }

        public virtual double? Mutate(double? probability = 0.05)
        {
            if (probability.HasValue && _generator.NextDouble() < probability) return Generate();

            return Value;
        }
    }
}
