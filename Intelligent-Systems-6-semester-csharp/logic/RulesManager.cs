using System;
using System.Collections;
using System.Collections.Generic;
using lab1.logic.lab2;

namespace lab1
{
    public class RulesManager : IEnumerable<Rule>
    {
        protected List<Rule> Rules
            = new List<Rule>();
        internal RulesManager() { }

        public int Length => Rules.Count;

        public Rule this[int index] => Rules[index];

        public void Add(Terma termaIf, Terma termaThen)
        { // По хорошему надо бы сделать, чтобы termaIf была типа IEnumireable
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

        /// <summary>
        /// Вызов нечёткого управления.
        /// </summary>
        /// <param name="typeMethod">Тип управления.</param>
        /// <param name="charact">Характеристика, которой мы управляем.</param>
        /// <param name="value">Значение характериситки.</param>
        /// <returns>Значения then характеристик из заданных правил.</returns>
        public ICollection<CharacteristicValue> Call(MethodType typeMethod, Characteristic charact, double value)
            => Call(typeMethod, new CharacteristicValue(charact, value));

        /// <summary>
        /// Вызов нечёткого управления.
        /// </summary>
        /// <param name="typeMethod">Тип управления.</param>
        /// <param name="charactValue">Характеристика, которой мы управляем. Её значение.</param>
        /// <returns>Значения then характеристик из заданных правил.</returns>
        public ICollection<CharacteristicValue> Call(MethodType typeMethod, CharacteristicValue charactValue)
        {
            switch(typeMethod)
            {
                case MethodType.Aggregation:
                    return Aggregation(charactValue);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Вызов агрегирование по текущим правилам.
        /// </summary>
        /// <param name="charactValue">Характеристика, которой мы управляем. Её значение.</param>
        /// <returns>Значения then характеристик из заданных правил.</returns>
        public ICollection<CharacteristicValue> Aggregation(CharacteristicValue charactValue)
        {
            // Поиск значения терм характеристики в входной точке.
            ICollection<TermaValue> terms = charactValue.TermsValuesAt();
            // Находим выделение внутри then по каждой терме.
            ICollection<SelectedAreaOfTerma> areas = SelectInThen(terms);
            // Если термы принадлежат одной характеристики, то объединить их.
            areas = Union(terms);
            // Считаем интеграл для каждой характеристики.
            return Integrals(areas);
        }

        /// <summary>
        /// Выделение областей в then (следствия).
        /// </summary>
        /// <param name="terms">Значения терм из if (причины)</param>
        /// <returns>Области.</returns>
        private ICollection<SelectedAreaOfTerma> SelectInThen(ICollection<TermaValue> terms)
        {
            List<SelectedAreaOfTerma> areas = new List<SelectedAreaOfTerma>();
            foreach(TermaValue tv in terms)
            {
                ISet<Terma> thens = SearchThen(tv.Terma);
                foreach (Terma then in thens)
                {
                    SelectedAreaOfTerma area = new SelectedAreaOfTerma(then);
                    area.AddRange(then);
                    area.CutUp(tv.Percent);
                }
            }
            throw new NotImplementedException();
            return areas;
        }

        public ISet<Terma> SearchThen(Terma if_)
        {
            HashSet<Terma> output = new HashSet<Terma>();
            foreach(Rule r in this)
                output.Add(r.SearchThen(if_));
            return output;
        }

        public IEnumerator<Rule> GetEnumerator()
        {
            return Rules.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rules.GetEnumerator();
        }
    }
}