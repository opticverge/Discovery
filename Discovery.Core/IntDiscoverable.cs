using System;
using Discovery.Generator;

namespace Discovery.Core
{
    public class IntDiscoverableArgs
    {
        public int? Seed { get; set; }
        public IGenerator Generator { get; set; }
        public int? LowerBound { get; set; }
        public int? UpperBound { get; set; }
        public int? Value { get; set; }
    }

    public class IntDiscoverable : IIntDiscoverable
    {
        private readonly int _lowerBound;

        private readonly int _upperBound;
        public IGenerator _generator;

        public IntDiscoverable(IntDiscoverableArgs args = null)
        {
            Arguments = args;
            Value = args?.Value ?? default;
            _generator = args?.Generator ?? new XorShiftPlusGenerator(args?.Seed);

            if (args != null)
            {
                var lowerBoundHasValue = Convert.ToBoolean(args.LowerBound.HasValue);
                var upperBoundHasValue = Convert.ToBoolean(args.UpperBound.HasValue);

                _lowerBound = lowerBoundHasValue ? args.LowerBound.Value : default;
                _upperBound = upperBoundHasValue ? args.UpperBound.Value : default;

                IsBounded = lowerBoundHasValue && upperBoundHasValue;

                if (lowerBoundHasValue && upperBoundHasValue && _upperBound < _lowerBound)
                    throw new ArgumentException(
                        $"{nameof(args.UpperBound)} ({_upperBound}) must be greater than {nameof(args.LowerBound)} ({_lowerBound})");
            }
        }

        public IntDiscoverableArgs Arguments { get; }

        public int Value { get; private set; }

        public bool IsBounded { get; }

        public int Generate()
        {
            return Value = IsBounded ? _generator.NextInt(_lowerBound, _upperBound) : _generator.NextInt();
        }

        public int Mutate(double? probability)
        {
            if (probability.HasValue && _generator.NextDouble() < probability) return Generate();

            return Value;
        }
    }
}
