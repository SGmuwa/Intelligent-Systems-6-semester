
using System.Collections.Generic;

namespace lab1.logic.lab2
{
    /// <summary>
    /// Представляет собой значение характеристики.
    /// </summary>
    public class CharacteristicValue
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
        /// Получение характериситики.
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