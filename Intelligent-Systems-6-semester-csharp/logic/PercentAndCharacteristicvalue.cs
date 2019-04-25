using System;

namespace lab1
{
    public class PercentAndCharacteristicvalue : IComparable, IComparable<PercentAndCharacteristicvalue>
    {
        public PercentAndCharacteristicvalue(double percent, double characteristicValue)
        {
            if (double.IsNaN(percent)
                || double.IsInfinity(percent)
                || percent < 0.0
                || percent > 1.0
                || double.IsNaN(characteristicValue)
                || double.IsInfinity(characteristicValue))
                throw new ArgumentException();
            Percent = percent;
            CharacteristicValue = characteristicValue;
        }

        public double Percent { get; }
        public double CharacteristicValue { get; }

        public override string ToString()
        {
            return ("{\"Percent\":\"" +
                Percent.ToString().Replace(',', '.') +
                "\", \"CharacteristicValue\":\"" +
                CharacteristicValue.ToString().Replace(',', '.') +
                "\" }");
        }

        public int CompareTo(object obj)
        {
            return CharacteristicValue.CompareTo(obj);
        }

        public int CompareTo(PercentAndCharacteristicvalue other)
        {
            return CharacteristicValue.CompareTo(other.CharacteristicValue);
        }

        public static bool operator >(PercentAndCharacteristicvalue left, PercentAndCharacteristicvalue right)
            => left.CharacteristicValue > right.CharacteristicValue;

        public static bool operator <(PercentAndCharacteristicvalue left, PercentAndCharacteristicvalue right)
            => left.CharacteristicValue < right.CharacteristicValue;
    }
}