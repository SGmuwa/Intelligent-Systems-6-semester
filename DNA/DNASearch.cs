using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace DNA
{
    public class DNASearch
    {
        public readonly static DNASearch Instance = new DNASearch();

        private DNASearch() { }

        private readonly static Random random = new RandomThreadSafe();

        public static double[] Search(Func<double[], double> func, ushort countArgs, CancellationToken token, bool isNeedMax, Action<string> debug = null)
            => Instance.Search(func, countArgs, isNeedMax, token, debug);

        public double[] Search(Func<double[], double> func, ushort countArgs, bool isNeedMax, CancellationToken token, Action<string> debug = null)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));
            if (countArgs <= 0)
                return new double[0];
            double[][] DNAs = CreateDNAs(1024 * 512, countArgs);
            double[] results = new double[DNAs.LongLength];
            long[] top = new long[10];
            while (true)
            {
                Calculate(func, DNAs, results);
                SearchTop(results, top, isNeedMax);
                SortTop(results, top, isNeedMax);
                debug?.Invoke($"{string.Join("; ", DNAs[top[0]])} = {results[top[0]]}");
                if (token.IsCancellationRequested)
                    break;
                MoveTopToBegin(DNAs, top);
                long countToMerge = (DNAs.LongLength - top.LongLength) * 1 / 2;
                long countToRandom = DNAs.LongLength - countToMerge - top.LongLength;
                Parallel.Invoke(
                    () => GenerateDNAsMerge(DNAs, top.LongLength, countToMerge),
                    () => GenerateDNAsRandom(DNAs, top.LongLength + countToMerge, countToRandom)
                );
            }
            return DNAs[top[0]];
        }

        private void GenerateDNAsRandom(double[][] DNAs, long beginGenerate, long count)
        {
            int countBits = DNAs[0].GetLength(0) * sizeof(double) * 8;
            Parallel.For(beginGenerate, beginGenerate + count, i => {
                DNAs[i] = random.NextBigInteger(countBits, true).ToByteArray().Incrise(countBits / 8).GetDoubles();
            });
        }

        private void GenerateDNAsMerge(double[][] DNAs, long beginGenerate, long count)
        {
            Parallel.For(beginGenerate, beginGenerate + count, i => {
                GenerateDNAMerge(DNAs, i, beginGenerate);
            });

            static void GenerateDNAMerge(double[][] DNAs, long indexPaste, long countTop)
            {
                long index1 = random.NextLong(0, countTop);
                long index2 = random.NextLong(0, countTop - 1);
                if(index2 >= index1)
                    index2++;
                BigInteger DNA1 = new BigInteger(DNAs[index1].GetBytes(), true);
                BigInteger DNA2 = new BigInteger(DNAs[index2].GetBytes(), true);
                BigInteger needRandom = DNA1 ^ DNA2;
                BigInteger randomNumber = needRandom & random.NextBigInteger(needRandom.GetBitLength(), true);
                BigInteger result = DNA1 & DNA2 | randomNumber;
                DNAs[indexPaste] = result.ToByteArray(true).Incrise(DNAs[index1].LongLength).GetDoubles();
            }
        }

        private void SortTop(double[] results, long[] top, bool isNeedMax)
        {
            Array.Sort(top, (l, r) => results[l].CompareTo(results[r]) * (isNeedMax ? -1 : 1));
        }

        private void MoveTopToBegin(double[][] DNAs, long[] top)
        {
            double[][] source = (double[][])DNAs.Clone();
            MoveToBegin();

            void MoveToBegin()
            {
                Parallel.For(0, top.Length, i => {
                    DNAs[i] = source[top[i]];
                    top[i] = i;
                });
            }
        }

        private void SearchTop(double[] results, long[] top, bool isNeedMax)
        {
            
            Parallel.For(0, top.Length, i => {
                top[i] = i;
            });
            long minmaxIndexTop = SearchMinmax();
            for (long i = top.LongLength; i < results.LongLength; i++)
            {
                if (isNeedMax && results[top[minmaxIndexTop]] < results[i]
                || !isNeedMax && results[top[minmaxIndexTop]] > results[i])
                {
                    top[minmaxIndexTop] = i;
                    minmaxIndexTop = SearchMinmax();
                }
            }

            long SearchMinmax()
            {
                long minmax = 0;
                for (long i = 1; i < top.LongLength; i++)
                {
                    if (isNeedMax && results[top[minmax]] < results[top[i]]
                    || !isNeedMax && results[top[minmax]] > results[top[i]])
                        minmax = i;
                }
                return minmax;
            }
        }

        private void Calculate(Func<double[], double> func, double[][] args, double[] results)
        {
            if (args.GetLongLength(0) != results.GetLongLength(0))
                throw new ArgumentOutOfRangeException();
            Parallel.For(0, args.LongLength, (i) =>
            {
                results[i] = func(args[i]);
            });
        }

        private double[][] CreateDNAs(long countDNA, ushort countArgs)
        {
            double[][] output = new double[countDNA][];
            Parallel.For(0, countDNA, (i) =>
            {
                output[i] = random.NextDoubleFullArray(countArgs);
            });
            return output;
        }
    }
}