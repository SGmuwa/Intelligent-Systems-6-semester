using System;
using System.Collections.Generic;
using System.Text;

namespace lab1
{
    public class Rule
    {
        private readonly Terma termaIf;
        private readonly Terma termaThen;

        public Rule(Terma termaIf, Terma termaThen)
        {
            this.termaIf = termaIf;
            this.termaThen = termaThen;
        }

        public override string ToString()
        {
            return "if " + termaIf.Parent.Name + "." + termaIf.Name +
                " then " + termaThen.Parent.Name + "." + termaThen.Name;
        }

        public string RelationStr()
        {
            StringBuilder sb = new StringBuilder();
            double[,] data = Relation();

            sb.Append(termaThen.Parent.Name);
            sb.Append('.');
            sb.Append(termaThen.Name);
            sb.Append(" \\/ \n");

            sb.Append(termaIf.Parent.Name);
            sb.Append('.');
            sb.Append(termaIf.Name);
            sb.Append(" ==> \n");
            foreach(PercentAndCharacteristicvalue i in termaIf)
            {
                sb.Append(i.CharacteristicValue.ToString("G4"));
                sb.Append('\t');
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("\n");
            sb.Append('_', 10);
            sb.Append("\n");
            IEnumerator<PercentAndCharacteristicvalue> iThen = termaThen.GetEnumerator();
            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int x = 0; x < data.GetLength(0); x++)
                {
                    if (x != 0)
                        sb.Append('\t');
                    sb.Append(data[x, y].ToString("G4"));
                }
                sb.Append("\t| ");
                iThen.MoveNext();
                sb.Append(iThen.Current.CharacteristicValue.ToString("G4"));
                sb.Append("\n");
            }
            return sb.ToString();
        }

        public double[,] Relation()
        {
            double[,] output = new double[termaIf.Length, termaThen.Length];
            {
                int x = 0, y = 0;
                foreach (PercentAndCharacteristicvalue left in termaIf)
                {
                    foreach (PercentAndCharacteristicvalue right in termaThen)
                    {
                        output[y, x++]
                            = Math.Min(left.Percent, right.Percent);
                    }
                    y++;
                    x = 0;
                }
            }
            return output;
        }

        /// <summary>
        /// Возвращает then если if_ и TermaIf совпадают.
        /// </summary>
        /// <param name="if_">Причина.</param>
        /// <returns>Следствие, если есть. Иначе - пустота.</returns>
        public Terma SearchThen(Terma if_)
        {
            if (this.termaIf.Equals(if_))
                return this.termaThen;
            else
                return null;
        }
    }
}