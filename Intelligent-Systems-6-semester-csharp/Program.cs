using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public static class PrintArray
    {
        public static string ToString<T>(this IEnumerable<T> l, string separator)
        {
            return "[" + string.Join(separator, l.Select(i => i.ToString()).ToArray()) + "]";
        }
    }
}
