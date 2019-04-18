using System;

namespace lab1.logic.logic.lab2
{
    public class TermaValue
    {
        private double _value;

        public TermaValue(Terma Terma, double Percent = 0.0)
        {
            this.Terma = Terma;
            this.Percent = Percent;
        }
        public Terma Terma { get; }
        public double Percent
        {
            get => _value;
            set
            {
                if (0.0 <= value && value <= 1.0)
                    _value = value;
                else
                    throw new ArgumentException();
            }
        }
    }
}