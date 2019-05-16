using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace lab1.logic.lab2
{
    /// <summary>
    /// Класс представляет выделенную
    /// територию термы.
    /// </summary>
    public class SelectedAreaOfTerma : IDictionary<CharacteristicValue, TermaValue>
    {
        private readonly Dictionary<CharacteristicValue, TermaValue> points = new Dictionary<CharacteristicValue, TermaValue>();

        public Terma Terma { get; }

        public SelectedAreaOfTerma(Terma terma)
            => this.Terma = terma;

        public TermaValue this[CharacteristicValue key] { get => points[key]; set => points[key] = value; }

        public ICollection<CharacteristicValue> Keys => points.Keys;

        public ICollection<TermaValue> Values => points.Values;

        public int Count => points.Count;

        public bool IsReadOnly => ((IDictionary)points).IsReadOnly;

        public void Add(CharacteristicValue key, TermaValue value)
        {
            if (!value.Terma.Equals(Terma))
                throw new ArgumentException();
            if (!key.Characteristic.Equals(Terma.Parent))
                throw new ArgumentException();
            points.Add(key, value);
        }

        public void Add(double charasteristicValue, double termaValue)
        {
            points.Add(new CharacteristicValue(Terma.Parent, charasteristicValue), new TermaValue(Terma, termaValue));
        }

        public void Add(KeyValuePair<CharacteristicValue, TermaValue> item)
        {
            if (!item.Value.Terma.Equals(Terma))
                throw new ArgumentException();
            ((IDictionary<CharacteristicValue, TermaValue>)points).Add(item);
        }

        public void Clear()
        {
            points.Clear();
        }

        public bool Contains(KeyValuePair<CharacteristicValue, TermaValue> item)
        {
            return ((IDictionary<CharacteristicValue, TermaValue>)points).Contains(item);
        }

        public bool ContainsKey(CharacteristicValue key)
        {
            return points.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<CharacteristicValue, TermaValue>[] array, int arrayIndex)
        {
            ((IDictionary<CharacteristicValue, TermaValue>)points).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<CharacteristicValue, TermaValue>> GetEnumerator()
        {
            return points.GetEnumerator();
        }

        public bool Remove(CharacteristicValue key)
        {
            return points.Remove(key);
        }

        public bool Remove(KeyValuePair<CharacteristicValue, TermaValue> item)
        {
            return ((IDictionary<CharacteristicValue, TermaValue>)points).Remove(item);
        }

        public bool TryGetValue(CharacteristicValue key, out TermaValue value)
        {
            return points.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return points.GetEnumerator();
        }

        /// <summary>
        /// Срезает верхушку. Изменению подчиняются только уже добавленные данные.
        /// </summary>
        /// <param name="percent">Устанавливает максимальное допустимое значение.
        /// от 0 до 1.</param>
        void CutUp(double percent)
        {
            SelectedAreaOfTerma update = new SelectedAreaOfTerma(Terma);
            if (!this.IsSort())
                throw new Exception("Последовательость ключей не отсортирована.");
            foreach(CharacteristicValue key in keysList)
            {
                if(key.Value)
            }
            throw new NotImplementedException();
        }
    }
}