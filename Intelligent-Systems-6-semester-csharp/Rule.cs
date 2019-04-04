namespace Intelligent_Systems_6_semester_csharp
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