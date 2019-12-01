using System;
using System.Threading;

namespace DNA
{
    public class RandomThreadSafe : Random
    {
        public object sync = new object();

        public RandomThreadSafe(){}

        public RandomThreadSafe(int Seed) : base(Seed){}

        //
        // Summary:
        //     Returns a non-negative random integer.
        //
        // Returns:
        //     A 32-bit signed integer that is greater than or equal to 0 and less than System.Int32.MaxValue.
        public override int Next()
        {
            Monitor.Enter(sync);
            try
            {
                return base.Next();
            }
            finally {
                Monitor.Exit(sync);
            }
        }
        //
        // Summary:
        //     Returns a non-negative random integer that is less than the specified maximum.
        //
        // Parameters:
        //   maxValue:
        //     The exclusive upper bound of the random number to be generated. maxValue must
        //     be greater than or equal to 0.
        //
        // Returns:
        //     A 32-bit signed integer that is greater than or equal to 0, and less than maxValue;
        //     that is, the range of return values ordinarily includes 0 but not maxValue. However,
        //     if maxValue equals 0, maxValue is returned.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     maxValue is less than 0.
        public override int Next(int maxValue)
        {
            Monitor.Enter(sync);
            try
            {
                return base.Next(maxValue);
            }
            finally
            {
                Monitor.Exit(sync);
            }
        }
        //
        // Summary:
        //     Returns a random integer that is within a specified range.
        //
        // Parameters:
        //   minValue:
        //     The inclusive lower bound of the random number returned.
        //
        //   maxValue:
        //     The exclusive upper bound of the random number returned. maxValue must be greater
        //     than or equal to minValue.
        //
        // Returns:
        //     A 32-bit signed integer greater than or equal to minValue and less than maxValue;
        //     that is, the range of return values includes minValue but not maxValue. If minValue
        //     equals maxValue, minValue is returned.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     minValue is greater than maxValue.
        public override int Next(int minValue, int maxValue)
        {
            Monitor.Enter(sync);
            try
            {
                return base.Next(minValue, maxValue);
            }
            finally
            {
                Monitor.Exit(sync);
            }
        }
        //
        // Summary:
        //     Fills the elements of a specified array of bytes with random numbers.
        //
        // Parameters:
        //   buffer:
        //     An array of bytes to contain random numbers.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     buffer is null.
        public override void NextBytes(byte[] buffer)
        {
            Monitor.Enter(sync);
            try
            {
                base.NextBytes(buffer);
            }
            finally
            {
                Monitor.Exit(sync);
            }
        }
        //
        // Parameters:
        //   buffer:
        public override void NextBytes(Span<byte> buffer)
        {
            Monitor.Enter(sync);
            try
            {
                base.NextBytes(buffer);
            }
            finally
            {
                Monitor.Exit(sync);
            }
        }
        //
        // Summary:
        //     Returns a random floating-point number that is greater than or equal to 0.0,
        //     and less than 1.0.
        //
        // Returns:
        //     A double-precision floating point number that is greater than or equal to 0.0,
        //     and less than 1.0.
        public override double NextDouble()
        {
            Monitor.Enter(sync);
            try
            {
                return base.NextDouble();
            }
            finally
            {
                Monitor.Exit(sync);
            }
        }
        //
        // Summary:
        //     Returns a random floating-point number between 0.0 and 1.0.
        //
        // Returns:
        //     A double-precision floating point number that is greater than or equal to 0.0,
        //     and less than 1.0.
        protected override double Sample()
        {
            Monitor.Enter(sync);
            try
            {
                return base.Sample();
            }
            finally
            {
                Monitor.Exit(sync);
            }
        }
    }
}