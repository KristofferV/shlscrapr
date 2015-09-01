namespace shlscrapr.Models
{
    public class PlayEvent
    {
        private readonly PlayEvent _originalPenalty;

        public PlayEvent()
        {
            
        }

        public PlayEvent(PlayEvent originalPenalty)
        {
            _originalPenalty = originalPenalty;
            Class = originalPenalty.Class;
            Description = "Extra " + originalPenalty.Description;
            StartTime = originalPenalty.StartTime;
            EndTime = originalPenalty.EndTime;
            HomeTeam = originalPenalty.HomeTeam;
        }

        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public string Class { get; set; }
        public string Description { get; set; }
        public bool HomeTeam { get; set; }
        public bool IsKvittad { get; set; }
        public bool IsGoal { get { return Class == "Goal"; } }
        public bool IsPowerPlayGoal { get { return IsGoal && Description.Contains("PP") ; } }
        public bool IsPenalty { get { return Class == "Penalty"; } }
        public int PenaltyTime { get { return IsPenalty ? EndTime-StartTime : 0; } }
        //public bool PenaltyHasMinutes { get { return IsPenalty && PenaltyTime > 0; } }
        public bool PenaltyIsMajor { get { return IsPenalty && PenaltyTime == 300; } }
        public bool PenaltyIsMisconduct { get { return IsPenalty && PenaltyTime == 600; } }
        public bool PenaltyIsGame { get { return IsPenalty && PenaltyTime == 1200; } }
        public bool PenaltyIsMinor { get { return !PenaltyIsMajor && !PenaltyIsMisconduct && !PenaltyIsGame && IsPenalty; } }
        public bool PenaltyIsDouble { get { return IsPenalty && Description.Contains("+"); } }
        public bool HasOriginalPenalty { get { return _originalPenalty != null; } }
        public PlayEvent OriginalPenalty { get { return _originalPenalty; } }
    }
}