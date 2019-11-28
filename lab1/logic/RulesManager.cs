using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        /// <param name="value">Значение характеристики.</param>
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
            IList<SelectedAreaOfTerma> areas = SelectInThen(terms);
            // Если термы принадлежат одной характеристики, то объединить их.
            areas = Union(areas);
            // Считаем интеграл для каждой характеристики.
            return GetCenters(areas);
        }

        private HashSet<CharacteristicValue> GetCenters(IList<SelectedAreaOfTerma> areas)
        {
            HashSet<CharacteristicValue> output = new HashSet<CharacteristicValue>();
            foreach(SelectedAreaOfTerma area in areas)
                output.Add(GetCenter(area));
            return output;
        }

        private CharacteristicValue GetCenter(SelectedAreaOfTerma area)
        {
            double mass = 0;
            double massMoment = 0;
            KeyValuePair<CharacteristicValue, TermaValue>? last = null;
            foreach(KeyValuePair<CharacteristicValue, TermaValue> pair in area)
            {
                if(last.HasValue)
                {
                    double currentMass = GetMassTrapeze(last.Value, pair);
                    massMoment += GetEquilibriumPointTrapeze(last.Value, pair) * currentMass;
                    mass += currentMass;
                }
                last = pair;
            }
            if(!last.HasValue)
                return null;
            return new CharacteristicValue(last.Value.Key.Characteristic, massMoment / mass);
        }

        private double GetEquilibriumPointTrapeze(KeyValuePair<CharacteristicValue, TermaValue> left, KeyValuePair<CharacteristicValue, TermaValue> right)
        {
            double first = GetEquilibriumPointTriangle(left.Key.Value, right.Key.Value);
            double second = GetEquilibriumPointRectangle(left.Key.Value, right.Key.Value);
            return first + second;
        }

        private double GetEquilibriumPointTriangle(double x0, double x1)
        {
            /*
            integral(min...t, k*y) = integral(t...max, k*y)

            minus(min...t, k*y^2/2) = minus(t...max, k*y^2/2)

            k*t^2/2 - k*min^2/2 = k*max^2/2 - k*t^2/2
            x = t^2
            k*x/2 - k*min^2/2 = k*max^2/2 - k*x/2
            k*x/2 + k*x/2 - k*min^2/2 = k*max^2/2
            k*x/2 + k*x/2 = k*max^2/2 + k*min^2/2
            x * (k/2 + k/2) = k*(max^2 + min^2)/2
            x * k = k*(max^2 + min^2)/2
            x = k/k*(max^2 + min^2)/2
            x = (max^2 + min^2)/2
            t = sqrt((max^2 + min^2)/2)
            */
            return Math.Sqrt((x1*x1 + x0*x0)/2);
        }

        private double GetEquilibriumPointRectangle(double x0, double x1)
            => (x0 + x1) / 2;

        private double GetMassTrapeze(KeyValuePair<CharacteristicValue, TermaValue> left, KeyValuePair<CharacteristicValue, TermaValue> right)
        {
            /*
               /|
              / |
             /  |
            /   | b
            |   |
          a |   |
            |___|
              h
            */
            double a = left.Value.Percent;
            double b = right.Value.Percent;
            double h = right.Key.Value - left.Key.Value;
            return h * (a + b) / 2;
        }

        private IList<SelectedAreaOfTerma> Union(IList<SelectedAreaOfTerma> areas)
        {
            return areas;
            
            foreach(var areaLeft in areas)
            {

                foreach(var pair in areaLeft)
                {
                    // TODO pair
                }
            }
        }

        private (double, double) GetIntersectionPoint(double xa, double ya, double xb, double yb, double xc, double yc, double xd, double yd)
        {
            // http://www.cyberforum.ru/c-beginners/thread665859.html
            double t2 = ((xc-xa)*(yb-ya)-(xb-xa)*(yc-ya)) / ((xb-xa)*(yd-yc)-(xd-xc)*(yb-ya));
            double x = xc+(xd-xc)*t2;
            double y = yc+(yd-yc)*t2;
            return (x, y);
        }

        private string DebugAreas(IList<SelectedAreaOfTerma> areas)
        {
            StringBuilder debug = new StringBuilder();
            int p = -1;
            foreach(var area in areas)
            {
                p++;
                foreach(var point in area)
                    debug.Append($"({point.Key.Value};{point.Value.Percent}) ");
                debug.Append("\n\n");
            }
            return debug.ToString();
        }

        /// <summary>
        /// Выделение областей в then (следствия).
        /// </summary>
        /// <param name="terms">Значения терм из if (причины)</param>
        /// <returns>Области.</returns>
        private List<SelectedAreaOfTerma> SelectInThen(ICollection<TermaValue> terms)
        {
            List<SelectedAreaOfTerma> areas = new List<SelectedAreaOfTerma>();
            foreach(TermaValue tv in terms)
            {
                ISet<Terma> thens = SearchThen(tv.Terma);
                foreach (Terma then in thens)
                {
                    SelectedAreaOfTerma area = new SelectedAreaOfTerma(then);
                    area.AddRange(then);
                    area = area.CutUp(tv.Percent);
                    areas.Add(area);
                }
            }
            return areas;
        }

        public ISet<Terma> SearchThen(Terma if_)
        {
            HashSet<Terma> output = new HashSet<Terma>();
            foreach(Rule r in this)
                output.Add(r.SearchThen(if_));
            output.Remove(null);
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