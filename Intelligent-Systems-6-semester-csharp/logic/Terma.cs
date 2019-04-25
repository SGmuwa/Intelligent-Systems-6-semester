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
            PercentAndCharacteristicvalue Previouse = getPreviouse(v);
            PercentAndCharacteristicvalue Next = getNext(v);
            if (Previouse == Next)
                return Next.Percent;
            return (Next.Percent - Previouse.Percent) / (Next.CharacteristicValue - Previouse.CharacteristicValue) * v;
        }

        /// <summary>
        /// Получает предыдущую состовляющую термы по значению характеристики
        /// </summary>
        /// <param name="v">Значение характериситики.</param>
        /// <returns>Предыдущий состовляющий термы. Если такого нет, то вернёт null.</returns>
        private PercentAndCharacteristicvalue getPreviouse(double v)
        {
            list.Find
        }
    }
}