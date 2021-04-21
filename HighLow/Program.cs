using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLow
{
    class Program
    {
        List<Cards> StaticDeck = new List<Cards>();
        List<Cards> Deck = new List<Cards>();
        const int deckSize = 52;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            /*   try { Console.SetWindowSize(125, 30); Console.SetBufferSize(125,30); }
                 catch { Console.SetWindowSize(200, 50); }*/
            Program program = new Program();
            while (true)
            {
                Console.Clear();
                program.CreateDeck();
            }

        }
        void CreateDeck()
        {
            CardsType type;
            CardsName name;
            string[] symbol = new string[] { "♣ ", "♦", "♥", "♠", " " };
            ConsoleColor[] color = new ConsoleColor[] { ConsoleColor.Cyan, ConsoleColor.DarkRed, ConsoleColor.Magenta, ConsoleColor.Green, ConsoleColor.DarkYellow };
            int i = 0;

            foreach (string typo in Enum.GetNames(typeof(CardsType)))
            {
                type = (CardsType)Enum.Parse(typeof(CardsType), typo);

                if (type != CardsType.ERROR)
                {
                    foreach (string nemo in Enum.GetNames(typeof(CardsName)))
                    {
                        name = (CardsName)Enum.Parse(typeof(CardsName), nemo);
                        if (name != CardsName.NONE)
                        {
                            StaticDeck.Add(new Cards(name, type, symbol[i], color[i]));
                        }
                    }
                }
                i++;
            }
            randomize();
        }
        void randomize()
        {
            Random random = new Random();
            int temp;
            for (int i = 0; i < deckSize; i++)
            {
                temp = random.Next(StaticDeck.Count);
                Deck.Add(StaticDeck[temp]);
                StaticDeck.Remove(StaticDeck[temp]);
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
            Deck.Clear();
        }

    }
}
