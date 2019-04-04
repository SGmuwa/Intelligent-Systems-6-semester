using System;
using System.Collections.Generic;

namespace Intelligent_Systems_6_semester_csharp
{
    public class CharacteristicsMaganger
    {
        protected List<Characteristic> Characteristics
            = new List<Characteristic>();
        internal CharacteristicsMaganger() { }

        public int Length => Characteristics.Count;

        public void Add(Characteristic toAdd)
        {
            if (toAdd == null)
                throw new ArgumentNullException();
            if (Characteristics.Contains(toAdd))
                throw new ArgumentException();
            Characteristics.Add(toAdd);
        }

    }
}