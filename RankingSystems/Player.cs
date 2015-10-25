
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace RankingSystems
{
    public class Player
    {
        private readonly List<Ranking> _rankings;
         
        public Player(Ranking initRanking)
        {
            Contract.Requires(initRanking != null);
            _rankings = new List<Ranking> { initRanking };
        }

        public Player(double initRanking)
        {
            _rankings = new List<Ranking> { new Ranking(initRanking, DateTimeOffset.UtcNow)};
        }

        public Ranking Ranking => _rankings.OrderBy(r => r.TimeStamp).Last();

        public IReadOnlyList<Ranking> All => _rankings.OrderBy(r => r.TimeStamp).ToList().AsReadOnly();

        public void UpdateRanking(Ranking ranking)
        {
            Contract.Requires(ranking != null);

            _rankings.Add(ranking);
        }
    }
}
