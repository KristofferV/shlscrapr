using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace shlscrapr.Models
{
    public class GamePlayState
    {
        public int GameId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PlayersOnIce PlayersOnIce { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TeamAdvantage HomeTeamAdvantage { get; set; }
    }
}