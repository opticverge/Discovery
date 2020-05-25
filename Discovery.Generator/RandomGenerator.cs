using System;

namespace Discovery.Generator
{
    public class RandomGenerator : IDoubleGenerator
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
    }
}
