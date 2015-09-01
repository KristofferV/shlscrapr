using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using shlscrapr.Infrastructure;
using shlscrapr.Models;

namespace shlscrapr.Importers
{
    public class ImporterRepository : IImporterRepository
    {
        public List<Event> GetEvents(int seasonId, int gameId)
        {
            var filePath = Settings.GetEventsFileName(seasonId, gameId);

            var eventsFileName = filePath;
            if (!File.Exists(eventsFileName))
                return new List<Event>();

            var liveEvent = JsonConvert.DeserializeObject<LiveEvent>(File.ReadAllText(filePath));

            foreach (var gameEvent in liveEvent.Events)
            {
                var test = gameEvent.Extra as JObject;
                if (test == null || test.Count <= 0)
                    continue;

                var extra = JsonConvert.DeserializeObject<Extra>(gameEvent.Extra.ToString());

                gameEvent.ExtraInfo = extra;
            }

            return liveEvent.Events;
        }

        public LiveReport GetGameReport(int seasonId, int gameId)
        {
            var filePath = Settings.GetReportFileName(seasonId, gameId);

            if (!File.Exists(filePath))
                return null;
            
            var gameReport = JsonConvert.DeserializeObject<LiveReport>(File.ReadAllText(filePath));

            return gameReport;
        }

        public PlayerStats GetPlayerStats(int seasonId, int gameId)
        {
            var filePath = Settings.GetPlayerStatsFileName(seasonId, gameId);
            var playerStats = JsonConvert.DeserializeObject<PlayerStats>(File.ReadAllText(filePath));

            return playerStats;
        }
    }
}
