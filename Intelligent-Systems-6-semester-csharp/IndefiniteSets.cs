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

        public double[,] Relation(object p1, object p2)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
