using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankingSystems;

namespace Elo.Test
{
    [TestClass]
    public class SinglesTests
    {
        [TestMethod]
        public void TestPlayerAGainsWhatPlayerBLoses()
        {
            var elo = new EloSystem(k: 32);
            var rp = new RatingPeriod(elo);
            var playerAStart = 1500;
            var playerA = new Player(playerAStart);
            var teamA = new Team(playerA);
            var playerBStart = 1700;
            var playerB = new Player(playerBStart);
            var teamB = new Team(playerB);

            rp.AddGame(teamA.Defeats(teamB));
            var newRankings = rp.UpdateRankings();
            var diffA = Math.Abs(newRankings.Where(p => p.Item1 == playerA).First().Item2.Value - playerAStart);
            var diffB = Math.Abs(newRankings.Where(p => p.Item1 == playerB).First().Item2.Value - playerBStart);

            Assert.AreEqual(diffA, diffB, .1);
        }

        [TestMethod]
        public void TestMatchAgainstWikipediaValues()
        {
            // Suppose Player A has a rating of 1613, and plays in a five-round tournament. 
            // He or she loses to a player rated 1609, draws with a player rated 1477, 
            // defeats a player rated 1388, defeats a player rated 1586, and loses to a 
            // player rated 1720. The player's actual score is 
            // (0 + 0.5 + 1 + 1 + 0) = 2.5. The expected score, calculated according to 
            // the formula above, was (0.506 + 0.686 + 0.785 + 0.539 + 0.351) = 2.867. 
            // Therefore, the player's new rating is (1613 + 32×(2.5 − 2.867)) = 1601, 
            // assuming that a K-factor of 32 is used.

            var elo = new EloSystem(k: 32);
            var tournament = new RatingPeriod(elo);

            var playerA = new Player(1613);
            var teamA = new Team(playerA);

            var games = new[]
            {
                teamA.LosesTo(new Team(new Player(1609))),
                teamA.Draws(new Team(new Player(1477))),
                teamA.Defeats(new Team(new Player(1388))),
                teamA.Defeats(new Team(new Player(1586))),
                teamA.LosesTo(new Team(new Player(1720)))
            };

            foreach (var game in games)
            {
                tournament.AddGame(game);
            }

            var teamsToNewRank = tournament.UpdateRankings();
            var teamANewRank = teamsToNewRank.First(tup => tup.Item1 == playerA).Item2;
            Assert.AreEqual(1601, teamANewRank.Value, 1.0);
        }
    }
}
