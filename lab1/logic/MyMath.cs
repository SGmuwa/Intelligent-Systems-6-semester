using System;
using System.Collections.Generic;

namespace lab1.logic
{
    static class MyMath
    {
        /// <summary>
        /// Функция интерполяции.
        /// При двух значениях аргумента и значения функции
        /// ищет значение в свободном аргументе.
        /// </summary>
        public static double Interpolation(double x0, double x1, double y0, double y1, double x)
            => (x-x1)/(x0-x1)*y0 + (x-x0)/(x1-x0)*y1;

        /// <summary>
        /// Определяет, отсортирована ли последовательность.
        /// На сортировку проверяются только ключи.
        /// </summary>
        /// <param name="list">Последовательность, которая должна быть отсортирована.</param>
        public static bool IsSort<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> list) where TKey : IComparable<TKey>
        {
            TKey last = default;
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
            T last = default;
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
