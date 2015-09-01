using shlscrapr.Models;

namespace shlscrapr.Infrastructure
{
    public interface IEventRepository
    {
        void WriteToFile(GamePlayStates gamePlayStates, string fileName);
        void WriteToFile(GameEvents gameEvents, string fileName);
    }
}