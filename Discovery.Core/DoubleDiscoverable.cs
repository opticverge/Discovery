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
        public DoubleDiscoverableArgs Arguments;

        private readonly double _lowerBound;

        private readonly double _upperBound;

        public readonly bool IsBounded;

        private readonly IDoubleGenerator _generator;

        public DoubleDiscoverable(DoubleDiscoverableArgs arguments = null)
        {
            Arguments = arguments;

            Value = arguments?.Value;

            _generator = arguments?.Generator ?? new XorShiftPlusGenerator(arguments?.Seed);

            var lowerBoundHasValue = Convert.ToBoolean(arguments?.LowerBound.HasValue);
            var upperBoundHasValue = Convert.ToBoolean(arguments?.UpperBound.HasValue);
            _lowerBound = lowerBoundHasValue ? arguments.LowerBound.Value : default;
            _upperBound = upperBoundHasValue ? arguments.UpperBound.Value : default;

            IsBounded = lowerBoundHasValue && upperBoundHasValue;

            if (lowerBoundHasValue && upperBoundHasValue && _upperBound < _lowerBound)
                throw new ArgumentException(
                    $"{nameof(arguments.UpperBound)} ({_upperBound}) must be greater than {nameof(arguments.LowerBound)} ({_lowerBound})");
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
