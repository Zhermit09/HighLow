using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLow
{
    class Player
    {

        int score;
        int round = 1;

        public void setScore(int score) { this.score = score; }

        public int getScore() { return score; }

        public void setRound(int round) { this.round = round; }

        public int getRound() { return round; }

        public void resetRound()
        {
            this.round = 1;
        }
    }
}
