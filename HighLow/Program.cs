using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HighLow_2._0
{
    class Program
    {
        List<Cards> StaticDeck = new List<Cards>();
        List<Cards> Deck = new List<Cards>();
        List<Player> HighScore = new List<Player>();

        int score;
        string path = Path.GetFullPath(@"..\..\HighScoreList.txt");

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetWindowSize(125, 30);


            Program program = new Program();
            program.gameLoop();

            Console.WriteLine("Exiting");
            Console.ReadLine();
        }

        void gameLoop()
        {
            bool exit = false;
            fileRead();
            title();

            while (exit == false)
            {
                score = 0;
                Console.Clear();
                Deck.Clear();
                CreateDeck();
                meny(ref exit);
            }
        }

        void title()
        {
            string[] titile = new string[]
           {
                " AMV   VMA       VMV                  MA              AMMMA         MA                                       ",
                "AMV     VMA                           MMA            AV   VA        MMA                                      ",
                "MMA     AMM    AMMMA                  AMV            VA   AV        MMM              AMMMMA       AMV     VMA",
                "MMMMMMMMMMM      VMM     AMMMMMMA     MMMVMMMMMA      AMMMA    MA   MMM            AMV    VMA     MV  VMV  VM",
                "MMV     VMM      AMM    AMV    VMA    MMV    VMMA   AMV   VM AV  Y  MMM           AMV      VMA    MA  AMA  AM",
                "MV       VM      VMV    VMA    AMM    MMA     AMM   MV     VMV      MMV           VMA      AMV    VMAMMMMMMMV",
                "VA       AV      AMA     VMMMMMMMV    MMM     MMM   VA     AVMA     MMA            VMA    AMV      VMV   VMV ",
                " VA     AV    AMMMMMA          AMV    VMV     VMV    VMMMMMV   MA   VMMMMMMMMMMA     VMMMMV        AMA   AMA ",
                "                           AMMMMV                                T                                           "
           };

            int i = 0;
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (string s in titile)
            {

                Console.SetCursorPosition((Console.WindowWidth - 109) / 2, ((Console.WindowHeight - 9) / 2) + i);
                Console.Write(s);
                i++;
                Thread.Sleep(50);
            }
            Console.ResetColor();
            Console.SetCursorPosition((Console.WindowWidth - "Press 'Enter' to start".Length) / 2, Console.CursorTop + 5);
            Console.WriteLine("Press 'Enter' to start");
            Console.ReadLine();
            Console.Clear();
        }
        void fileRead()
        {
            int lenght;
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "");
            }
            string[] data = File.ReadAllLines(path);

            lenght = (data.Length % 2 == 0) ? data.Length : data.Length - 1;

            for (int i = 0; i < lenght; i += 2)
            {
                HighScore.Add(new Player(data[i], int.Parse(data[i + 1])));
            }
            sort();
        }

        void CreateDeck()
        {
            CardsType type;
            CardsName name;
            string[] symbol = new string[] { "♣ ", "♦", "♥", "♠" };
            ConsoleColor[] color = new ConsoleColor[] { ConsoleColor.Cyan, ConsoleColor.DarkRed, ConsoleColor.Magenta, ConsoleColor.Green};
            int i = 0;

            foreach (string typo in Enum.GetNames(typeof(CardsType)))
            {
                type = (CardsType)Enum.Parse(typeof(CardsType), typo);
                foreach (string nemo in Enum.GetNames(typeof(CardsName)))
                {
                    name = (CardsName)Enum.Parse(typeof(CardsName), nemo);

                    StaticDeck.Add(new Cards(name, type, symbol[i], color[i]));
                }
                i++;
            }
            randomize();
        }
        void randomize()
        {
            Random random = new Random();
            int temp;
            for (int i = 0; i < 52; i++)
            {
                temp = random.Next(StaticDeck.Count);
                Deck.Add(StaticDeck[temp]);
                StaticDeck.Remove(StaticDeck[temp]);
            }

        }
        void meny(ref bool exit)
        {
            Console.Clear();
            Console.Write(
                    " Meny\n" +
                    "---------------------------------\n\n" +
                    " 1. Start\n" +
                    " 2. High Scores\n" +
                    " 3. Exit\n" +
                    "\nYour choice: ");
            string answer = Console.ReadLine().ToLower().Trim();
            Console.Clear();

            switch (answer)
            {
                case "1":
                case "s":
                case "start":
                    gameStart();
                    break;
                case "2":
                case "h":
                case "high":
                case "high scores":
                    showHighScores();
                    Console.ReadLine();
                    break;

                case "3":
                case "e":
                case "exit":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Unkown Command");
                    Console.ReadLine();
                    meny(ref exit);
                    break;
            }
        }

        void gameStart()
        {
            for (int round = 1; round <= 4; round++)
            {
                Console.Clear();
                gameRound(13 * (round - 1), 13 * round - 1, round);
            }
            spiral();
            Console.WriteLine("Game over, score: " + score);
            getName();
            Console.ReadLine();
        }

        void gameRound(int card, int lastCard, int round)
        {

            bool roundAlive = true;


            while (roundAlive)
            {
                if (card != lastCard) higerLower(card, ref roundAlive, round);
                else
                {
                    roundOver(ref roundAlive);
                    Console.ReadLine();
                }
                card++;
                Console.Clear();
            }
        }
        void higerLower(int card, ref bool roundAlive, int round)
        {
            Console.WriteLine("Round " + round + " Card# " + ((card + 1) - ((round - 1) * 13)) + " Score: " + score);
            Console.ForegroundColor = Deck[card].GetColor();
            cardDeal(card);
            Console.ForegroundColor = ConsoleColor.White;

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
                    cardCheck(card, ref roundAlive, true);

                    break;

                case "lower":
                case "l":
                case "2":
                    cardCheck(card, ref roundAlive, false);

                    break;
                default:
                    Console.WriteLine("Unknown command");
                    Console.ReadLine();
                    Console.Clear();
                    higerLower(card, ref roundAlive, round);
                    break;
            }
        }

        void cardDeal(int card)
        {


            string[] diamonds = new string[]{
                          " ___________________",
                         @"|                   |",
                         @"|         A         |",
                         @"|        / \        |",
                         @"|       /   \       |",
                         @"|      /     \      |",
                         @"|     /       \     |",
                         @"|    /         \    |",
                         @"|   /           \   |",
                         @"|   \           /   |",
                         @"|    \         /    |",
                         @"|     \       /     |",
                         @"|      \     /      |",
                         @"|       \   /       |",
                         @"|        \ /        |",
                         @"|         V         |",
                         @"|___________________|" };
            string[] hearts = new string[]{
                           " ___________________",
                          @"|                   |",
                          @"|                   |",
                          @"|                   |",
                          @"|    ___     ___    |",
                          @"|  _/   \_ _/   \_  |",
                          @"| /       V       \ |",
                          @"||                 ||",
                          @"|\                 /|",
                          @"| \_             _/ |",
                          @"|   \           /   |",
                          @"|    \_       _/    |",
                          @"|      \_   _/      |",
                          @"|        \ /        |",
                          @"|         V         |",
                          @"|                   |",
                          @"|___________________|" };
            string[] spades = new string[]{
                          " ___________________",
                         @"|                   |",
                         @"|                   |",
                         @"|         A         |",
                         @"|       _/ \_       |",
                         @"|     _/     \_     |",
                         @"|    /         \    |",
                         @"|  _/           \_  |",
                         @"| /               \ |",
                         @"|/                 \|",
                         @"||                 ||",
                         @"| \_             _/ |",
                         @"|   \___/| |\___/   |",
                         @"|       _/ \_       |",
                         @"|      /_____\      |",
                         @"|                   |",
                          "|___________________|" };
            string[] clubs = new string[]{
                          " ___________________",
                         @"|                   |",
                         @"|                   |",
                         @"|                   |",
                         @"|       _____       |",
                         @"|      /     \      |",
                         @"|     (       )     |",
                         @"|   __ \_   _/ __   |",
                         @"| _/  \__\ /__/  \_ |",
                         @"|/                 \|",
                         @"|\_     __ __     _/|",
                         @"|  \___/ / \ \___/  |",
                         @"|        | |        |",
                         @"|       /___\       |",
                         @"|                   |",
                         @"|                   |",
                          "|___________________|" };
            string[] unknown = new string[]{
                          " ___________________",
                         @"|      _______      |",
                         @"|     / _____ \     |",
                         @"|    / /     \ \    |",
                         @"|   / /       \ \   |",
                         @"|  / /         \ \  |",
                         @"|  \ \         / /  |",
                         @"|   \_|      _/_/   |",
                         @"|          _/_/     |",
                         @"|         / /       |",
                         @"|        / /        |",
                         @"|        \_|        |",
                         @"|         _         |",
                         @"|        / \        |",
                         @"|        \_/        |",
                         @"|                   |",
                         @"|___________________|" };

            string[] temp = new string[17];

            switch (Deck[card].Type())
            {
                case CardsType.CLUBS:
                    temp = clubs;
                    break;
                case CardsType.DIAMONDS:
                    temp = diamonds;
                    break;
                case CardsType.HEARTS:
                    temp = hearts;
                    break;
                case CardsType.SPADES:
                    temp = spades;
                    break;
            }

            int y = Console.CursorTop;
            int row = 0;

            Console.ForegroundColor = Deck[card].GetColor();

            for (int i = 0; i < 2; i++)
            {
                foreach (string s in temp)
                {
                    Console.SetCursorPosition((i * 22), y + row);
                    row++;
                    Console.Write(s);
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                temp = unknown;
                row = 0;
            }
            Console.WriteLine("\n\n");
            int top = Console.CursorTop;


            string heading = Deck[card].Name() + " " + Deck[card].Type();
            Console.SetCursorPosition((21 - heading.Length) / 2, y + 1);
            Console.ForegroundColor = Deck[card].GetColor();
            Console.Write(heading);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, top);

        }
        void cardCheck(int card, ref bool roundAlive, bool guess)
        {
            if (Deck[card].Value() == Deck[card + 1].Value())
            {
                Console.WriteLine("It's a pair!");
                roundOver(ref roundAlive);
            }
            else if (Deck[card].Name() == CardsName.ACE || Deck[card + 1].Name() == CardsName.ACE)
            {
                Console.WriteLine("It's an Ace!");
                awardPoints(card);
            }
            else { guessCheck(card, ref roundAlive, guess); }
            Console.ReadLine();
        }

        void guessCheck(int card, ref bool roundAlive, bool guess)
        {
            if (guess == (Deck[card].Value() < Deck[card + 1].Value()))
            {
                awardPoints(card);
            }
            else if (guess != (Deck[card].Value() < Deck[card + 1].Value()))
            {
                Console.Clear();
                Console.WriteLine("Wrong");
                roundOver(ref roundAlive);
            }

        }
        void awardPoints(int card)
        {
            score += 1 + ((card + 1) % 13 == 0 ? 50 : 0);
            Console.WriteLine("Points +" + (1 + ((card + 1) % 13 == 0 ? 50 : 0)));
        }
        void roundOver(ref bool roundAlive)
        {
            roundAlive = false;
            Console.WriteLine("Round over");

        }

        void spiral()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            int counter = (height < width) ? height : width;

            Console.SetCursorPosition((width-"Game over".Length) / 2, height/2);
            Console.WriteLine("Game over");

            Console.BackgroundColor = ConsoleColor.DarkYellow;

            for (int loop = 0; loop < counter / 2; loop++)
            {
                for (int w = 0 + loop; w <= width - 1 - loop; w++)
                {
                    Console.SetCursorPosition(w, 0 + loop);
                    Console.Write(" ");
                
                }
                for (int h = 1 + loop; h <= height - 2 - loop; h++)
                {
                    Console.SetCursorPosition(width - 1 - loop, h);
                    Console.Write(" ");
                    Thread.Sleep(1);
                }
                for (int w = width - 1 - loop; w >= 0 + loop; w--)
                {
                    Console.SetCursorPosition(w, height - 2 - loop);
                    Console.Write(" ");
                }
                for (int h = height - 2 - loop; h >= 0 + loop; h--)
                {
                    Console.SetCursorPosition(0 + loop, h);
                    Console.Write(" ");
                    Thread.Sleep(1);
                }
            }

            Console.ResetColor();
            Console.Clear();
        }

        void getName()
        {
            Console.Write("How would you like to be remembered?\n" +
                "Name: ");
            string name = Console.ReadLine().Trim();
            updateScore(name);
            Console.Clear();
            showHighScores();
        }

        void updateScore(string name)
        {
            Player temp = HighScore.Find(x => x.nick.Contains(name));
            if (temp != null)
            {
                if (temp.GetScore() < score)
                {
                    Console.WriteLine("New Personal Record!");
                    temp.SetScore(score);
                }
                else if (temp.score >= score)
                {
                    Console.WriteLine("No new Record");
                }
                Console.ReadLine();
            }
            else
            {
                HighScore.Add(new Player(name, score));
            }
            fileWrite();
        }
        void fileWrite()
        {
            string[] write = new string[HighScore.Count * 2];
            int i = 0;
            foreach (Player player in HighScore)
            {
                write[i] = player.Name();
                i++;
                write[i] = player.GetScore().ToString();
                i++;
            }
            sort();
            File.WriteAllLines(path, write);
        }
        void sort()
        {
            HighScore.Sort((x, y) => y.score.CompareTo(x.score));
            if (HighScore.Count > 5)
            {
                for (int i = HighScore.Count - 1; i >= 5; i--)
                {
                    HighScore.Remove(HighScore[i]);
                }
            }
        }

        void showHighScores()
        {
            Console.WriteLine("HIGH SCORES TOP 5:\n" +
                "---------------------------------\n");
            foreach (Player player in HighScore)
            {
                player.ShowScore();
            }
            if (HighScore.Count == 0) Console.WriteLine("No Scores yet, be the first!");
        }
    }
}
