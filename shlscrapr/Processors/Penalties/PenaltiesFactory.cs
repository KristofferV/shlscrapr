using System.Collections.Generic;
using System.Linq;
using shlscrapr.Infrastructure;
using shlscrapr.Models;

namespace shlscrapr.Processors.Penalties
{
    public class PenaltiesFactory
    {
        public static List<PlayEvent> Create(IList<Event> events, string homeTeam)
        {
            var penalties = new List<PlayEvent>();

            var penaltyEvents = events.Where(e => e.Class == "Penalty");

            foreach (var penaltyEvent in penaltyEvents)
            {
                var penaltyStartTime = GameTimeCalculator.Calculate(penaltyEvent.Period, penaltyEvent.TimePeriod);
                var penaltyEndTime = CalculatePenaltyEndTime(penaltyEvent.Description, penaltyEvent.Period, penaltyEvent.TimePeriod);

                Logger.Debug(string.Format("Game {0} S {1} E {2} ET {3}", events.First().GameId, penaltyStartTime.ToClockTime(), penaltyEndTime.ToClockTime(), penaltyEvent.Description));

                penalties.Add(new PlayEvent
                {
                    StartTime = penaltyStartTime,
                    EndTime = penaltyEndTime,
                    Class = penaltyEvent.Class,
                    Description = penaltyEvent.Description,
                    HomeTeam = penaltyEvent.Team == homeTeam,
                });
            }

            return penalties;
        }

        private static int CalculatePenaltyLength(string description)
        {
            if (description.Contains("5 + GM"))
                return 300;
            if (description.Contains("Match Penalty"))
                return 300;
            if (description.Contains("5 min"))
                return 300;
            if (description.Contains("2 + 10"))
                return 120;
            if (description.Contains("2 + 2"))
                return 240;
            if (description.Contains("10 min"))
                return 0;
            if (description.Contains("Penalty shot"))
                return 0;            

            return 120;
        }

        private static int CalculatePenaltyEndTime(string description, int period, int timePeriod)
        {
            return CalculatePenaltyLength(description) + GameTimeCalculator.Calculate(period, timePeriod);
        }
    }
}
