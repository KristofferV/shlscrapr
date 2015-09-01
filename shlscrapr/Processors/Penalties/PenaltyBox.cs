using System.Collections.Generic;
using System.Linq;
using shlscrapr.Models;

namespace shlscrapr.Processors.Penalties
{
    public class PenaltyBox
    {
        //TODO Refactor this!!

        private readonly int _gameId;
        private readonly List<PlayEvent> _penalties = new List<PlayEvent>();
        private readonly List<PlayEvent> _expiredPenalties = new List<PlayEvent>();
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
            _expiredPenalties.AddRange(playEvents);
            CalculatePlayersOnIce(second);
        }

        public IEnumerable<PlayEvent> PenaltiesThatExpireThisSecond(int second)
        {
            return _penalties.WithoutKvittningar().Where(p => p.EndTime == second);
        }

        public void HandlePowerPlayGoal(PlayEvent powerPlayGoal)
        {
            var penaltyThatExpiresForPowerPlayGoal = GetPenaltyThatExpiresForPowerPlayGoal(powerPlayGoal);
            if (penaltyThatExpiresForPowerPlayGoal == null)
            {
                //Logger.InfoFormat("{1} No expiring Penalty for PP Second {0}", powerPlayGoal.StartTime.ToClockTime(), _gameId);
                return;
            }

            //Logger.InfoFormat("{4} Penalty for PP Second {0} => S {1} E {2} D {3}", powerPlayGoal.StartTime.ToClockTime(), penaltyThatExpiresForPowerPlayGoal.StartTime.ToClockTime(), penaltyThatExpiresForPowerPlayGoal.EndTime.ToClockTime(), penaltyThatExpiresForPowerPlayGoal.Description, _gameId);
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
            get
            {
                return _playersOnIce; 
            } 
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

    public static class PenaltyExtensions
    {
        public static void CalculateKvittningar(this IEnumerable<PlayEvent> penalties, PlayersOnIce playersOnIce)
        {
            var penaltyList = penalties.ToList();

            var homePenalties = penaltyList.Where(p => p.HomeTeam && !p.PenaltyIsMisconduct).ToList();
            var awayPenalties = penaltyList.Where(p => !p.HomeTeam && !p.PenaltyIsMisconduct).ToList();

            if (IsExceptionToRule(playersOnIce, homePenalties, awayPenalties))
                return;

            foreach (var homePenalty in homePenalties)
            {
                var awayPenaltyThatShouldKvittas = awayPenalties.WithoutKvittningar().FirstOrDefault(p => p.StartTime == homePenalty.StartTime && p.PenaltyTime == homePenalty.PenaltyTime);
                if (awayPenaltyThatShouldKvittas == null)
                    continue;

                homePenalty.IsKvittad = true;
                awayPenaltyThatShouldKvittas.IsKvittad = true;
            }

            //Handle extra minors if original penalty is kvittad they should get the original startime
            foreach (var penalty in penaltyList.WithoutKvittningar().Where(p => p.HasOriginalPenalty))
            {
                penalty.HandleOriginalPenaltyIsKvittad();
            }
        }

        public static void HandleOriginalPenaltyIsKvittad(this PlayEvent penalty)
        {
            if (!penalty.PenaltyIsMinor || !penalty.OriginalPenalty.PenaltyIsMinor || !penalty.OriginalPenalty.IsKvittad) return;

            penalty.StartTime = penalty.OriginalPenalty.StartTime;
            penalty.EndTime = penalty.OriginalPenalty.EndTime;
        }

        private static bool IsExceptionToRule(PlayersOnIce playersOnIce, IEnumerable<PlayEvent> homePenalties, IEnumerable<PlayEvent> awayPenalties)
        {
            return playersOnIce == PlayersOnIce.FiveOnFive 
                                    && homePenalties.Count(p => p.PenaltyIsMinor) == 1
                                    && awayPenalties.Count(p => p.PenaltyIsMinor) == 1;
        }

        public static IEnumerable<PlayEvent> WithoutKvittningar(this IEnumerable<PlayEvent> penalties)
        {
            return penalties.Where(p => !p.IsKvittad);
        }

        public static IEnumerable<PlayEvent> SplitIntoMultiplePenalties(this IList<PlayEvent> penalties)
        {
            var penaltyList = new List<PlayEvent>();

            penaltyList.AddRange(penalties.Where(p => !p.PenaltyIsDouble));

            foreach (var playEvent in penalties.Where(p => p.PenaltyIsDouble))
            {
                var extraPenalty = new PlayEvent(playEvent);
                if (playEvent.Description.Contains("2 + 2"))
                {
                    var endTime = playEvent.StartTime + 120;
                    playEvent.EndTime = endTime;
                    extraPenalty.StartTime = endTime;
                }
                if (playEvent.Description.Contains("2 + 10"))
                {
                    var endTime = playEvent.StartTime + 120;
                    playEvent.EndTime = endTime;
                    extraPenalty.StartTime = endTime;
                    extraPenalty.EndTime = extraPenalty.StartTime + 600;
                }
                if (playEvent.Description.Contains("5 + GM"))
                {
                    var endTime = playEvent.StartTime + 300;
                    playEvent.EndTime = endTime;
                    extraPenalty.StartTime = endTime;
                    extraPenalty.EndTime = extraPenalty.StartTime + 1200;
                }
                penaltyList.Add(playEvent);
                penaltyList.Add(extraPenalty);
            }

            return penaltyList;
        }
    }

    public class PenaltyScoreBoard
    {
        private readonly IEnumerable<ScoreBoardPenalty> _homePenalties;
        private readonly IEnumerable<ScoreBoardPenalty> _awayPenalties;

        public PenaltyScoreBoard(IEnumerable<PlayEvent> penalties)
        {
            var ps = penalties.WithoutKvittningar().OrderBy(p => p.PenaltyTime);
            _homePenalties = ps.Where(p => p.HomeTeam = true).Select(p => new ScoreBoardPenalty(p));
            _awayPenalties = ps.Where(p => p.HomeTeam = false).Select(p => new ScoreBoardPenalty(p));
        }

        public IEnumerable<ScoreBoardPenalty> Home { get { return _homePenalties; } } 
        public IEnumerable<ScoreBoardPenalty> Away { get { return _awayPenalties; } } 
    }

    public class ScoreBoardPenalty
    {
        private readonly PlayEvent _penalty;

        public ScoreBoardPenalty(PlayEvent penalty)
        {
            _penalty = penalty;
        }

        public int StartTime { get { return _penalty.StartTime; } }
        public int EndTime { get { return _penalty.EndTime; } }
        public string Description { get { return _penalty.Description; } }
    }
}