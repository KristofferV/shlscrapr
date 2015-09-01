using System.Collections.Generic;
using System.Linq;
using shlscrapr.Models;

namespace shlscrapr.Processors.Penalties
{
    public static class PlayTypeExtensions
    {
        public static PlayEvent GetPenaltyThatExpiresForPowerPlayGoal(this IEnumerable<PlayEvent> events, PlayEvent powerPlayGoal)
        {
            var penalties = events
                .OrderBy(p => p.PenaltyTime)
                .Where(p => p.PenaltyIsMinor)
                .Where(p => p.HomeTeam != powerPlayGoal.HomeTeam)
                .Where(p => p.StartTime <= powerPlayGoal.StartTime && p.EndTime >= powerPlayGoal.StartTime);
            return penalties.FirstOrDefault() ?? new PlayEvent { Description = "No Penalty FOUND!!", StartTime = powerPlayGoal.StartTime};
        }
    }
}