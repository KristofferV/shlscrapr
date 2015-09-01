using System.Collections.Generic;
using System.Linq;
using shlscrapr.Models;

namespace shlscrapr.Processors
{
    public static class EventListExtensions
    {
        public static List<IGrouping<string, Event>> TeamEvents(this IList<Event> events)
        {
            return events.Where(e => e.Team != "SHL" && e.Class != "Period").GroupBy(e => e.Team).ToList();
        }
    }
}