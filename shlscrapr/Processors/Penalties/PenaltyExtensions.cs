using System.Collections.Generic;
using System.Linq;
using shlscrapr.Models;

namespace shlscrapr.Processors.Penalties
{
    public static class PenaltyExtensions
    {
        public static void CalculateKvittningar(this IEnumerable<PlayEvent> penalties, PlayersOnIce playersOnIce)
        {
            var penaltyList = penalties.ToList();

            var homePenalties = penaltyList.Where(p => p.HomeTeam && !p.PenaltyIsMisconduct).ToList();
            var awayPenalties = penaltyList.Where(p => !p.HomeTeam && !p.PenaltyIsMisconduct).ToList();

            if (IsExceptionToRule(playersOnIce, homePenalties, awayPenalties))
                return;

            foreach (var homePenalty in homePenalties)
            {
                var awayPenaltyThatShouldKvittas = awayPenalties.WithoutKvittningar().FirstOrDefault(p => p.StartTime == homePenalty.StartTime && p.PenaltyTime == homePenalty.PenaltyTime);
                if (awayPenaltyThatShouldKvittas == null)
                    continue;

                homePenalty.IsKvittad = true;
                awayPenaltyThatShouldKvittas.IsKvittad = true;
            }

            //Handle extra minors if original penalty is kvittad they should get the original startime
            foreach (var penalty in penaltyList.WithoutKvittningar().Where(p => p.HasOriginalPenalty))
            {
                penalty.HandleOriginalPenaltyIsKvittad();
            }
        }

        public static void HandleOriginalPenaltyIsKvittad(this PlayEvent penalty)
        {
            if (!penalty.PenaltyIsMinor || !penalty.OriginalPenalty.PenaltyIsMinor || !penalty.OriginalPenalty.IsKvittad) return;

            penalty.StartTime = penalty.OriginalPenalty.StartTime;
            penalty.EndTime = penalty.OriginalPenalty.EndTime;
        }

        private static bool IsExceptionToRule(PlayersOnIce playersOnIce, IEnumerable<PlayEvent> homePenalties, IEnumerable<PlayEvent> awayPenalties)
        {
            return playersOnIce == PlayersOnIce.FiveOnFive 
                   && homePenalties.Count(p => p.PenaltyIsMinor) == 1
                   && awayPenalties.Count(p => p.PenaltyIsMinor) == 1;
        }

        public static IEnumerable<PlayEvent> WithoutKvittningar(this IEnumerable<PlayEvent> penalties)
        {
            return penalties.Where(p => !p.IsKvittad);
        }

        public static IEnumerable<PlayEvent> SplitIntoMultiplePenalties(this IList<PlayEvent> penalties)
        {
            var penaltyList = new List<PlayEvent>();

            penaltyList.AddRange(penalties.Where(p => !p.PenaltyIsDouble));

            foreach (var playEvent in penalties.Where(p => p.PenaltyIsDouble))
            {
                var extraPenalty = new PlayEvent(playEvent);
                if (playEvent.Description.Contains("2 + 2"))
                {
                    var endTime = playEvent.StartTime + 120;
                    playEvent.EndTime = endTime;
                    extraPenalty.StartTime = endTime;
                }
                if (playEvent.Description.Contains("2 + 10"))
                {
                    var endTime = playEvent.StartTime + 120;
                    playEvent.EndTime = endTime;
                    extraPenalty.StartTime = endTime;
                    extraPenalty.EndTime = extraPenalty.StartTime + 600;
                }
                if (playEvent.Description.Contains("5 + GM"))
                {
                    var endTime = playEvent.StartTime + 300;
                    playEvent.EndTime = endTime;
                    extraPenalty.StartTime = endTime;
                    extraPenalty.EndTime = extraPenalty.StartTime + 1200;
                }
                penaltyList.Add(playEvent);
                penaltyList.Add(extraPenalty);
            }

            return penaltyList;
        }
    }
}