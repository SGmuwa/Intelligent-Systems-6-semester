using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    /// <summary>
    /// Test lab2.
    /// </summary>
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            IndefiniteSets state = new IndefiniteSets();
            Characteristic x =
                new Characteristic("x", 0, 4);
            state.Characteristics.Add(x);
            x.Add(
                "Малый", 0, 0,
                1, 1,
                0, 2);
            x.Add(
                "Средний", 0, 1,
                1, 2,
                0, 3);
            x.Add(
                "Большой", 0, 2,
                1, 3,
                0, 4);

            Characteristic y =
                new Characteristic("y", 10, 40);
            state.Characteristics.Add(y);
            y.Add(
                "Малый", 0, 0,
                1, 10,
                0, 20);
            y.Add(
                "Средний", 0, 10,
                1, 20,
                0, 30);
            y.Add(
                "Большой", 0, 20,
                1, 30,
                0, 40);


            state.Rules.Add(x["Малый"], y["Малый"]);
            state.Rules.Add(x["Средний"], y["Средний"]);
            state.Rules.Add(x["Большой"], y["Большой"]);

            
            
            Console.WriteLine(state);
        }
    }
}
