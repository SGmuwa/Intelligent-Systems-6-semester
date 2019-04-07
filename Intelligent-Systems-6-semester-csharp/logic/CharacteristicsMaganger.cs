using System;
using System.Collections.Generic;

namespace lab1
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

        public Characteristic this[string name]
            => Characteristics.Find((Characteristic c) => name.Equals(c.Name));

        public override string ToString()
        {
            return "{ \"Characteristics\":" +
                Characteristics.ToString(", ") +
                " }";
        }
    }
}