using System;

namespace Discovery.Generator
{
    public class XorShiftPlusGenerator : IGenerator
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XorShiftPlusBase" /> class.
        ///     Constructs a new  generator using two
        ///     random Guid hash codes as a seed.
        /// </summary>
        public XorShiftPlusGenerator()
        {
            x_ = (ulong) Guid.NewGuid().GetHashCode();
            y_ = (ulong) Guid.NewGuid().GetHashCode();
        }

        public XorShiftPlusGenerator(ulong? seed)
        {
            var preparedSeed = seed ?? (ulong) DateTime.Now.Ticks;

            x_ = preparedSeed << 3;
            y_ = preparedSeed >> 3;
        }

        public XorShiftPlusGenerator(int? seed)
        {
            var preparedSeed = seed.HasValue ? (ulong) seed : (ulong) DateTime.Now.Ticks;

            x_ = preparedSeed << 3;
            y_ = preparedSeed >> 3;
        }

        public double NextDouble()
        {
            double _;
            ulong temp_x, temp_y, temp_z;

            temp_x = y_;
            x_ ^= x_ << 23;
            temp_y = x_ ^ y_ ^ (x_ >> 17) ^ (y_ >> 26);

            temp_z = temp_y + y_;
            _ = DOUBLE_UNIT * (0x7FFFFFFF & temp_z);

            x_ = temp_x;
            y_ = temp_y;

            return _;
        }

        public double NextDouble(double lowerBound, double upperBound)
        {
            return NextDouble() * (upperBound - lowerBound) + lowerBound;
        }

        public bool NextBool(double? loc)
        {
            return NextDouble() < (loc ?? 0.5);
        }

        public int NextInt()
        {
            int _;
            ulong temp_x, temp_y;

            temp_x = y_;
            x_ ^= x_ << 23;
            temp_y = x_ ^ y_ ^ (x_ >> 17) ^ (y_ >> 26);

            _ = (int) (temp_y + y_);

            x_ = temp_x;
            y_ = temp_y;

            return _;
        }

        public int NextInt(int min, int max)
        {
            return (int) (NextDouble() * (max - min) + min);
        }

        #region Data Members

        // Constants
        protected const double DOUBLE_UNIT = 1.0 / (int.MaxValue + 1.0);

        // State Fields
        protected ulong x_;
        protected ulong y_;

        // Buffer for optimized bit generation.
        protected ulong buffer_;
        protected ulong bufferMask_;

        #endregion
    }
}