using System.Collections.Generic;
using System.Linq;
using shlscrapr.Models;

namespace shlscrapr.Processors.Penalties
{
    public class PenaltyScoreBoard
    {
        private readonly IEnumerable<PlayEvent> _homePenalties;
        private readonly IEnumerable<PlayEvent> _awayPenalties;

        public PenaltyScoreBoard(IEnumerable<PlayEvent> penalties)
        {
            var ps = penalties.WithoutKvittningar().OrderBy(p => p.PenaltyTime);
            _homePenalties = ps.Where(p => p.HomeTeam = true);
            _awayPenalties = ps.Where(p => p.HomeTeam = false);
        }

        public IEnumerable<PlayEvent> Home { get { return _homePenalties; } }
        public IEnumerable<PlayEvent> Away { get { return _awayPenalties; } } 
    }
}