using System.Collections.Generic;
using shlscrapr.Models;

namespace shlscrapr.Importers
{
    public interface IImporterRepository
    {
        List<Event> GetEvents(int seasonId, int gameId);
        PlayerStats GetPlayerStats(int seasonId, int gameId);
        LiveReport GetGameReport(int seasonId, int gameId);
    }
}