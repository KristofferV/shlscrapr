using System.Collections.Generic;
using System.Linq;

namespace shlscrapr.Models
{
    public class GamePlays : BaseModel
    {
        public List<GamePlay> Items { get; set; }
        public override int Id { get { return Items.Any() ? Items.First().GameId : -1; } }
    }

}
