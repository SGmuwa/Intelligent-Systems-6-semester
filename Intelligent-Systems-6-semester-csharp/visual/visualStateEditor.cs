using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.visual
{
    class VisualStateEditor
    {
        public VisualStateEditor()
        {
            Console.CancelKeyPress += Cancel;
        }

        public void Cancel(object sender, ConsoleCancelEventArgs args)
        {
            throw new Exception("[CTRL+BREAK] Good bye.");
        }

        private string line = null;
        private string[] lineSplit = null;
        private IndefiniteSets sets = null;

        public void Run()
        {
            ConsoleColor defaultColorF = Console.ForegroundColor;
            ConsoleColor defaultColorB = Console.BackgroundColor;
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(sets);
                Console.ForegroundColor = defaultColorF;
                string input = Console.ReadLine();
                if (input == null || input.Length < 1)
                {
                    Console.WriteLine("Комбинация не распознана.");
                    continue;
                }
                input = input.Trim();
                lineSplit = input.Split(' ');
                string first = lineSplit[0].ToLower();
                try
                {
                    switch (first)
                    {
                        case "create":
                            sets = new IndefiniteSets();
                            break;
                        case "addcharacteristic":
                            AddCharacteristic();
                            break;
                        case "addterm":
                            AddTerm();
                            break;
                        case "help":
                            Console.WriteLine(Properties.Resources.help);
                            break;
                        case "exit":
                            return;
                        default:
                            Console.WriteLine("Комбинация не распознана. Воспользуйтесь help.");
                            break;
                    }
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ForegroundColor = defaultColorF;
                }
            } while (true);
        }

        private void AddTerm()
        {
            double[] doubleParams = new double[lineSplit.Length - 3];
            for(int i = 3; i < lineSplit.Length; i++)
            {
                doubleParams[i - 3] = double.Parse(lineSplit[i]);
            }
            sets.Characteristics[lineSplit[1]].Add(lineSplit[2], doubleParams);
        }

        private void AddCharacteristic()
        {
            sets.Characteristics.Add(new Characteristic(
                lineSplit[1],
                float.Parse(lineSplit[2]),
                float.Parse(lineSplit[3])
                ));
        }

        ~VisualStateEditor()
        {
            Console.CancelKeyPress -= Cancel;
        }
    }
}
