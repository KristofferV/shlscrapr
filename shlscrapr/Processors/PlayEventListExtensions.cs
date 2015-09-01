using System.Collections.Generic;
using System.Linq;
using shlscrapr.Models;

namespace shlscrapr.Processors
{
    public static class PlayEventListExtensions
    {
        public static IList<PlayEvent> EventsThatStartThisSecond(this IList<PlayEvent> events, int second)
        {
            return events.Where(p => p.StartTime == second).ToList();
        }
        public static IList<PlayEvent> Goals(this IList<PlayEvent> events)
        {
            return events.Where(p => p.IsGoal).ToList();
        }
        public static IList<PlayEvent> Penalties(this IList<PlayEvent> events)
        {
            return events.Where(p => p.IsPenalty).ToList();
        }
    }
}
