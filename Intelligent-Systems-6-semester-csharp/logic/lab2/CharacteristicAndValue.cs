namespace lab1.logic.lab2
{
    public class CharacteristicAndValue
    {
        public CharacteristicAndValue(Characteristic characteristic, double value = )
        {
            Characteristic = characteristic;
            Value = value;
        }

        public Characteristic Characteristic { get; }
        public double Value { set; get; }
    }
}