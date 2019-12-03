using NeuralNetworkWithoutTraining;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class NeuralTest
    {
        [TestMethod]
        public void Test()
        {
            Neuron and = new Neuron(2, 1, 1);
            Assert.IsTrue(and.Calculate(1, 1));
            Assert.IsFalse(and.Calculate(0, 1));
            Assert.IsFalse(and.Calculate(1, 0));
            Assert.IsFalse(and.Calculate(0, 0));

            Neuron or = new Neuron(1, 1, 1);
            Assert.IsTrue(or.Calculate(1, 1));
            Assert.IsTrue(or.Calculate(0, 1));
            Assert.IsTrue(or.Calculate(1, 0));
            Assert.IsFalse(or.Calculate(0, 0));

            Neuron not = new Neuron(0, -1);
            Assert.IsFalse(not.Calculate(1));
            Assert.IsTrue(not.Calculate(0));
        }
    }
}