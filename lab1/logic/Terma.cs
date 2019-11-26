using lab1.logic;
using System;
using System.Collections;
using System.Collections.Generic;

namespace lab1
{
    /// <summary>
    /// Терма характеристики.
    /// </summary>
    public class Terma : IEnumerable<PercentAndCharacteristicvalue>
    {
        protected List<PercentAndCharacteristicvalue> list
            = new List<PercentAndCharacteristicvalue>();
        /// <summary>
        /// Создание термы.
        /// </summary>
        /// <param name="name">Имя термы. "Низкий", "Высокий", "Большой", ...</param>
        /// <param name="characteristic">Характеристика, к которой принадлежит терма.</param>
        public Terma(string name, Characteristic characteristic)
        {
            if (name?.Length == 0)
                throw new ArgumentException();
            Name = name ?? throw new ArgumentNullException();
            Parent = characteristic ?? throw new ArgumentNullException();
        }

        public string Name { get; }
        public Characteristic Parent { get; }
        public int Length => list.Count;

        public void Add(double percent, double characteristicValue)
        {
            list.Add(new PercentAndCharacteristicvalue(percent, characteristicValue));
            list.Sort();
        }

        public IEnumerator<PercentAndCharacteristicvalue> GetEnumerator()
            => list.GetEnumerator();

        public override string ToString()
        {
            return "{\"Name\":\"" +
                Name +
                "\", \"list\":\"" +
                list.ToString(", ") +
                "\" }";
        }

        IEnumerator IEnumerable.GetEnumerator()
            => list.GetEnumerator();

        /// <summary>
        /// Получить процент термы в заданной точке.
        /// </summary>
        /// <param name="v">Заданная точка, в которой нас интересует процент Термы.</param>
        /// <returns>Процент термы в заданной точке.</returns>
        public double GetPercentAt(double v)
        {
            if (Length == 0)
                throw new NotSupportedException();
            PercentAndCharacteristicvalue Previous = GetPrevious(v);
            PercentAndCharacteristicvalue Next = GetNext(v);
            if (Previous == Next)
                return Next.Percent;
            return MyMath.Interpolation(Previous.CharacteristicValue, Next.CharacteristicValue, Previous.Percent, Next.Percent, v);
        }


        private PercentAndCharacteristicvalue GetNext(double v)
        {
            PercentAndCharacteristicvalue output = null;
            foreach(PercentAndCharacteristicvalue contain in this)
            {
                if (output == null)
                    output = contain;
                if (output.CharacteristicValue <= v)
                    output = contain;
                else // Работает только для сортированных данных.
                    return output;
            }
            return output;
        }

        /// <summary>
        /// Получает предыдущую составляющую термы по значению характеристики
        /// </summary>
        /// <param name="v">Значение характеристики.</param>
        /// <returns>Предыдущий составляющий термы. Если такого нет, то вернёт null.</returns>
        private PercentAndCharacteristicvalue GetPrevious(double v)
        {
            PercentAndCharacteristicvalue output = null;
            List<PercentAndCharacteristicvalue> reverseList
                = new List<PercentAndCharacteristicvalue>(this);
            reverseList.Reverse();
            foreach (PercentAndCharacteristicvalue contain in reverseList)
            {
                if (output == null)
                    output = contain;
                if (output.CharacteristicValue >= v)
                    output = contain;
                else // Работает только для сортированных данных.
                    return output;
            }
            return output;
        }
    }
}