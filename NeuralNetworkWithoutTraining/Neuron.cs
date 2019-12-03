using System;
using System.Collections.Generic;

namespace NeuralNetworkWithoutTraining
{
    public class Neuron
    {
        private readonly IReadOnlyCollection<double> Weights;
        private readonly double Sensitivity;

        public Neuron(double sensitivity, params double[] weights) : this(sensitivity, (IReadOnlyCollection<double>)weights){}

        public Neuron(double sensitivity, IReadOnlyCollection<double> weights)
        {
            Weights = weights;
            Sensitivity = sensitivity;
        }

        public bool Calculate(params double[] input) => Calculate((IReadOnlyCollection<double>)input);

        public bool Calculate(IReadOnlyCollection<double> input)
        {
            if(input.Count != Weights.Count)
                throw new ArgumentOutOfRangeException();
            IEnumerator<double> w = Weights.GetEnumerator();
            IEnumerator<double> i = input.GetEnumerator();
            double output = 0;
            while(w.MoveNext() && i.MoveNext())
                output += w.Current * i.Current;
            Console.WriteLine(output);
            return output >= Sensitivity;
        }
    }
}