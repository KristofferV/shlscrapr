using System.Collections.Generic;
using System.Linq;
using shlscrapr.Models;

namespace shlscrapr.Processors
{
    public static class PlayStateListExtensions
    {
        public static GamePlayState StateThisSecond(this IList<GamePlayState> gamePlayState, int eventStartTime)
        {
            return gamePlayState.FirstOrDefault(p => p.StartTime <= eventStartTime && p.EndTime > eventStartTime) ?? gamePlayState.Last();
        }
    }
}