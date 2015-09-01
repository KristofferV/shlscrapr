using System.Collections.Generic;
using System.Linq;
using shlscrapr.Infrastructure;
using shlscrapr.Models;
using shlscrapr.Processors.Goals;
using shlscrapr.Processors.Penalties;

namespace shlscrapr.Processors
{
    public static class GamePlayFactory
    {
        //TODO Refactor this!!

        public static GamePlays HandleGame(IList<Event> events, string homeTeam)
        {
            var gamePlays = new List<GamePlay>();

            var gameId = events.First().GameId;

            var goals = GoalFactory.Create(events, homeTeam);

            var penalties = PenaltiesFactory.Create(events, homeTeam);

            var goalsAndPenalties = goals;

            goalsAndPenalties.AddRange(penalties);

            var homeTeamAdvantage = TeamAdvantage.Even;
            var playScore = new PlayScore();
            var penaltyBox = new PenaltyBox(gameId);

            var lastEvent = goalsAndPenalties.OrderBy(e => e.EndTime).Last();

            var lastEndTime = 0;
            for (var second = 0; second <= lastEvent.EndTime; second++)
            {
                var eventsThatStartThisSecond = goalsAndPenalties.Where(p => p.StartTime == second).ToList();
                var penaltiesThatExpireThisSecond = penaltyBox.PenaltiesThatExpireThisSecond(second).ToList();

                if (eventsThatStartThisSecond.Any() || penaltiesThatExpireThisSecond.Any())
                {
                    gamePlays.Add(new GamePlay
                    {
                        GameId = gameId,
                        StartTime = lastEndTime,
                        EndTime = second,
                        PlayersOnIce = penaltyBox.PlayersOnIce,
                        HomeTeamAdvantage = homeTeamAdvantage,
                    });
                    lastEndTime = second;
                }
                
                foreach (var playEvent in eventsThatStartThisSecond.Where(p => p.IsGoal))
                {
                    if (playEvent.IsGoal)
                    {
                        playScore.AddGoal(playEvent.HomeTeam);
                        homeTeamAdvantage = playScore.HomeTeamAdvantage;
                    }

                    Logger.Debug(string.Format("{4} Start Second {0} => S {1} E {2} D {3} PoI {5} Ha {6} Cs {7} Home {8}", second.ToClockTime(), playEvent.StartTime.ToClockTime(), playEvent.EndTime.ToClockTime(), playEvent.Description, gameId, penaltyBox.PlayersOnIce, homeTeamAdvantage, playScore.CurrentScore, playEvent.HomeTeam));

                    if (playEvent.IsPowerPlayGoal)
                    {
                        penaltyBox.HandlePowerPlayGoal(playEvent);
                    }
                }

                var penaltiesThatStartThisSecond = eventsThatStartThisSecond.Where(p => p.IsPenalty).ToList();
                penaltyBox.AddPenalties(penaltiesThatStartThisSecond);
                foreach (var playEvent in penaltiesThatStartThisSecond)
                {
                    Logger.Debug(string.Format("{4} Start Second {0} => S {1} E {2} D {3} PoI {5} Ha {6} Cs {7} Home {8}", second.ToClockTime(), playEvent.StartTime.ToClockTime(), playEvent.EndTime.ToClockTime(), playEvent.Description, gameId, penaltyBox.PlayersOnIce, homeTeamAdvantage, playScore.CurrentScore, playEvent.HomeTeam));
                }

                penaltyBox.ExpirePenalties(penaltiesThatExpireThisSecond);
                foreach (var playEvent in penaltiesThatExpireThisSecond)
                {
                    Logger.Debug(string.Format("{4} End Second {0} => S {1} E {2} D {3} PoI {5} Ha {6} Cs {7} Home {8}", second.ToClockTime(), playEvent.StartTime.ToClockTime(), playEvent.EndTime.ToClockTime(), playEvent.Description, gameId, penaltyBox.PlayersOnIce, homeTeamAdvantage, playScore.CurrentScore, playEvent.HomeTeam));
                }
            }

            Logger.Debug(string.Format("END GAME {0}", gameId));
            Logger.Debug("");

            return new GamePlays {Items = gamePlays.OrderBy(g => g.StartTime).ToList()};
        }        
    }
}