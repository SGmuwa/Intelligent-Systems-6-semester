namespace lab1
{
    public class Rule
    {
        private Terma termaIf;
        private Terma termaThen;

        public Rule(Terma termaIf, Terma termaThen)
        {
            this.termaIf = termaIf;
            this.termaThen = termaThen;
        }
    }
}