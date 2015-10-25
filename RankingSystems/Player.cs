
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

        public Ranking Ranking => _rankings.OrderBy(r => r.TimeStamp).Last();

        public IReadOnlyList<Ranking> All => _rankings.OrderBy(r => r.TimeStamp).ToList().AsReadOnly();

        public void UpdateRanking(Ranking ranking)
        {
            Contract.Requires(ranking != null);

            _rankings.Add(ranking);
        }
    }

    public static class PlayerExtensions
    {
        public static Game Defeats(this Player player, Player opponent)
        {
            Contract.Requires(player != null && opponent != null);

            return new Game(player, opponent) {Winner = player};
        }

        public static Game LosesTo(this Player player, Player opponent)
        {
            Contract.Requires(player != null && opponent != null);

            return new Game(player, opponent) {Winner = opponent};
        }

        public static Game Draws(this Player player, Player opponent)
        {
            Contract.Requires(player != null && opponent != null);

            return new Game(player, opponent);
        }
    }
}
