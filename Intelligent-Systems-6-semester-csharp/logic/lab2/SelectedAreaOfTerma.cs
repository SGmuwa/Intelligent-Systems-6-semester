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
        /// Срезает верхушку. Результат возвращается.
        /// </summary>
        /// <param name="percent">Устанавливает максимальное допустимое значение.
        /// от 0 до 1.</param>
        /// 
        public SelectedAreaOfTerma CutUp(double percent)
        {
            SelectedAreaOfTerma update = new SelectedAreaOfTerma(Terma);
            if (!this.IsSort())
                throw new Exception("Последовательость ключей не отсортирована.");
            KeyValuePair<CharacteristicValue, TermaValue> last =
                new KeyValuePair<CharacteristicValue, TermaValue>(null, null);
            foreach(KeyValuePair<CharacteristicValue, TermaValue> pair in this)
            {
                if (pair.Value.Percent <= percent)
                {
                    update.Add(pair);
                    last = pair;
                }
                else
                {
                    if (last.Key == null)
                    {
                        last = new KeyValuePair<CharacteristicValue, TermaValue>(
                            new CharacteristicValue(
                                pair.Key.Characteristic,
                                pair.Key.Value),
                            pair.Value);
                        update.Add(last);
                    }
                    else
                    {
                        double keyValue = MyMath.SearchYifX0X1Y0YX(
                            last.Value.Percent,
                            pair.Value.Percent,
                            last.Key.Value,
                            pair.Key.Value,
                            percent);
                        update.Add(keyValue, percent);
                        last = new KeyValuePair<CharacteristicValue, TermaValue>(new CharacteristicValue(pair.Key.Characteristic, pair.Key.Value), new TermaValue(pair.Value.Terma, pair.Value.Percent));
                        update.Add(last);
                    }
                }
            }
            return update;
        }

        /// <summary>
        /// Добавление термы в выделение.
        /// </summary>
        /// <param name="needToSelect">Терма, которую надо выделить.</param>
        public void AddRange(Terma needToSelect)
        {
            foreach(var piece in needToSelect)
            {
                Add(piece.CharacteristicValue, piece.Percent);
            }
        }
    }
}