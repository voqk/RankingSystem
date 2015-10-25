using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingSystems
{
    public class Team
    {
        private readonly List<Player> _players;

        public Team(params Player[] players)
        {
            Contract.Requires(players != null && players.Any());

            _players = players.ToList();
        }

        // Team of one.
        public Team(Player player)
        {
            Contract.Requires(player != null);

            _players = new List<Player>(1) { player };
        }

        public Ranking Ranking => new Ranking(_players.Select(p => p.Ranking.Value).Average(), DateTimeOffset.Now);

        public IReadOnlyList<Player> Players => _players.AsReadOnly();
    }

    /// <summary>
    /// Convenient methods to create Game objects.
    /// </summary>
    public static class TeamExtensions
    {
        public static Game Defeats(this Team team, Team opponent)
        {
            Contract.Requires(team != null && opponent != null);

            var game = new Game(team, opponent);
            game.Winner = game.TeamA;
            return game;
        }

        public static Game LosesTo(this Team team, Team opponent)
        {
            Contract.Requires(team != null && opponent != null);

            var game = new Game(team, opponent);
            game.Winner = game.TeamB;
            return game;
        }

        public static Game Draws(this Team team, Team opponent)
        {
            Contract.Requires(team != null && opponent != null);

            return new Game(team, opponent);
        }
    }
}
