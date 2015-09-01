using shlscrapr.Models;

namespace shlscrapr.tests
{
    public class PenaltyCreator
    {
        private const int DefaultStartTime = 100;

        public static PlayEvent CreateMinor(bool homeTeam, int startTime = DefaultStartTime)
        {
            return new PlayEvent { StartTime = startTime, EndTime = startTime + 120, HomeTeam = homeTeam, Description = "2min Minor", Class = "Penalty" };
        }

        public static PlayEvent CreateDoubleMinor(bool homeTeam, int startTime = DefaultStartTime)
        {
            return new PlayEvent { StartTime = startTime, EndTime = startTime + 240, HomeTeam = homeTeam, Description = "2 + 2min Minor", Class = "Penalty" };
        }

        public static PlayEvent CreateMinorAndMisconduct(bool homeTeam, int startTime = DefaultStartTime)
        {
            return new PlayEvent { StartTime = startTime, EndTime = startTime + 120, HomeTeam = homeTeam, Description = "2 + 10min Minor", Class = "Penalty" };
        }

        public static PlayEvent CreateMajor(bool homeTeam, int startTime = DefaultStartTime)
        {
            return new PlayEvent { StartTime = startTime, EndTime = startTime + 300, HomeTeam = homeTeam, Description = "5min Major", Class = "Penalty" };
        }

        public static PlayEvent CreateMajorAndGame(bool homeTeam, int startTime = DefaultStartTime)
        {
            return new PlayEvent { StartTime = startTime, EndTime = startTime + 300, HomeTeam = homeTeam, Description = "5 + GM Major", Class = "Penalty" };
        }
    }
}
