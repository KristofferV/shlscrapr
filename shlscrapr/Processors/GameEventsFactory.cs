using System.Collections.Generic;
using System.Linq;
using shlscrapr.Models;

namespace shlscrapr.Processors
{
    public class GameEventsFactory
    {
        public static GameEvents HandleEvents(List<Event> events, List<GamePlay> gamePlays, string teamId, int round)
        {
            var gameEvents = new List<GameEvent>();
            var gameId = events.First().GameId;
            var teamEvents = events.Where(e => e.Team != "SHL" && e.Class != "Period").GroupBy(e => e.Team).ToList();

            foreach (var teamEvent in teamEvents)
            {
                var teamName = teamEvent.Key;

                foreach (var liveEvent in teamEvent)
                {
                    var eventStartTime = GameTimeCalculator.Calculate(liveEvent.Period, liveEvent.TimePeriod);

                    var gamePlay =
                        gamePlays.FirstOrDefault(p => p.StartTime <= eventStartTime && p.EndTime > eventStartTime) ??
                        gamePlays.Last();

                    var gameEvent = new GameEvent()
                    {
                        GameId = gameId,
                        Round = round,
                        TeamName = teamName,
                        HomeTeam = teamId == teamName,
                        EventId = liveEvent.EventId,
                        Class = liveEvent.Class,
                        Description = liveEvent.Description,
                        GameTime = liveEvent.GameTime,
                        StartTime = eventStartTime,
                        Type = liveEvent.Type,
                        LocationX = liveEvent.Location != null ? liveEvent.Location.X : -1,
                        LocationY = liveEvent.Location != null ? liveEvent.Location.Y : -1,
                        HomeTeamAdvantage = gamePlay.HomeTeamAdvantage,
                        PlayersOnIce = gamePlay.PlayersOnIce,
                        Period = liveEvent.Period,
                    };

                    gameEvents.Add(gameEvent);
                }
            }

            return new GameEvents {Items = gameEvents.OrderBy(g => g.EventId).ToList()};
        }        
    }
}