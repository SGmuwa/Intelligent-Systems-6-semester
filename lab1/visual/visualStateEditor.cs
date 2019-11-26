using System;
using System.Text;

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
        
        private string[] lineSplit = null;
        private IndefiniteSets sets = null;

        public void Run()
        {
            ConsoleColor defaultColorF = Console.ForegroundColor;
            ConsoleColor defaultColorB = Console.BackgroundColor;
            do
            {
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
                        case "addrule":
                            AddRule();
                            break;
                        case "relation":
                            Relation();
                            break;
                        case "show":
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(Serial(sets));
                            Console.ForegroundColor = defaultColorF;
                            break;
                        case "addrulelogic":
                            AddRuleLogic();
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

        private void AddRuleLogic()
        {
            sets.Characteristics[lineSplit[2]].Add(
                (LogicalFunctions)Enum.Parse(typeof(LogicalFunctions), lineSplit[1]),
                lineSplit[3],
                lineSplit[4]);
        }

        private void Relation()
        {
            Console.WriteLine(
            sets.Rules[int.Parse(lineSplit[1])].RelationStr()
            );
        }

        private string Serial(object input)
        {
            if (input == null)
                return null;
            StringBuilder stringBuilder = new StringBuilder();
            int tab = 0;
            foreach(char a in input.ToString())
            {
                switch(a)
                {
                    case '[':
                    case '{':
                        stringBuilder.Append("\n");
                        stringBuilder.Append(new String('\t', tab));
                        tab++;
                        stringBuilder.Append(a + "\n");
                        stringBuilder.Append(new String('\t', tab));
                        break;
                    case ']':
                    case '}':
                        tab--;
                        stringBuilder.Append("\n");
                        stringBuilder.Append(new String('\t', tab));
                        stringBuilder.Append(a);
                        stringBuilder.Append("\n");
                        stringBuilder.Append(new String('\t', tab));
                        break;
                    case ',':
                        stringBuilder.Append(",\n");
                        stringBuilder.Append(new String('\t', tab));
                        break;
                    default:
                        stringBuilder.Append(a);
                        break;
                }
            }
            return stringBuilder.ToString();
        }

        private void AddRule()
        {
            sets.Rules.Add(sets.Characteristics[lineSplit[1]][lineSplit[2]],
                sets.Characteristics[lineSplit[3]][lineSplit[4]]);
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
