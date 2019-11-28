using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace lab1.logic.lab2
{
    /// <summary>
    /// Класс представляет выделенную
    /// территорию термы.
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
            if (this.Count < 1)
                throw new NotSupportedException("Требуется хотя бы одна точка.");
            KeyValuePair<CharacteristicValue, TermaValue> last =
                new KeyValuePair<CharacteristicValue, TermaValue>(null, null);
            KeyValuePair<CharacteristicValue, TermaValue>? previous = null;
            bool isPreviousTooBig = this.First().Value.Percent > percent;
            foreach (KeyValuePair<CharacteristicValue, TermaValue> pair in this)
            {
                if (pair.Value.Percent <= percent)
                {
                    if (isPreviousTooBig)
                    {
                        double keyValue = MyMath.Interpolation(
                            previous.Value.Value.Percent,
                            pair.Value.Percent,
                            previous.Value.Key.Value,
                            pair.Key.Value,
                            percent);
                        update.Add(keyValue, percent);
                    }
                    update.Add(pair);
                    last = pair;
                    isPreviousTooBig = false;
                }
                else
                {
                    if (last.Key == null)
                    {
                        last = new KeyValuePair<CharacteristicValue, TermaValue>(
                            new CharacteristicValue(
                                pair.Key.Characteristic,
                                pair.Key.Value),
                            new TermaValue(pair.Value.Terma, percent));
                        update.Add(last);
                    }
                    else
                    {
                        double keyValue = MyMath.Interpolation(
                            last.Value.Percent,
                            pair.Value.Percent,
                            last.Key.Value,
                            pair.Key.Value,
                            percent);
                        if (keyValue != last.Key.Value)
                        {
                            update.Add(keyValue, percent);
                            last = new KeyValuePair<CharacteristicValue, TermaValue>(new CharacteristicValue(pair.Key.Characteristic, keyValue), new TermaValue(pair.Value.Terma, percent));
                        }
                    }
                    isPreviousTooBig = true;
                }
                previous = pair;
            }
            if (isPreviousTooBig)
            {
                update.Add(previous.Value.Key.Value, percent);
            }
            return update;
        }

        /// <summary>
        /// Добавление термы в выделение.
        /// </summary>
        /// <param name="needToSelect">Терма, которую надо выделить.</param>
        public void AddRange(Terma needToSelect)
        {
            foreach (var piece in needToSelect)
            {
                Add(piece.CharacteristicValue, piece.Percent);
            }
        }

        /// <summary>
        /// Получить процент термы в заданной точке.
        /// </summary>
        /// <param name="v">Заданная точка, в которой нас интересует процент Термы.</param>
        /// <returns>Процент термы в заданной точке.</returns>
        public double GetPercentAt(double v)
        {
            if (Count == 0)
                throw new NotSupportedException();
            KeyValuePair<CharacteristicValue, TermaValue> Previous = GetPrevious(v);
            KeyValuePair<CharacteristicValue, TermaValue> Next = GetNext(v);
            if (Previous.Key.Value == Next.Key.Value)
                return Next.Value.Percent;
            return MyMath.Interpolation(Previous.Key.Value, Next.Key.Value, Previous.Value.Percent, Next.Value.Percent, v);
        }


        public KeyValuePair<CharacteristicValue, TermaValue> GetNext(double v)
        {
            KeyValuePair<CharacteristicValue, TermaValue>? output = null;
            foreach (KeyValuePair<CharacteristicValue, TermaValue> contain in this)
            {
                if (output == null)
                    output = contain;
                if (output.Value.Key.Value <= v)
                    output = contain;
                else // Работает только для сортированных данных.
                    return output.Value;
            }
            return output.Value;
        }

        /// <summary>
        /// Получает предыдущую составляющую термы по значению характеристики
        /// </summary>
        /// <param name="v">Значение характеристики.</param>
        /// <returns>Предыдущий составляющий термы. Если такого нет, то вернёт null.</returns>
        public KeyValuePair<CharacteristicValue, TermaValue> GetPrevious(double v)
        {
            KeyValuePair<CharacteristicValue, TermaValue>? output = null;
            List<KeyValuePair<CharacteristicValue, TermaValue>> reverseList
                = new List<KeyValuePair<CharacteristicValue, TermaValue>>(this);
            reverseList.Reverse();
            foreach (KeyValuePair<CharacteristicValue, TermaValue> contain in reverseList)
            {
                if (!output.HasValue)
                    output = contain;
                if (output.Value.Key.Value >= v)
                    output = contain;
                else // Работает только для сортированных данных.
                    return output.Value;
            }
            return output.Value;
        }

        /// <summary>
        /// Выбирает только часть от выделения между min и max, а также по одной точке за границами min и max.
        /// </summary>
        /// <param name="min">Минимальная граница выделения.</param>
        /// <param name="max">Максимальная граница выделения.</param>
        /// <returns>Новый промежуток выделения.</returns>
        public List<KeyValuePair<CharacteristicValue, TermaValue>> SubAndOnePointBeyondEachBorder(CharacteristicValue min, CharacteristicValue max)
        {
            SubAndOnePointBeyondEachBorder_State minNotAdded = SubAndOnePointBeyondEachBorder_State.minNotAdded, minAdded_maxNotAdded = SubAndOnePointBeyondEachBorder_State.minAdded_maxNotAdded, minmaxAdded = SubAndOnePointBeyondEachBorder_State.minmaxAdded;
            if (Count == 0)
                throw new NotSupportedException();
            if (min.Characteristic != max.Characteristic || max.Characteristic != this.First().Key.Characteristic)
                throw new ArgumentException();
            List<KeyValuePair<CharacteristicValue, TermaValue>> sortedPoints = new List<KeyValuePair<CharacteristicValue, TermaValue>>(points);
            sortedPoints.Sort((l, r) => l.Key.Value.CompareTo(r.Key.Value));
            KeyValuePair<CharacteristicValue, TermaValue>? previous = null;
            List<KeyValuePair<CharacteristicValue, TermaValue>> output = new List<KeyValuePair<CharacteristicValue, TermaValue>>(sortedPoints.Count);
            SubAndOnePointBeyondEachBorder_State state = minNotAdded;
            foreach (var point in sortedPoints)
            {
                if (min.Value <= point.Key.Value && state == minNotAdded)
                {
                    if (previous.HasValue)
                        output.Add(previous.Value);
                    else
                        output.Add(new KeyValuePair<CharacteristicValue, TermaValue>(new CharacteristicValue(point.Key.Characteristic, point.Key.Value - 1), point.Value));
                    state = minAdded_maxNotAdded;
                }
                if(min.Value <= point.Key.Value && point.Key.Value <= max.Value)
                {
                    output.Add(point);
                }
                if (point.Key.Value > max.Value && state == minAdded_maxNotAdded)
                {
                    output.Add(point);
                    state = minmaxAdded;
                }
                previous = point;
            }
            if (state == minAdded_maxNotAdded)
            {
                output.Add(new KeyValuePair<CharacteristicValue, TermaValue>(new CharacteristicValue(previous.Value.Key.Characteristic, previous.Value.Key.Value + 1), previous.Value.Value));
                state = minmaxAdded;
            }
            return output;
        }

        private enum SubAndOnePointBeyondEachBorder_State
        {
            minNotAdded,
            minAdded_maxNotAdded,
            minmaxAdded
        }
    }
}