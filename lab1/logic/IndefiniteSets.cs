namespace lab1
{
    public class IndefiniteSets
    {
        public CharacteristicsMaganger Characteristics { get; }
            = new CharacteristicsMaganger();
        public RulesManager Rules { get; }
            = new RulesManager();

        public override string ToString()
        {
            return "{ \"Characteristics\":\"" +
                Characteristics +
                "\", \"Rules\":\"" +
                Rules +
                "\" }";
        }
    }
}
