using System.Collections.Generic;

namespace RankingSystems.Interfaces
{
    public interface ITeam : IRanked
    {
        IEnumerable<IRanked> Players { get; } 
    }
}
