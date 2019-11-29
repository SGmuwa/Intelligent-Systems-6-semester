using System;
using org.mariuszgromada.math.mxparser;

namespace dna
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Function My = new Function("My(a) = a*2");
            Argument a = new Argument(nameof(a), 2);
            Console.WriteLine(My.calculate(a));
        }
    }
}
