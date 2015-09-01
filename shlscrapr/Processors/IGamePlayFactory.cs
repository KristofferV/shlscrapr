using System.Collections.Generic;
using shlscrapr.Models;

namespace shlscrapr.Processors
{
    public interface IGamePlayFactory
    {
        GamePlayStates HandleGame(IList<Event> events, string homeTeam);
    }
}