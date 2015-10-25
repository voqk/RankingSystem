using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingSystems
{
    public class Result
    {
        public Result(Player player, Player opponent, Player winner)
        {
            Contract.Requires(player != null && opponent != null);
            Contract.Requires(winner == null || winner.Equals(player) || winner.Equals(opponent));

            this.Player = player;
            this.Opponent = opponent;

            this.ActualScore = GetActualResult(player, winner);
        }

        public Player Player { get; private set; }

        public Player Opponent { get; private set; }

        public double ActualScore { get; private set; }

        public double ExpectedScore
        {
            get
            {
                var rPlayer = this.Player.Ranking.Value;
                var rOpponent = this.Opponent.Ranking.Value;
                var qPlayer = Math.Pow(10, rPlayer / 400);
                var qOpponent = Math.Pow(10, rOpponent / 400);

                return qPlayer/(qPlayer + qOpponent);
            }
        }

        private static double GetActualResult(Player player, Player winner)
        {
            if (winner == null) // Draw
            {
                return 0.5;
            }

            if (winner.Equals(player)) // Win
            {
                return 1.0;
            }

            // Lose
            return 0.0;
            
        }
    }
}
