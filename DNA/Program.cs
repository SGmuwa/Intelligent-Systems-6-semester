using System;
using System.Runtime.InteropServices;
using org.mariuszgromada.math.mxparser;
using System.Threading;

namespace DNA
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Введите функцию. Например: f(x, y) = -(x^2 + y^2)");
            //Function f = new Function(Console.ReadLine());
            //Console.WriteLine($"Количество аргументов: {f.getArgumentsNumber()}");
            bool isNeedMax = true;//GetBool("True - поиск максимума. False - поиск минимума.");
            using CancellationTokenSource tokenSource = new CancellationTokenSource();
            double[] result = null;
            Thread thr = new Thread(_ => result = DNASearch.Search((x) => -((x[0]+213)*(x[0]+213)), 1, tokenSource.Token, isNeedMax, DNAWriter));
            Console.WriteLine("Нажмите на любую кнопку, чтобы остановить программу...");
            thr.Start();
            Console.ReadKey(true);
            Console.WriteLine("\nЗавершение работы...");
            tokenSource.Cancel();
            thr.Join();
            Console.WriteLine($"\nРезультат: {string.Join(' ', result)}");
        }

        private static Func<double[], double> FunctionMaker(Function f)
        {
            return arg => {
                while(true)
                {
                    try
                    {
                        return f.calculate(arg);
                    }
                    catch { continue; }
                }
            };
        }

        private static void DNAWriter(string message)
        {
            Console.Write($"\r{message}{new string(' ', Console.BufferWidth - message.Length - 1)}");
        }

        private static bool GetBool(string message)
        {
            bool output;
            do
            {
                Console.WriteLine(message);
            } while(!bool.TryParse(Console.ReadLine(), out output));
            return output;
        }
    }
}
