using System;
using System.Collections;
using System.Collections.Generic;

namespace lab1.logic.lab2
{
    /// <summary>
    /// Класс представляет выделенную
    /// територию термы.
    /// </summary>
    public class SelectedAreaOfTerma : IDictionary<CharacteristicValue, TermaValue>
    {
        private readonly IDictionary<CharacteristicValue, TermaValue> points = new Dictionary<CharacteristicValue, TermaValue>();

        public Terma Terma { get; }

        public SelectedAreaOfTerma(Terma terma)
            => this.Terma = terma;

        public TermaValue this[CharacteristicValue key] { get => points[key]; set => points[key] = value; }

        public ICollection<CharacteristicValue> Keys => points.Keys;

        public ICollection<TermaValue> Values => points.Values;

        public int Count => points.Count;

        public bool IsReadOnly => points.IsReadOnly;

        public void Add(CharacteristicValue key, TermaValue value)
        {
            if (!value.Terma.Equals(Terma))
                throw new ArgumentException();
            points.Add(key, value);
        }

        public void Add(KeyValuePair<CharacteristicValue, TermaValue> item)
        {
            if (!item.Value.Terma.Equals(Terma))
                throw new ArgumentException();
            points.Add(item);
        }

        public void Clear()
        {
            points.Clear();
        }

        public bool Contains(KeyValuePair<CharacteristicValue, TermaValue> item)
        {
            return points.Contains(item);
        }

        public bool ContainsKey(CharacteristicValue key)
        {
            return points.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<CharacteristicValue, TermaValue>[] array, int arrayIndex)
        {
            points.CopyTo(array, arrayIndex);
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
            return points.Remove(item);
        }

        public bool TryGetValue(CharacteristicValue key, out TermaValue value)
        {
            return points.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return points.GetEnumerator();
        }
    }
}