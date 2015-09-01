using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace shlscrapr.Models
{
    public class GameEvent
    {
        public int GameId { get; set; }
        public int Round { get; set; }
        public int Period { get; set; }
        public string TeamName { get; set; }
        public bool HomeTeam { get; set; }
        public int EventId { get; set; }
        public string GameTime { get; set; }
        public int StartTime { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PlayersOnIce PlayersOnIce { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TeamAdvantage HomeTeamAdvantage { get; set; }

    }
}