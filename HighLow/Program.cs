using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLow
{
    class Program
    {
        List<Cards> Deck = new List<Cards>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            try { Console.SetWindowSize(237, 63); }
            catch { Console.SetWindowSize(200, 50); }
            Program program = new Program();
            program.CreateDeck();
        }
        void CreateDeck()
        {
            CardsType typo;
            CardsName name;
            string[] symbol = new string[] { "♣ ", "♦", "♥", "♠", " " };
            ConsoleColor[] color = new ConsoleColor[] { ConsoleColor.Cyan, ConsoleColor.DarkRed, ConsoleColor.Magenta, ConsoleColor.Green, ConsoleColor.DarkYellow };
            int i = 0;

            foreach (string type in Enum.GetNames(typeof(CardsType)))
            {
                typo = (CardsType)Enum.Parse(typeof(CardsType), type);

                if (typo != CardsType.ERROR)
                {
                    foreach (string nemo in Enum.GetNames(typeof(CardsName)))
                    {
                        name = (CardsName)Enum.Parse(typeof(CardsName), nemo);
                        if (name != CardsName.NONE)
                        {
                            Deck.Add(new Cards(name, typo, symbol[i], color[i]));
                        }
                    }
                }
                i++;
            }
            start();
        }

        void start()
        {
            
            foreach (Cards card in Deck)
            {
                Console.ForegroundColor = card.GetColor();
                card.Deal();
            }
            Cards Error = new Cards(CardsName.NONE, CardsType.ERROR, "/!\\", ConsoleColor.DarkYellow);
            Console.ForegroundColor = Error.GetColor();
            Error.Deal();
            Console.WriteLine(Deck.Count());

            Console.ReadLine();
        }
    }
}
