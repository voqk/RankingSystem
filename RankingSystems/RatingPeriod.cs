using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

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

            var resultsByTeam = results.GroupBy(r => r.Team, (t, rs) => new { Team = t, Results = rs });

            var rankingCache = new Dictionary<Player, Ranking>();

            // Calculate new rankings but don't apply them until iterating through
            // the entire set.
            foreach (var teamAndResults in resultsByTeam)
            {
                var team = teamAndResults.Team;
                var expected = teamAndResults.Results.Select(r => r.ExpectedScore).Sum();
                var actual = teamAndResults.Results.Select(r => r.ActualScore).Sum();

                foreach (var player in team.Players)
                {
                    var oldRanking = player.Ranking.Value;
                    var newRanking = oldRanking + _elo.K * (actual - expected);
                    rankingCache.Add(player, new Ranking(newRanking, timeStamp));
                }
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
