using System.Collections.Generic;
using System.Linq;
using shlscrapr.Infrastructure;
using shlscrapr.Models;

namespace shlscrapr.Processors.Penalties
{
    public class PenaltyBox
    {
        private readonly int _gameId;
        private readonly List<PlayEvent> _penalties = new List<PlayEvent>();
        private PlayersOnIce _playersOnIce = PlayersOnIce.FiveOnFive;

        public PenaltyBox(int gameId)
        {
            _gameId = gameId;
        }

        public void AddPenalties(IList<PlayEvent> playEvents)
        {
            if (!playEvents.Any()) return;
            
            var penalties = playEvents.SplitIntoMultiplePenalties().ToList();
            _penalties.AddRange(penalties);
            CalculateKvittningar(penalties);
            CalculatePlayersOnIce(playEvents.First().StartTime);
        }

        public void ExpirePenalties(List<PlayEvent> playEvents)
        {
            if (!playEvents.Any()) return;

            var second = playEvents.First().EndTime;
            var kvittningarThatExpireThisSecond = _penalties.Where(p => p.IsKvittad && p.EndTime == second).ToList();

            playEvents.AddRange(kvittningarThatExpireThisSecond);
            foreach (var playEvent in playEvents)
            {
                _penalties.Remove(playEvent);
            }
            CalculatePlayersOnIce(second);
        }

        public IList<PlayEvent> PenaltiesThatExpireThisSecond(int second)
        {
            return _penalties.WithoutKvittningar().Where(p => p.EndTime == second).ToList();
        }

        public void HandlePowerPlayGoal(PlayEvent powerPlayGoal)
        {
            var penaltyThatExpiresForPowerPlayGoal = GetPenaltyThatExpiresForPowerPlayGoal(powerPlayGoal);
            if (penaltyThatExpiresForPowerPlayGoal == null)
            {
                Logger.Debug(string.Format("{1} No expiring Penalty for PP Second {0}", powerPlayGoal.StartTime.ToClockTime(), _gameId));
                return;
            }

            Logger.Debug(string.Format("{4} Penalty for PP Second {0} => S {1} E {2} D {3}", powerPlayGoal.StartTime.ToClockTime(), penaltyThatExpiresForPowerPlayGoal.StartTime.ToClockTime(), penaltyThatExpiresForPowerPlayGoal.EndTime.ToClockTime(), penaltyThatExpiresForPowerPlayGoal.Description, _gameId));
            penaltyThatExpiresForPowerPlayGoal.EndTime = powerPlayGoal.StartTime;
            ExpirePenalties(new List<PlayEvent> { penaltyThatExpiresForPowerPlayGoal });
        }

        private PlayEvent GetPenaltyThatExpiresForPowerPlayGoal(PlayEvent powerPlayGoal)
        {
            var penalties = _penalties
                .WithoutKvittningar()
                .OrderBy(p => p.PenaltyTime)
                .Where(p => p.PenaltyIsMinor)
                .Where(p => p.HomeTeam != powerPlayGoal.HomeTeam)
                .Where(p => p.StartTime <= powerPlayGoal.StartTime && p.EndTime >= powerPlayGoal.StartTime);
            return penalties.FirstOrDefault();
        }

        public PlayersOnIce PlayersOnIce 
        { 
            get { return _playersOnIce; } 
        }

        public PenaltyScoreBoard PenaltyScoreBoard
        {
            get { return new PenaltyScoreBoard(_penalties);}
        }

        private void CalculateKvittningar(IEnumerable<PlayEvent> playEvents)
        {
            playEvents.CalculateKvittningar(_playersOnIce);
        }

        private void CalculatePlayersOnIce(int startTime)
        {
            var homePenalties = _penalties.WithoutKvittningar().Count(p => p.HomeTeam && !p.PenaltyIsMisconduct && !p.PenaltyIsGame && p.StartTime <= startTime && p.EndTime > startTime);
            var awayPenalties = _penalties.WithoutKvittningar().Count(p => !p.HomeTeam && !p.PenaltyIsMisconduct && !p.PenaltyIsGame && p.StartTime <= startTime && p.EndTime > startTime);

            _playersOnIce = PlayersOnIce.Unknown;

            if (homePenalties == 0 && awayPenalties == 0)
                _playersOnIce = PlayersOnIce.FiveOnFive;
            if (homePenalties == 0 && awayPenalties == 1)
                _playersOnIce = PlayersOnIce.FiveOnFour;
            if (homePenalties == 0 && awayPenalties >= 2)
                _playersOnIce = PlayersOnIce.FiveOnThree;
            if (homePenalties == 1 && awayPenalties >= 2)
                _playersOnIce = PlayersOnIce.FourOnThree;
            if (homePenalties == 1 && awayPenalties == 0)
                _playersOnIce = PlayersOnIce.FourOnFive;
            if (homePenalties >= 2 && awayPenalties == 0)
                _playersOnIce = PlayersOnIce.ThreeOnFive;
            if (homePenalties >= 2 && awayPenalties == 1)
                _playersOnIce = PlayersOnIce.ThreeOnFour;
            if (homePenalties == 1 && awayPenalties == 1)
                _playersOnIce = PlayersOnIce.FourOnFour;
            if (homePenalties >= 2 && awayPenalties >= 2)
                _playersOnIce = PlayersOnIce.ThreeOnThree;                
        }
    }
}