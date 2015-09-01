using System.Threading.Tasks;
using shlscrapr.Importers;
using shlscrapr.Infrastructure;
using shlscrapr.Models;
using shlscrapr.Processors;

namespace shlscrapr
{
    class Program
    {
        static void Main(string[] args)
        {
            //Init logging
            Logger.Init("shlscrapr");

            //IoC
            var processor = new GameProcessor(new ImporterRepository());

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
    }
}
