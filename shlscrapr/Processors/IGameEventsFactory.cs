using System.Collections.Generic;
using shlscrapr.Models;

namespace shlscrapr.Processors
{
    public interface IGameEventsFactory
    {
        GameEvents HandleEvents(List<Event> events, List<GamePlayState> gamePlayStates, string teamId, int round);
    }
}