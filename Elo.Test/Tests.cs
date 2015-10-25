using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RankingSystems;

namespace Elo.Test
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestPlayerAGainsWhatPlayerBLoses()
        {
            var elo = new EloSystem(k: 32);
            var rp = new RatingPeriod(elo);
            var playerAStart = 1500;
            var playerA = new Player(new Ranking(playerAStart, DateTimeOffset.UtcNow));
            var playerBStart = 1700;
            var playerB = new Player(new Ranking(playerBStart, DateTimeOffset.UtcNow));

            rp.AddGame(playerA.Defeats(playerB));
            rp.UpdateRankings(DateTimeOffset.UtcNow);
            var diffA = Math.Abs(playerA.Ranking.Value - playerAStart);
            var diffB = Math.Abs(playerB.Ranking.Value - playerBStart);

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

            var playerA = new Player(new Ranking(1613, DateTimeOffset.Now));

            var games = new[]
            {
                playerA.LosesTo(new Player(new Ranking(1609, DateTimeOffset.Now))),
                playerA.Draws(new Player(new Ranking(1477, DateTimeOffset.Now))),
                playerA.Defeats(new Player(new Ranking(1388, DateTimeOffset.Now))),
                playerA.Defeats(new Player(new Ranking(1586, DateTimeOffset.Now))),
                playerA.LosesTo(new Player(new Ranking(1720, DateTimeOffset.Now)))
            };

            foreach (var game in games)
            {
                tournament.AddGame(game);
            }

            tournament.UpdateRankings(DateTimeOffset.Now);
            Assert.AreEqual(1601, playerA.Ranking.Value, 1.0);
        }
    }
}
