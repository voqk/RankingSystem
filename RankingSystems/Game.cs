
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;

namespace RankingSystems
{
    public class Game
    {
        private Player _winner;

        public Game(Player playerA, Player playerB)
        {
            Contract.Requires(playerA != null && playerB != null);

            this.PlayerA = playerA;
            this.PlayerB = playerB;
        }

        public Player PlayerA { get; }
        public Player PlayerB { get; }

        public Player Winner
        {
            get { return _winner; }
            set
            {
                Contract.Requires(value.Equals(this.PlayerA) || value.Equals(this.PlayerB));
                _winner = value;
            }
        }

        public IEnumerable<Result> GetResults()
        {
            yield return new Result(this.PlayerA, this.PlayerB, this.Winner);
            yield return new Result(this.PlayerB, this.PlayerA, this.Winner);
        }
    }
}
