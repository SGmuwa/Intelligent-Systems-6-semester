using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.logic
{
    static class MyMath
    {
        /// <summary>
        /// Функция интерпритации.
        /// При двух значениях аргумента и значеня функции
        /// ищет значение в свободном аргументе.
        /// </summary>
        public static double SearchYifX0X1Y0YX(double x0, double x1, double y0, double y1, double x)
        {
            return (y1 - y0) / (x1 - x0) * x;
        }

        /// <summary>
        /// Определяет, отсортирована ли последовательность.
        /// На сортировку проверяются только ключи.
        /// </summary>
        /// <param name="list">Последовательность, которая должна быть отсортирована.</param>
        public static bool IsSort<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> list) where TKey : IComparable<TKey>
        {
            TKey last = default(TKey);
            bool init = false;
            foreach (var t in list)
            {
                if (!init)
                    last = t.Key;
                else if (t.Key.CompareTo(last) < 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Определяет, отсортирована ли последовательность.
        /// </summary>
        /// <param name="list">Последовательность, которая должна быть отсортирована.</param>
        public static bool IsSort<T>(this IEnumerable<T> list) where T : IComparable
        {
            T last = default(T);
            bool init = false;
            foreach(T t in list)
            {
                if (!init)
                    last = t;
                else if (t.CompareTo(last) < 0)
                    return false;
            }
            return true;
        }
    }
}
