﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingSystems
{
    public class Result
    {
        public Result(Team team, Team opponent, Team winner)
        {
            Contract.Requires(team != null && opponent != null);
            Contract.Requires(winner == null || winner.Equals(team) || winner.Equals(opponent));

            this.Team = team;
            this.Opponent = opponent;

            this.ActualScore = GetActualResult(team, winner);
        }

        public Team Team { get; private set; }

        public Team Opponent { get; private set; }

        public double ActualScore { get; private set; }

        public double ExpectedScore
        {
            get
            {
                var rTeam = this.Team.Ranking.Value;
                var rOpponent = this.Opponent.Ranking.Value;
                var qTeam = Math.Pow(10, rTeam / 400);
                var qOpponent = Math.Pow(10, rOpponent / 400);

                return qTeam/(qTeam + qOpponent);
            }
        }

        private static double GetActualResult(Team team, Team winner)
        {
            if (winner == null) // Draw
            {
                return 0.5;
            }

            if (winner.Equals(team)) // Win
            {
                return 1.0;
            }

            // Lose
            return 0.0;
            
        }
    }
}
