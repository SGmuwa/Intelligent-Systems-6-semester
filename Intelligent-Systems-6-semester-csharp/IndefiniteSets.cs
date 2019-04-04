using System;
using System.Collections.Generic;

namespace Intelligent_Systems_6_semester_csharp
{
    public class IndefiniteSets
    {
        public CharacteristicsMaganger Characteristics { get; }
            = new CharacteristicsMaganger();
        public RulesManager Rules { get; }
            = new RulesManager();

        public double[,] Relation(Terma lefts, Terma rights)
        {
            double[,] output = new double[lefts.Length, rights.Length];
            {
                int x = 0, y = 0;
                foreach (PercentAndCharacteristicvalue left in lefts)
                {
                    foreach (PercentAndCharacteristicvalue right in rights)
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

        public override string ToString()
        {
            return "{ \"Characteristics\":" +
                Characteristics +
                "\", \"Rules\":\"" +
                Rules +
                "\" }";
        }
    }
}
