using System;
using lab1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            /*
	        Псевдокод.
	        Инициализация.
	        Вес яблока {
	        	МАЛЫЙ		1/50 +	 0.5/150 +	0/250 +		0/350
	        	СРЕДНИЙ		0/50 +	 1/150 +	0.5/250 +	0/350
	        	БОЛЬШОЙ		0/50 +	 0/150 +	0.5/250 +	1/350
	        }
	        Потребительская стоимость яблока {
	        	НИЗКАЯ	1.0/10 +	0.8/20 +	0.1/30 +	0.0/40
	        	СРЕДНЯЯ	0.1/10 +	1.0/20 +	0.5/30 +	0.0/40
	        	ВЫСОКАЯ	0.0/10 +	0.4/20 +	0.7/30 +	1.0/40
	        }
	        Правила {
	        	"Вес яблока"."МАЛЫЙ" => "Потребительская стоимость яблока"."НИЗКАЯ"
	        	"Вес яблока"."СРЕДНИЙ" => "Потребительская стоимость яблока"."ВЫСОКАЯ"
	        	"Вес яблока"."БОЛЬШОЙ" => "Потребительская стоимость яблока"."СРЕДНИЙ"
	        }
	        Сделать ввод конкретных данных {
	        	Множество яблок 1 = ....
	        	...
	        }
	        Надо постротить таблицу. R1 =
	        N1 ->
	        1	0.8	0.1	0		M1
	        0.5	0.5	0.1	0		||
	        0	0	0	0		\/
	        0	0	0	0
	        Программа должна реализовать генерацию данной таблицы.
	        Программа должна уметь строить НЕ к терме.
	        Программа должна уметь добавлять терму к существующим.
	        */

            IndefiniteSets state = new IndefiniteSets();
            Characteristic weight =
                new Characteristic("Вес яблока", 50, 350);
            state.Characteristics.Add(weight);
            weight.Add(
                "Малый", 1, 50,
                0.5, 150,
                0, 250,
                0, 350);
            weight.Add(
                "Средний", 0, 50,
                1, 150,
                0.5, 250,
                0, 350);
            weight.Add(
                "Большой", 0, 50,
                0, 150,
                0.5, 250,
                1, 350);

            Characteristic price =
                new Characteristic("Потребительская стоимость яблока", 10, 40);
            state.Characteristics.Add(price);
            price.Add(
                "Низкая", 1, 10,
                0.8, 20,
                0.1, 30,
                0, 40);
            price.Add(
                "Средняя", 0.1, 10,
                1, 20,
                0.5, 30,
                0, 40);
            price.Add(
                "Высокая", 0, 10,
                0.4, 20,
                0.7, 30,
                0.1, 40);


            state.Rules.Add(weight["Малый"], price["Низкая"]);
            state.Rules.Add(weight["Средний"], price["Высокая"]);
            state.Rules.Add(weight["Большой"], price["Средняя"]);

            Assert.AreEqual(2, state.Characteristics.Length);
            Assert.AreEqual(3, state.Rules.Length);
            try { price.Add("?", 1, 0); Assert.Fail("Удалось вне диапазона."); } catch(Exception e) { if (e is AssertFailedException) throw e; }
            try { price.Add("?", 2, 20); Assert.Fail("Удалось вне диапазона проценты."); } catch (Exception e) { if (e is AssertFailedException) throw e; }
            try
            {
                price.Add(
                    "Высокая", 0, 10,
                    0.4, 25,
                    0.7, 30,
                    0.1, 40);
                Assert.Fail("Получилось добавить одинаковую по имени.");
            }
            catch (Exception e) { if (e is AssertFailedException) throw e; }


            double[,] RN1M1assert = new double[,] {
                { 1, 0.8, 0.1, 0 },
                { 0.5, 0.5, 0.1, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 } };
            double[,] RN1M1 = state.Relation(weight["Малый"], price["Низкая"]);
            CollectionAssert.AreEqual(RN1M1assert, RN1M1);

            state.Characteristics["Потребительская стоимость яблока"].Add(LogicalFunctions.NOT, "Низкая", "Очень высокая");

            Console.WriteLine(state);
        }
    }
}
