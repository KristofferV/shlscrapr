using System.Linq;
using shlscrapr.Importers;
using shlscrapr.Infrastructure;
using shlscrapr.Models;

namespace shlscrapr.Processors
{
    public class GameProcessor
    {
        private readonly IImporterRepository _importerRepository;

        public GameProcessor(IImporterRepository importerRepository)
        {
            _importerRepository = importerRepository;
        }

        public void Process(Season season)
        {
            for (var i = season.StartGame; i < season.LastGame; i++)
            {
                var liveReport = _importerRepository.GetGameReport(season.Id, i);

                if (liveReport == null)
                    continue;

                var report = liveReport.GameReport.games.First();
                var homeTeam = report.homeTeamCode;
                
                var events = _importerRepository.GetEvents(season.Id, i);

                if (events.Any() == false) 
                    continue;
                
                var gamePlays = GamePlayFactory.HandleGame(events, homeTeam);

                //TODO IoC storage
                JsonToFileSerializer<GamePlays>.WriteToFile(gamePlays, Settings.GetGamePlaysDataFileName(season.Id, i));

                var gameEvents = GameEventsFactory.HandleEvents(events, gamePlays.Items, homeTeam, report.round);

                //TODO IoC storage
                JsonToFileSerializer<GameEvents>.WriteToFile(gameEvents, Settings.GetGameEventsDataFileName(season.Id, i));
            }

        }
    }
}
