using System;
using System.Collections.Generic;
using lab1.logic.lab2;
using lab1.logic.logic.lab2;

namespace lab1
{
    public class RulesManager
    {
        protected List<Rule> Rules
            = new List<Rule>();
        internal RulesManager() { }

        public int Length => Rules.Count;

        public Rule this[int index] => Rules[index];

        public void Add(Terma termaIf, Terma termaThen)
        {
            Rule toAdd = new Rule(termaIf, termaThen);
            if (toAdd == null)
                throw new ArgumentNullException();
            if (Rules.Contains(toAdd))
                throw new ArgumentException();
            Rules.Add(toAdd);
        }

        public override string ToString()
        {
            return "{ \"Rules\":" +
                Rules.ToString(", ") +
                " }";
        }
        
        public ICollection<CharacteristicValue> Call(MethodType typeMethod, Characteristic charact, double v)
        {
            switch(typeMethod)
            {
                case MethodType.Aggregation:
                    return Aggregation(charact, v);
                default:
                    throw new NotImplementedException();
            }
        }

        public ICollection<CharacteristicValue> Aggregation(Characteristic charact, double v)
        {
            // Поиск значения терм характеристики в точке.
            ICollection<TermaValue> terms = charact.ValuesAt(v);
            // Находим выделение внутри then по каждой терме.
            ICollection<SelectedAreaOfTerma> areas = Select(terms);
            // Если термы принадлежат одной характеристики, то объединить их.
            areas = Union(terms);
            // Считаем интеграл для каждой характеристики.
            return Integrals(areas);
        }
    }
}