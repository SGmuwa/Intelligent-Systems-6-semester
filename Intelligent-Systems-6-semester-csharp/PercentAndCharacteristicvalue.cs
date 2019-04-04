using System;

namespace Intelligent_Systems_6_semester_csharp
{
    public struct PercentAndCharacteristicvalue
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
    }
}