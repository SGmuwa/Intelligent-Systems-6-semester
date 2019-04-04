using System.Collections.Generic;

namespace Intelligent_Systems_6_semester_csharp
{
    /// <summary>
    /// Терма характеристики.
    /// </summary>
    public class Terma
    {
        protected List<PercentAndCharacteristicvalue> list;
        public Terma(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}