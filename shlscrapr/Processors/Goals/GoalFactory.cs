using System.Collections.Generic;
using System.Linq;
using shlscrapr.Models;

namespace shlscrapr.Processors.Goals
{
    public static class GoalFactory
    {
        public static List<PlayEvent> Create(IEnumerable<Event> events, string homeTeam)
        {
            return events.Where(e => e.Class == "Goal").Select(e => new PlayEvent
            {
                StartTime = GameTimeCalculator.Calculate(e.Period, e.TimePeriod),
                EndTime = GameTimeCalculator.Calculate(e.Period, e.TimePeriod),
                Class = e.Class,
                Description = e.Description,
                HomeTeam = homeTeam == e.Team,
            }).ToList();
        }
    }
}