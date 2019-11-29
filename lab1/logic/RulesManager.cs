using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
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
        { // По хорошему надо бы сделать, чтобы termaIf была типа IEnumerable
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
            IEnumerable<SelectedAreaOfTerma> areas = SelectInThen(terms);
            // Если термы принадлежат одной характеристики, то объединить их.
            areas = Union(areas);
            // Объединяем до одной линии.
            areas = Merge(areas);
            // Считаем интеграл для каждой характеристики.
            return GetCenters(areas);
        }

        private HashSet<CharacteristicValue> GetCenters(IEnumerable<SelectedAreaOfTerma> areas)
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
                    double currentEquilibriumPoint = GetEquilibriumPointTrapeze(last.Value, pair);
                    massMoment += currentEquilibriumPoint * currentMass;
                    mass += currentMass;
                }
                last = pair;
            }
            if(!last.HasValue)
                return null;
            return new CharacteristicValue(area.Characteristic, massMoment / mass);
        }

        private double GetEquilibriumPointTrapeze(KeyValuePair<CharacteristicValue, TermaValue> left, KeyValuePair<CharacteristicValue, TermaValue> right)
        {
            /*
Треугольник
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
Трапеция
integral(min...t, k*y + h) = integral(t...max, k*y + h)
minus(min...t, k*y^2/2 + h*y) = minus(t...max, k*y^2/2 + h*y)

k*t^2/2 + h*t - k*min^2/2 - h*min = k*max^2/2 + h*max - k*t^2/2 - h*t
k*t^2/2 + 2*h*t - k*min^2/2 - h*min - k*max^2/2 - h*max + k*t^2/2 = 0
k*t^2 + 2*h*t - k*min^2/2 - h*min - k*max^2/2 - h*max = 0
k*t^2 + 2*h*t - k*(min^2+max^2)/2 - h*(min + max) = 0
a = k
b = 2*h
c = -k*(min^2+max^2)/2 - h*(min + max)
t = (-b + sqrt(b^2 - 4*a*c)) / (2*a)
t = (-(2*h) + sqrt((2*h)^2 - 4*k*(-k*(min^2+max^2)/2 - h*(min + max)))) / (2*k)
t = (-(2*h) + sqrt(4*h^2 - 4*k*(-k*(min^2+max^2)/2 - h*(min + max)))) / (2*k)
t = (-(2*h) + 2*sqrt(h^2 - k*(-k*(min^2+max^2)/2 - h*(min + max)))) / (2*k)
t = (-(h) + sqrt(h^2 - k*(-k*(min^2+max^2)/2 - h*(min + max)))) / (k)
t = (sqrt(h^2 - k*(-k*(min^2+max^2)/2 - h*(min + max))) - h) / k
t = (sqrt(h^2 + k*(k*(min^2+max^2)/2 + h*(min + max))) - h) / k
h = 0 (Проверка)
t = (sqrt(0^2 + k*(k*(min^2+max^2)/2 + 0*(min + max))) - 0) / k
t = sqrt(k*(k*(min^2+max^2)/2)) / k
t = sqrt(((min^2+max^2)/2))
t = sqrt((min^2+max^2)/2)
Проверка два
k*t^2 + 2*h*t - k*(min^2+max^2)/2 - h*(min + max) = 0
k = 0
0*t^2 + 2*h*t - 0*(min^2+max^2)/2 - h*(min + max) = 0
2*h*t - h*(min + max) = 0
t = (min + max)/2

            */
            double min = left.Key.Value;
            double max = right.Key.Value;
            double h = GetH();
            double k = GetK();
            if(k == 0) // Предотвращение деления на ноль.
                return (min + max) / 2;
            if(h == 0) // Оптимизация
                return Math.Sqrt((min*min + max*max)/2);
            return (Math.Sqrt(h*h + k*(k*(min*min + max*max)/2 + h*(min + max))) - h) / k;

            double GetH()
            {
                double h0 = left.Value.Percent;
                double h1 = right.Value.Percent;
                return h0 < h1 ? h0 : h1;
            }

            double GetK()
            {
                double h0 = left.Value.Percent - h;
                double h1 = right.Value.Percent - h;
                return h0 > h1 ? h0 / (max - min) : h1 / (max - min);
            }
        }

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

        private List<SelectedAreaOfTerma> Union(IEnumerable<SelectedAreaOfTerma> areas)
        {
            var output = new List<SelectedAreaOfTerma>();
            
            foreach(var areaLeft in areas)
            {
                foreach(var areaRight in areas)
                {
                    if(areaLeft == areaRight)
                        continue;
                    if(areaLeft.Characteristic == areaRight.Characteristic)
                    {
                        SelectedAreaOfTerma editedArea = new SelectedAreaOfTerma(areaLeft.Values.First().Terma);
                        bool? PreviousLeftBiggerRight = null;
                        KeyValuePair<CharacteristicValue, TermaValue>? previousLeft = null;
                        foreach(KeyValuePair<CharacteristicValue, TermaValue> leftPoint in areaLeft)
                        {
                            double valueLeft = leftPoint.Value.Percent;
                            double valueRight = areaRight.GetPercentAt(leftPoint.Key.Value);
                            if(valueLeft >= valueRight)
                            { // Левый хороший, надо его оставить.
                                if(PreviousLeftBiggerRight.HasValue && !PreviousLeftBiggerRight.Value)
                                { // Хотя раньше он был плохим
                                    List<KeyValuePair<CharacteristicValue, TermaValue>> rightPoints = areaRight.SubAndOnePointBeyondEachBorder(previousLeft.Value.Key, leftPoint.Key);
                                    KeyValuePair<CharacteristicValue, TermaValue>? previousRightPoint = null;
                                    foreach(var rightPoint in rightPoints)
                                    {
                                        if(previousRightPoint.HasValue)
                                        {
                                            var crossingPoint = GetIntersectionPoint(previousLeft.Value, leftPoint, previousRightPoint.Value, rightPoint);
                                            if(crossingPoint.HasValue)
                                                editedArea.Add(crossingPoint.Value);
                                        }
                                        previousRightPoint = rightPoint;
                                    }
                                }
                                editedArea.Add(leftPoint.Key, leftPoint.Value);
                                PreviousLeftBiggerRight = true;
                            }
                            else
                            { // Левый плохой.
                                if(PreviousLeftBiggerRight.HasValue && PreviousLeftBiggerRight.Value)
                                { // Хотя раньше он был хорошим
                                    // Найдём, когда он стал плохим.
                                    List<KeyValuePair<CharacteristicValue, TermaValue>> rightPoints = areaRight.SubAndOnePointBeyondEachBorder(previousLeft.Value.Key, leftPoint.Key);
                                    KeyValuePair<CharacteristicValue, TermaValue>? previousRightPoint = null;
                                    foreach(var rightPoint in rightPoints)
                                    {
                                        if(previousRightPoint.HasValue)
                                        {
                                            var crossingPoint = GetIntersectionPoint(previousLeft.Value, leftPoint, previousRightPoint.Value, rightPoint);
                                            if(crossingPoint.HasValue)
                                                editedArea.Add(crossingPoint.Value);
                                        }
                                        previousRightPoint = rightPoint;
                                    }
                                }
                                PreviousLeftBiggerRight = false;
                            }
                            previousLeft = leftPoint;
                        }
                        output.Add(editedArea);
                    }
                    else
                    {
                        output.Add(areaLeft);
                    }
                }
            }
            return output;
        }

        
        /// <summary>
        /// Пытается объединить выделения до одной полосы.
        /// </summary>
        /// <param name="areas">Выделения, которые надо объединить.</param>
        /// <returns>Объединённые выделения.</returns>
        private ICollection<SelectedAreaOfTerma> Merge(IEnumerable<SelectedAreaOfTerma> areas)
        {
            Dictionary<Characteristic, SelectedAreaOfTerma> outputDictionary = new Dictionary<Characteristic, SelectedAreaOfTerma>();
            foreach(SelectedAreaOfTerma area in areas)
            {
                if(!outputDictionary.ContainsKey(area.Characteristic))
                    outputDictionary[area.Characteristic] = area;
                else
                {
                    var toMerge = outputDictionary[area.Characteristic];
                    SelectedAreaOfTerma mergeResult = new SelectedAreaOfTerma(new Terma("unknown", area.Characteristic));
                    foreach(var point in toMerge)
                        mergeResult.Add(point.Key.Value, point.Value.Percent);
                    foreach(var point in area)
                        if(!mergeResult.ContainsKey(point.Key))
                            mergeResult.Add(point.Key.Value, point.Value.Percent);
                    outputDictionary[area.Characteristic] = mergeResult;
                }
            }
            return outputDictionary.Values;
        }

        /// <summary>
        /// Ищет пересечение между прямыми A --- B и C --- D.
        /// </summary>
        private KeyValuePair<CharacteristicValue, TermaValue>? GetIntersectionPoint(
            KeyValuePair<CharacteristicValue, TermaValue> A,
            KeyValuePair<CharacteristicValue, TermaValue> B,
            KeyValuePair<CharacteristicValue, TermaValue> C,
            KeyValuePair<CharacteristicValue, TermaValue> D)
        {
            (double, double) answer = GetIntersectionPoint(
                A.Key.Value, A.Value.Percent,
                B.Key.Value, B.Value.Percent,
                C.Key.Value, C.Value.Percent,
                D.Key.Value, D.Value.Percent
            );
            return double.IsFinite(answer.Item1) && double.IsFinite(answer.Item2) ? new KeyValuePair<CharacteristicValue, TermaValue>(
                new CharacteristicValue(A.Key.Characteristic, answer.Item1),
                new TermaValue(A.Value.Terma, answer.Item2)
            ) : new KeyValuePair<CharacteristicValue, TermaValue>?();
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