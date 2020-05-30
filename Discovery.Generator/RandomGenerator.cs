using System;

namespace Discovery.Generator
{
    public class RandomGenerator : IGenerator
    {
        private readonly Random _random;

        public RandomGenerator()
        {
            _random = new Random();
        }

        public RandomGenerator(int? seed)
        {
            seed ??= (int) DateTime.Now.Ticks;
            _random = new Random(seed.Value);
        }

        public double NextDouble()
        {
            return _random.NextDouble();
        }

        public double NextDouble(double lowerBound, double upperBound)
        {
            return _random.NextDouble() * (upperBound - lowerBound) + lowerBound;
        }

        public bool NextBool(double? loc = 0.5)
        {
            return _random.NextDouble() < (loc ?? 0.5);
        }

        public int NextInt()
        {
            return _random.Next();
        }

        public int NextInt(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
