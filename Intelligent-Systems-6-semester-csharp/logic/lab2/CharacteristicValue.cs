namespace lab1.logic.lab2
{
    /// <summary>
    /// Представляет собой значение характеристики.
    /// </summary>
    public class CharacteristicValue
    {
        public CharacteristicValue(Characteristic characteristic, double value = )
        {
            Characteristic = characteristic;
            Value = value;
        }

        public Characteristic Characteristic { get; }
        public double Value { set; get; }
    }
}