using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using shlscrapr.Infrastructure;
using shlscrapr.Models;

namespace shlscrapr.Importers
{
    public class ShlImporter<T> : IImporter where T : BaseModel
    {
        private readonly string _urlPattern;
        private readonly string _fileNamePattern;
        private readonly bool _downloadIfExists;

        public ShlImporter(string urlPattern, string fileNamePattern, bool downloadIfExists = false)
        {
            _urlPattern = urlPattern;
            _fileNamePattern = fileNamePattern;
            _downloadIfExists = downloadIfExists;
        }

        public void ImportAll()
        {
            var description = typeof(T).Name;

            Parallel.ForEach(Season.Seasons, (season) =>
            {
                var watch = Stopwatch.StartNew();

                Logger.Info(string.Format("Start {0} {1}", description, season.Name));
                var agent = new ApiAgent<T>(_urlPattern);

                for (var i = season.StartGame; i < season.LastGame; i++)
                {
                    try
                    {
                        var fileName = string.Format(_fileNamePattern, season.Id, i);

                        if (!_downloadIfExists && File.Exists(fileName))
                            continue;

                        var model = agent.GetModel(i);

                        if (model.Id < 1)
                            continue;

                        JsonToFileSerializer<T>.WriteToFile(model, fileName);
                        Logger.Debug(string.Format("Imported game {0} to {1}", i, fileName));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(string.Format("Error importing game {0} in season {1}", i, season.Name), ex);
                    }
                }

                Logger.Info(string.Format("[Success] Finished {0} {1} in {2}", description, season.Name, watch.Elapsed.ToString()));
            });
        }
    }
}