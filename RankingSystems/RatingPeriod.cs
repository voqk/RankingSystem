using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RankingSystems
{
    public class RatingPeriod
    {
        private readonly EloSystem _elo;

        private readonly List<Game> _games = new List<Game>();

        public RatingPeriod(EloSystem eloSystem)
        {
            Contract.Requires(eloSystem != null);

            this._elo = eloSystem;
        }

        public void AddGame(Game game)
        {
            Contract.Requires(game != null);

            _games.Add(game);
        }

        public void UpdateRankings(DateTimeOffset timeStamp)
        {
            // Get expected and actual scores over the entire rating period
            // for each player. Calulate a new ranking and add the new ranking
            // to the player's rankings.

            var results = _games.SelectMany(g => g.GetResults()).ToList();

            var resultsByPlayer = results.GroupBy(r => r.Player, (p, rs) => new { Player = p, Results = rs });

            var rankingCache = new Dictionary<Player, Ranking>();

            // Calculate new rankings but don't apply them until iterating through
            // the entire set.
            foreach (var playerAndResults in resultsByPlayer)
            {
                var player = playerAndResults.Player;
                var expected = playerAndResults.Results.Select(r => r.ExpectedScore).Sum();
                var actual = playerAndResults.Results.Select(r => r.ActualScore).Sum();

                var oldRanking = player.Ranking.Value;
                var newRanking = oldRanking + _elo.K*(actual - expected);

                rankingCache.Add(player, new Ranking(newRanking, timeStamp));
            }

            // Now apply new rankings
            foreach (var kvp in rankingCache)
            {
                var player = kvp.Key;
                var ranking = kvp.Value;
                player.UpdateRanking(ranking);
            }
        }
    }
}
