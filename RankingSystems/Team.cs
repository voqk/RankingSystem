using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RankingSystems.Interfaces;

namespace RankingSystems
{
    public class Team : ITeam
    {
        private readonly List<IRanked> _players;

        public Team(params IRanked[] players)
        {
            Contract.Requires(players != null && players.Any());

            _players = players.ToList();
        }

        // Team of one.
        public Team(IRanked player)
        {
            Contract.Requires(player != null);

            _players = new List<IRanked>(1) { player };
        }

        public Rank Rank => new Rank(_players.Select(p => p.Rank.Value).Average());

        public IEnumerable<IRanked> Players => _players;
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
