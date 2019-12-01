using System;
using System.Linq;
using System.Numerics;

namespace DNA
{
    internal static class StaticTools
    {
        /// <summary>
        /// Returns a random long from min (inclusive) to max (exclusive)
        /// </summary>
        /// <param name="random">The given random instance</param>
        /// <param name="min">The inclusive minimum bound</param>
        /// <param name="max">The exclusive maximum bound.  Must be greater than min</param>
        public static long NextLong(this Random random, long min, long max)
        {
            if (max <= min)
                throw new ArgumentOutOfRangeException("max", "max must be > min!");

            //Working with ulong so that modulo works correctly with values > long.MaxValue
            ulong uRange = (ulong)(max - min);

            //Prevent a modolo bias; see https://stackoverflow.com/a/10984975/238419
            //for more information.
            //In the worst case, the expected number of calls is 2 (though usually it's
            //much closer to 1) so this loop doesn't really hurt performance at all.
            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange) + min;
        }

        /// <summary>
        /// Returns a random long from 0 (inclusive) to max (exclusive)
        /// </summary>
        /// <param name="random">The given random instance</param>
        /// <param name="max">The exclusive maximum bound.  Must be greater than 0</param>
        public static long NextLong(this Random random, long max)
        {
            return random.NextLong(0, max);
        }

        /// <summary>
        /// Returns a random long over all possible values of long (except long.MaxValue, similar to
        /// random.Next())
        /// </summary>
        /// <param name="random">The given random instance</param>
        public static long NextLong(this Random random)
        {
            return random.NextLong(long.MinValue, long.MaxValue);
        }

        public static ulong NextULong(this Random ran)
        {
            byte[] output = new byte[sizeof(ulong)];
            ran.NextBytes(output);
            return BitConverter.ToUInt64(output);
        }

        public static ulong NextULong(this Random ran, ulong min, ulong max)
        {
            return (ran.NextULong() % (max - min)) + min;
        }

        public static double NextDoubleFull(this Random ran)
        {
            byte[] output = new byte[sizeof(double)];
            ran.NextBytes(output);
            return BitConverter.ToDouble(output);
        }

        public static double[] NextDoubleFullArray(this Random ran, long count)
        {
            double[] output = new double[count];
            for (long i = 0; i < output.LongLength; i++)
                output[i] = ran.NextDoubleFull();
            return output;
        }

        public static bool Contains<T>(this Array array, T search)
        {
            foreach(var o in array)
            {
                if(o.Equals(search))
                    return true;
            }
            return false;
        }

        public static void Swap(this Array array, long indexA, long indexB)
        {
            var buffer = array.GetValue(indexA);
            array.SetValue(array.GetValue(indexB), indexA);
            array.SetValue(buffer, indexB);
        }

        public static byte[] GetBytes(this double[] values)
        {
            return values.SelectMany(value => BitConverter.GetBytes(value)).ToArray();
        }

        public static double[] GetDoubles(this byte[] bytes)
        {
            if(bytes.Length % sizeof(double) != 0)
            {
                byte[] bytesNew = new byte[bytes.Length + sizeof(double) - bytes.Length % sizeof(double)];
                long difference = bytesNew.LongLength - bytes.LongLength;
                for(long i = 0; i < bytes.LongLength; i++)
                    bytesNew[i] = bytes[i];
                bytes = bytesNew;
            }
            return Enumerable.Range(0, bytes.Length / sizeof(double))
                .Select(offset => BitConverter.ToDouble(bytes, offset * sizeof(double)))
                .ToArray();
        }

        public static BigInteger NextBigInteger(this Random ran, int countBits, bool isUnsigned = false)
        {
            byte[] buffer = new byte[countBits / 8 + (countBits % 8 == 0 ? 0 : 1)];
            ran.NextBytes(buffer);
            BigInteger output = new BigInteger(buffer, isUnsigned);
            return output >> countBits % 8;
        }

        public static int GetBitLength(this BigInteger that)
        {
            int bitLength = 0;
            if(that.Sign < 0)
                that = -that;
            do
            {
                bitLength++;
            } while((that >>= 1) != 0);
            return bitLength;
        }

        /// <summary>
        /// Изменяет размер массива под заданных размер.
        /// В конце заполняется нулями.
        /// </summary>
        /// <param name="array">Исходный массив.</param>
        /// <param name="sizeMust">Какого он должен быть размера.</param>
        /// <returns>
        /// Новый массив из исходного размера sizeMust.
        /// Либо возвращает исходный массив, если он нужного размера.
        /// </returns>
        public static byte[] Incrise(this byte[] array, long sizeMust)
        {
            if(array.LongLength == sizeMust)
                return array;
            byte[] output = new byte[sizeMust];
            for(long i = 0; i < output.LongLength && i < array.LongLength; i++)
                output[i] = array[i];
            return output;
        }
    }
}