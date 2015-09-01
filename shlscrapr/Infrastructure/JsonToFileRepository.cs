using shlscrapr.Models;

namespace shlscrapr.Infrastructure
{
    public class JsonToFileRepository : IEventRepository
    {
        public void WriteToFile(GamePlayStates gamePlayStates, string fileName)
        {
            JsonToFileSerializer<GamePlayStates>.WriteToFile(gamePlayStates, fileName);
        }

        public void WriteToFile(GameEvents gameEvents, string fileName)
        {
            JsonToFileSerializer<GameEvents>.WriteToFile(gameEvents, fileName);
        }
    }
}