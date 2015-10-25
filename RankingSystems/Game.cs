
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;

namespace RankingSystems
{
    public class Game
    {
        private Team _winner;

        public Game(Team teamA, Team teamB)
        {
            Contract.Requires(teamA != null && teamB != null);

            this.TeamA = teamA;
            this.TeamB = teamB;
        }

        public Team TeamA { get; }
        public Team TeamB { get; }

        public Team Winner
        {
            get { return _winner; }
            set
            {
                Contract.Requires(value.Equals(this.TeamA) || value.Equals(this.TeamB));

                _winner = value;
            }
        }


        public IEnumerable<Result> GetResults()
        {
            yield return new Result(this.TeamA, this.TeamB, this.Winner);
            yield return new Result(this.TeamB, this.TeamA, this.Winner);
        }
    }
}
