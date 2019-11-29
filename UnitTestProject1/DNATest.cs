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
            CancellationTokenSource tokenSource = new CancellationTokenSource(100);
            Search((x) => x[0]*x[0], 1, tokenSource.Token, true, _ => Console.WriteLine(_));
        }
    }
}