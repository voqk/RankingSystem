
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using RankingSystems.Interfaces;

namespace RankingSystems
{
    public class Game
    {
        private ITeam _winner;

        public Game(ITeam teamA, ITeam teamB)
        {
            Contract.Requires(teamA != null && teamB != null);

            this.TeamA = teamA;
            this.TeamB = teamB;
        }

        public ITeam TeamA { get; }
        public ITeam TeamB { get; }

        public ITeam Winner
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
