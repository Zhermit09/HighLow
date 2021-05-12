using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HighLow
{
    class Program
    {
        List<Cards> StaticDeck = new List<Cards>();
        List<Cards> Deck = new List<Cards>();
        List<Cards> RoundDeck = new List<Cards>();
        Player player = new Player();
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
                ;
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

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            meny();
            Deck.Clear();
        }

        void meny()
        {
            Console.Clear();
            Console.Write(
                    " Meny\n" +
                    "---------------------------------\n\n" +
                    " 1. Begin\n" +
                    " 2. -\n" +
                    " 3. -\n" +
                    " 4. Exit\n" +
                    "\nYour choice: ");
            string answer = Console.ReadLine().ToLower().Trim();

            switch (answer)
            {
                case "1":
                case "b":
                case "begin":
                    Console.Clear();
                    gameStart();
                    break;

                default:
                    unknownCommand();
                    break;
            }
        }

        void gameStart()
        {
            if (player.getRound() < 5)
            {
                clearRound();
                switch (player.getRound())
                {
                    case 1:
                        gameRound(0, 12);
                        break;
                    case 2:
                        gameRound(13, 25);
                        break;
                    case 3:
                        gameRound(26, 38);
                        break;
                    case 4:
                        gameRound(39, 51);
                        break;

                }
               
            }

            Console.WriteLine("Final Score: " + player.getScore());
            Console.ReadLine();
            meny();
        }

        void clearRound()
        {
            Console.Clear();
            Console.WriteLine("Round " + player.getRound());
            Console.ReadLine();
            RoundDeck.Clear();
            Console.Clear();
        }

        void gameRound(int firstCard, int lastCard)
        {
            firstCard += RoundDeck.Count();
            int previousCard = firstCard;
            bool flawless = true;


            for (int i = firstCard + 1; i <= lastCard; i++)
            {
                if (i == (firstCard + 1)) RoundDeck.Add(Deck[firstCard]);
                RoundDeckDeal();
                hiddenCardDeal();
                Console.WriteLine("count :" + RoundDeck.Count() + "\n Score :" + player.getScore());
                compare(higerLower(), i, previousCard, ref flawless);
                RoundDeck.Add(Deck[i]);
                previousCard = i;
                Console.ReadLine();
                Console.Clear();
            }
            RoundDeckDeal();
            Console.WriteLine("count :" + RoundDeck.Count() + "\n Score :" + player.getScore());
            Console.ReadLine();
            player.setRound(player.getRound() + 1);
            gameStart();
        }

        void RoundDeckDeal()
        {
            foreach (Cards card in RoundDeck)
            {
                card.Deal();
            }
        }

        bool higerLower()
        {

            bool higher = false;

            Console.Write("Will next Card be 'Higer' or 'Lower'?\n" +
                "1. Higher\n" +
                "2. Lower\n" +
                   "\nYour choice: ");
            string answer = Console.ReadLine().ToLower().Trim();

            switch (answer)
            {
                case "higher":
                case "h":
                case "1":
                    higher = true;
                    break;

                case "lower":
                case "l":
                case "2":
                    higher = false;
                    break;
                default:
                    higher = false;
                    /*  unknownCommand();
                      RoundDeckDeal();
                      hiddenCardDeal();
                      higerLower();*/
                    break;
            }
            return higher;
        }

        void compare(bool guess, int i, int previousCard, ref bool flawless)
        {
            bool compareCardHigher = false;

            if (Deck[previousCard].Value() < Deck[i].Value()) compareCardHigher = true;
            else if (Deck[previousCard].Value() > Deck[i].Value()) compareCardHigher = false;
            else
            {
                Console.WriteLine("It's a pair!\n" +
                    "You lose!");
                roundOver();

            }

            if (Deck[i].Name() == CardsName.ACE)
            {
                Console.WriteLine("It's an Ace!");
                awardPoints(i, flawless);
            }
            else if (compareCardHigher == guess)
            {
                awardPoints(i, flawless);
            }
            else
            {
                flawless = false;
                Console.WriteLine("Wrong! \n" +
                    "No points");
            }

        }

        void awardPoints(int cardDraw, bool flawless)
        {
            player.setScore(player.getScore() + 1 + ((cardDraw == 12 && flawless) ? 50 : 0));
            Console.WriteLine("Points +" + (1 + ((cardDraw == 12 && flawless) ? 50 : 0)));
            //      Console.ReadLine();
        }

        void roundOver()
        {
            Console.WriteLine("Points +0");
            Console.ReadLine();
            player.setRound(player.getRound() + 1);
            gameStart();
        }

        void hiddenCardDeal()
        {
            Console.WriteLine("Card(?) " + " Name(?) " + " Type(?)");
        }
        void unknownCommand()
        {   //Centerar text
            Console.SetCursorPosition((Console.BufferWidth - 15) / 2, 12);
            Console.WriteLine("UNKNOWN COMMAND\n");
            //Centerar texten som ska blinka
            flicker(((Console.BufferWidth - 26) / 2));
        }

        public void flicker(int x)
        {
            //Får positionen av markören för att senare kunna skriva om på samma plats

            int top = Console.CursorTop;
            int color = 0;

            Console.SetCursorPosition(x, top);
            while (true)
            {
                if (Console.KeyAvailable == false)
                {//Texten blinkar

                    if (color == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        color++;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        color = 0;
                    }
                    Console.Write("Press 'Enter' to continue                   \n" +
                        "                                                        ");
                    Console.SetCursorPosition(x, top);
                    Thread.Sleep(800);
                }
                else if (Console.ReadKey().Key == ConsoleKey.Enter)
                {   //Färgen återställs och allt suddas bort

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    break;
                }
            }
        }
    }
}
