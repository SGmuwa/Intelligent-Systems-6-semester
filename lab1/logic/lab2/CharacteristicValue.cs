
using System;
using System.Collections.Generic;

namespace lab1.logic.lab2
{
    /// <summary>
    /// Представляет собой значение характеристики.
    /// </summary>
    public class CharacteristicValue : IComparable, IComparable<CharacteristicValue>, IComparable<double>
    {
        private double _value;

        public CharacteristicValue(Characteristic characteristic, double value = double.NaN)
        {
            Characteristic = characteristic;
            if (double.IsNaN(value))
                value = characteristic.Min;
            Value = value;
        }

        /// <summary>
        /// Получение характеристики.
        /// </summary>
        public Characteristic Characteristic { get; }
        /// <summary>
        /// Получение значение характеристики.
        /// </summary>
        public double Value { get => _value;
            set
            {
                if (Characteristic.Min <= value && value <= Characteristic.Max)
                    _value = value;
                else
                    throw new System.ArgumentOutOfRangeException
                        ($"value вне диапазона характеристики: " +
                        $"{Characteristic.Min} ... {value} ... {Characteristic.Max}");
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is double || obj is float)
                return CompareTo((double)obj);
            if (obj is CharacteristicValue value)
                return CompareTo(value);
            throw new ArgumentException();
        }

        public int CompareTo(CharacteristicValue other)
        {
            return CompareTo(other.Value);
        }

        public int CompareTo(double other)
        {
            return Value.CompareTo(other);
        }

        public override bool Equals(object obj)
        {
            return obj is CharacteristicValue other &&
                   Characteristic.Equals(other.Characteristic) &&
                   Value == other.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        /// <summary>
        /// Получить значения терм в заданной <see cref="Value"/> точке.
        /// </summary>
        /// <returns>Перечень значений терм.</returns>
        public ICollection<TermaValue> TermsValuesAt()
        {
            return Characteristic.TermsValuesAt(Value);
        }

        
    }
}