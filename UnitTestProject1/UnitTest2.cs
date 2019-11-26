using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using lab1;
using lab1.logic.lab2;

namespace UnitTestProject1
{
    /// <summary>
    /// Test lab2.
    /// </summary>
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestGetPercentAt()
        {
            IndefiniteSets state = new IndefiniteSets();
            state.Characteristics.Add(new Characteristic("Test", 0, 1));
            state.Characteristics["Test"].Add("Testing", 0, 0, 1, 1);
            Assert.AreEqual(0.3, state.Characteristics["Test"]["Testing"].GetPercentAt(0.3), 0.0001);
        }

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
                new Characteristic("y", 0, 40);
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

            ICollection<CharacteristicValue> outputCollection =
                state.Rules.Call(MethodType.Aggregation, x, 1.3);
            Assert.IsNotNull(outputCollection);
            CharacteristicValue[] expect = new CharacteristicValue[]
            { new CharacteristicValue(y, 163 / 12.1) };
            Assert.AreEqual(expect.Length, outputCollection.Count);
            CharacteristicValue output = outputCollection.GetFirst();
            Assert.AreEqual(expect[0].Characteristic, output.Characteristic);
            Assert.AreEqual(expect[0].Value, output.Value, 0.01);

            Console.WriteLine(state);
        }

        
    }
    static class GetFirstInCollection
    {
        public static T GetFirst<T>(this IEnumerable<T> list)
        {
            IEnumerator<T> en = list.GetEnumerator();
            if (!en.MoveNext())
                throw new ArgumentException();
            return en.Current;
        }
    }
}
