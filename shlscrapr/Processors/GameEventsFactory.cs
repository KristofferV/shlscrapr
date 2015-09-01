using System.Collections.Generic;
using System.Linq;
using shlscrapr.Models;

namespace shlscrapr.Processors
{
    public class GameEventsFactory : IGameEventsFactory
    {
        public GameEvents HandleEvents(List<Event> events, List<GamePlayState> gamePlayStates, string teamId, int round)
        {
            var gameEvents = new List<GameEvent>();
            var gameId = events.First().GameId;

            foreach (var teamEvent in events.TeamEvents())
            {
                var teamName = teamEvent.Key;

                foreach (var liveEvent in teamEvent)
                {
                    var eventStartTime = GameTimeCalculator.Calculate(liveEvent.Period, liveEvent.TimePeriod);

                    var gamePlayState = gamePlayStates.StateThisSecond(eventStartTime);

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
                        HomeTeamAdvantage = gamePlayState.HomeTeamAdvantage,
                        PlayersOnIce = gamePlayState.PlayersOnIce,
                        Period = liveEvent.Period,
                    };

                    gameEvents.Add(gameEvent);
                }
            }

            return new GameEvents {Items = gameEvents.OrderBy(g => g.EventId).ToList()};
        }        
    }
}