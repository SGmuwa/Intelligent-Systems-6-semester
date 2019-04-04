using System;
using System.Collections.Generic;

namespace Intelligent_Systems_6_semester_csharp
{
    public class RulesManager
    {
        protected List<Rule> Rules
            = new List<Rule>();
        internal RulesManager() { }

        public int Length => Rules.Count;

        public void Add(Terma termaIf, Terma termaThen)
        {
            Rule toAdd = new Rule(termaIf, termaThen);
            if (toAdd == null)
                throw new ArgumentNullException();
            if (Rules.Contains(toAdd))
                throw new ArgumentException();
            Rules.Add(toAdd);
        }
    }
}