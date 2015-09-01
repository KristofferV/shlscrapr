namespace shlscrapr.Models
{
    public class TeamEventState : GamePlayState
    {
        public bool Close { get; set; }
        public bool Leading { get; set; }
        public bool Trailing { get; set; }
    }
}