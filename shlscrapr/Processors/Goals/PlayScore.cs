using shlscrapr.Models;

namespace shlscrapr.Processors.Goals
{
    public class PlayScore
    {
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }

        public string CurrentScore { get { return string.Format("{0}-{1}", HomeTeamGoals, AwayTeamGoals); } }

        public void AddGoal(bool homeTeam)
        {
            if (homeTeam)
                HomeTeamGoals++;
            else
                AwayTeamGoals++;
        }

        public TeamAdvantage HomeTeamAdvantage { 
            get
            {
                var goalDiff = HomeTeamGoals - AwayTeamGoals;
                if (goalDiff > 2) return TeamAdvantage.ThreeUpPlus;
                if (goalDiff == 2) return TeamAdvantage.TwoUp;
                if (goalDiff == 1) return TeamAdvantage.OneUp;
                if (goalDiff == -1) return TeamAdvantage.OneDown;
                if (goalDiff == -2) return TeamAdvantage.TwoDown;
                if (goalDiff < -2) return TeamAdvantage.ThreeDownPlus;
                return TeamAdvantage.Even;
            }  
        }
    }
}