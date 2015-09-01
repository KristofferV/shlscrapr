namespace shlscrapr.Infrastructure
{
    public class Settings
    {
        private const string BaseUrl = "http://shl.se/shl-arena/";
        private const string BasePath = @"C:\shlscrapr\games\{0}\{1}\game_{1}_";
        private const string DataPath = @"C:\shlscrapr\games\data\{0}\{1}_game_";

        public const string LiveEventsUrl = BaseUrl + "{0}/liveevents/0/";
        public const string PlayerStatsUrl = BaseUrl + "{0}/liveevents/playerstats";
        public const string LiveReportUrl = BaseUrl + "{0}/game/report/";

        public const string LiveEventsPath = BasePath + "livevents";
        public const string PlayerStatsPath = BasePath + "playerstats";
        public const string LiveReportPath = BasePath + "report";

        private const string GamePlaysData = DataPath + "plays";
        private const string GameEventsData = DataPath + "events";

        public static string GetEventsFileName(int seasonId, int gameId)
        {
            return string.Format(LiveEventsPath, seasonId, gameId);
        }

        public static string GetPlayerStatsFileName(int seasonId, int gameId)
        {
            return string.Format(PlayerStatsPath, seasonId, gameId);
        }

        public static string GetReportFileName(int seasonId, int gameId)
        {
            return string.Format(LiveReportPath, seasonId, gameId);
        }

        public static string GetGamePlaysDataFileName(int seasonId, int gameId)
        {
            return string.Format(GamePlaysData, seasonId, gameId);
        }

        public static string GetGameEventsDataFileName(int seasonId, int gameId)
        {
            return string.Format(GameEventsData, seasonId, gameId);
        }
    }
}