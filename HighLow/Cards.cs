using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLow_2._0
{
    class Cards
    {
        private string symbol;
        private ConsoleColor color;
        private CardsType type;
        private CardsName name;

        public Cards(CardsName name, CardsType type, string symbol, ConsoleColor color)
        {
            this.name = name;
            this.type = type;
            this.symbol = symbol;
            this.color = color;
        }

        public ConsoleColor GetColor() { return color; }
        public CardsType Type() { return type; }
        public CardsName Name() { return name; }
        public int Value() { return (int)name; }
    }

    public enum CardsType
    {
        CLUBS, DIAMONDS, HEARTS, SPADES
    }
    public enum CardsName
    {
        TWO = 2,
        THREE = 3,
        FOUR = 4,
        FIVE = 5,
        SIX = 6,
        SEVEN = 7,
        EIGHT = 8,
        NINE = 9,
        TEN = 10,
        JACK = 11,
        QUEEN = 12,
        KING = 13,
        ACE = 999
    }
}
