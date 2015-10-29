
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using RankingSystems.Interfaces;

namespace RankingSystems
{
    public class Player : IRanked
    {
        private readonly List<Rank> _rankings;

        public Player(Rank initRank)
        {
            Contract.Requires(initRank != null);
            Rank = initRank;
            _rankings = new List<Rank> { initRank };
        }

        public Player(double initRanking)
        {
            var rank = new Rank(initRanking);
            this.Rank = rank;
            _rankings = new List<Rank> { rank };
        }

        public Rank Rank { get; }

        public Player SetNewRank(Rank rank)
        {
            Contract.Requires(rank != null);

            return new Player(rank);
        }
    }
}
