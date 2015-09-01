using System.Collections.Generic;
using System.Linq;
using shlscrapr.Infrastructure;
using shlscrapr.Models;
using shlscrapr.Processors.Goals;
using shlscrapr.Processors.Penalties;

namespace shlscrapr.Processors
{
    public class GamePlayFactory : IGamePlayFactory
    {
        private List<GamePlayState> _gamePlayStates;
        private int _gameId;
        private TeamAdvantage _homeTeamAdvantage;
        private ScoreBoard _scoreBoard;
        private PenaltyBox _penaltyBox;

        private void Init(IEnumerable<Event> events)
        {
            _gamePlayStates = new List<GamePlayState>();

            _gameId = events.First().GameId;

            _homeTeamAdvantage = TeamAdvantage.Even;
            _scoreBoard = new ScoreBoard();
            _penaltyBox = new PenaltyBox(_gameId);
        }


        public GamePlayStates HandleGame(IList<Event> events, string homeTeam)
        {
            Init(events);
            
            var goalsAndPenalties = GetGoalsAndPenalties(events, homeTeam);
            var endOfGame = goalsAndPenalties.OrderBy(e => e.EndTime).Last().EndTime;
            var lastEndTime = 0;

            for (var second = 0; second <= endOfGame; second++)
            {
                var eventsThatStartThisSecond = goalsAndPenalties.EventsThatStartThisSecond(second);
                var penaltiesThatExpireThisSecond = _penaltyBox.PenaltiesThatExpireThisSecond(second);

                if (eventsThatStartThisSecond.Any() || penaltiesThatExpireThisSecond.Any())
                {
                    _gamePlayStates.Add(new GamePlayState
                    {
                        GameId = _gameId,
                        StartTime = lastEndTime,
                        EndTime = second,
                        PlayersOnIce = _penaltyBox.PlayersOnIce,
                        HomeTeamAdvantage = _homeTeamAdvantage,
                    });
                    lastEndTime = second;
                }
                
                HandleGoalsThisSecond(eventsThatStartThisSecond, second);

                HandlePenaltiesThisSecond(eventsThatStartThisSecond, second);
            }

            Logger.Debug(string.Format("END GAME {0}", _gameId));
            Logger.Debug("");

            return new GamePlayStates { Items = _gamePlayStates.OrderBy(g => g.StartTime).ToList() };
        }

        private static List<PlayEvent> GetGoalsAndPenalties(IList<Event> events, string homeTeam)
        {
            var goals = GoalFactory.Create(events, homeTeam);
            var penalties = PenaltiesFactory.Create(events, homeTeam);
            var goalsAndPenalties = goals;
            goalsAndPenalties.AddRange(penalties);
            return goalsAndPenalties;
        }

        private void HandlePenaltiesThisSecond(IList<PlayEvent> eventsThatStartThisSecond, int second)
        {
            var penaltiesThatStartThisSecond = eventsThatStartThisSecond.Penalties();
            _penaltyBox.AddPenalties(penaltiesThatStartThisSecond);

            var penaltiesThatExpireThisSecond = _penaltyBox.PenaltiesThatExpireThisSecond(second);
            _penaltyBox.ExpirePenalties(penaltiesThatExpireThisSecond.ToList());

            //Logging
            foreach (var playEvent in penaltiesThatStartThisSecond)
            {
                Log(playEvent, second);
            } 
            foreach (var playEvent in penaltiesThatExpireThisSecond)
            {
                Log(playEvent, second);
            }
        }

        private void HandleGoalsThisSecond(IList<PlayEvent> eventsThatStartThisSecond, int second)
        {
            foreach (var playEvent in eventsThatStartThisSecond.Goals())
            {
                if (playEvent.IsGoal)
                {
                    _scoreBoard.AddGoal(playEvent.HomeTeam);
                    _homeTeamAdvantage = _scoreBoard.HomeTeamAdvantage;
                }

                Log(playEvent, second);

                if (playEvent.IsPowerPlayGoal)
                {
                    _penaltyBox.HandlePowerPlayGoal(playEvent);
                }
            }
        }
        private void Log(PlayEvent playEvent, int second)
        {
            Logger.Debug(string.Format("{4} Start Second {0} => S {1} E {2} D {3} PoI {5} Ha {6} Cs {7} Home {8}",
                second.ToClockTime(), playEvent.StartTime.ToClockTime(), playEvent.EndTime.ToClockTime(),
                playEvent.Description, _gameId, _penaltyBox.PlayersOnIce, _homeTeamAdvantage, _scoreBoard.CurrentScore,
                playEvent.HomeTeam));
        }
    }
}