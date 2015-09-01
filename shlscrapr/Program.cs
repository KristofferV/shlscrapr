using System.Threading.Tasks;
using shlscrapr.Importers;
using shlscrapr.Infrastructure;
using shlscrapr.Models;
using shlscrapr.Processors;
using SimpleInjector;

namespace shlscrapr
{
    class Program
    {
        static void Main(string[] args)
        {
            //Init logging
            Logger.Init("shlscrapr");

            //IoC
            var container = Bootstrap();

            var processor = container.GetInstance<GameProcessor>();

            //Download all games
            Parallel.ForEach(Importer.Importers, importer =>
            {
                importer.ImportAll();
            });
            
            //Process all games to game events

            //Parallel.ForEach(Season.Seasons, season =>
            //{
            //    processor.Process(season);
            //});

            foreach (var season in Season.Seasons)
            {
                processor.Process(season);
            }

            //Done
        }

        private static Container Bootstrap()
        {
            var container = new Container();

            container.Register<IImporterRepository, ImporterRepository>();
            container.Register<GameProcessor>();
            container.Register<IEventRepository, JsonToFileRepository>();
            container.Register<IGamePlayFactory, GamePlayFactory>();
            container.Register<IGameEventsFactory, GameEventsFactory>();

            container.Verify();
            return container;
        }
    }
}

/*
 * TODOS
 * Refactor some classes
 * Rename models to better names
 * Add extras to events if exists
 * 
*/