using System.Collections.Generic;
using shlscrapr.Infrastructure;
using shlscrapr.Models;

namespace shlscrapr.Importers
{
    public class Importer
    {
        public static IEnumerable<IImporter> Importers
        {
            get
            {
                var importers = new List<IImporter>
                {
                    new ShlImporter<LiveEvent>(Settings.LiveEventsUrl, Settings.LiveEventsPath),
                    new ShlImporter<PlayerStats>(Settings.PlayerStatsUrl, Settings.PlayerStatsPath),
                    new ShlImporter<LiveReport>(Settings.LiveReportUrl, Settings.LiveReportPath),
                };
                return importers;
            }
        }
    }
}

