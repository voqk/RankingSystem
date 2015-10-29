using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using RankingSystems.Interfaces;

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

        public IEnumerable<Tuple<IRanked, Rank>> UpdateRankings()
        {
            // Get expected and actual scores over the entire rating period
            // for each player. Calulate a new Rank and add the new Rank
            // to the player's rankings.

            var results = _games.SelectMany(g => g.GetResults()).ToList();

            var resultsByTeam = results.GroupBy(r => r.Team, (t, rs) => new { Team = t, Results = rs });

            // Calculate new rankings but don't apply them until iterating through
            // the entire set.
            foreach (var teamAndResults in resultsByTeam)
            {
                var team = teamAndResults.Team;
                var expected = teamAndResults.Results.Select(r => r.ExpectedScore).Sum();
                var actual = teamAndResults.Results.Select(r => r.ActualScore).Sum();

                foreach (var player in team.Players)
                {
                    var oldRanking = player.Rank.Value;
                    var newRanking = oldRanking + _elo.K * (actual - expected);
                    yield return Tuple.Create(player, new Rank(newRanking));
                }
            }
        }
    }
}
