using System.Linq;
using shlscrapr.Importers;
using shlscrapr.Infrastructure;
using shlscrapr.Models;

namespace shlscrapr.Processors
{
    public class GameProcessor
    {
        private readonly IImporterRepository _importerRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IGamePlayFactory _gamePlayFactory;
        private readonly IGameEventsFactory _gameEventsFactory;

        public GameProcessor(IImporterRepository importerRepository, IEventRepository eventRepository, IGamePlayFactory gamePlayFactory, IGameEventsFactory gameEventsFactory)
        {
            _importerRepository = importerRepository;
            _eventRepository = eventRepository;
            _gamePlayFactory = gamePlayFactory;
            _gameEventsFactory = gameEventsFactory;
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

                var gamePlays = _gamePlayFactory.HandleGame(events, homeTeam);

                _eventRepository.WriteToFile(gamePlays, Settings.GetGamePlaysDataFileName(season.Id, i));

                var gameEvents = _gameEventsFactory.HandleEvents(events, gamePlays.Items, homeTeam, report.round);

                _eventRepository.WriteToFile(gameEvents, Settings.GetGameEventsDataFileName(season.Id, i));
            }

        }
    }
}
