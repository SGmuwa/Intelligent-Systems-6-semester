using System;
using System.Collections;
using System.Collections.Generic;

namespace Intelligent_Systems_6_semester_csharp
{
    /// <summary>
    /// Терма характеристики.
    /// </summary>
    public class Terma : IEnumerable
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
            Name = name ?? throw new ArgumentException();
            Perent = characteristic ?? throw new ArgumentNullException();
        }

        public string Name { get; }
        public Characteristic Perent { get; }
        public int Length => list.Count;

        public void Add(double percent, double characteristicValue)
            => list.Add(new PercentAndCharacteristicvalue(percent, characteristicValue));

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}