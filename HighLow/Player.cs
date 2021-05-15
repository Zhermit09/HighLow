using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLow_2._0
{
    class Player
    {
        public string nick;
        public int score;
        public Player(string nick, int score)
        {
            this.nick = nick;
            this.score = score;
        }

        public int GetScore()
        {
            return score;
        }

        public string Name()
        {
            return nick;

        }

        public void SetScore(int score)
        {
            this.score = score;
        }
        public void ShowScore()
        {
            Console.WriteLine("'" + nick + "' Score: " + score);
        }
    }
}
