using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using static DNA.DNASearch;

namespace DNA.Test
{
    [TestClass]
    public class DNATest
    {
        [TestMethod]
        public void SimpleDNATest()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource(1000);
            double answer = Search((x) => -(x[0]*x[0]), 1, tokenSource.Token, true, _ => Console.WriteLine(_))[0];
            Assert.AreEqual(0, answer, 0.0001);
        }
    }
}