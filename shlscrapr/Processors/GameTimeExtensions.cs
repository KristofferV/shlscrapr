namespace shlscrapr.Processors
{
    public static class GameTimeExtensions
    {
        public static string ToClockTime(this int time)
        {
            return GameTimeCalculator.Format(time);
        }

        public static string ToLongClockTime(this int time)
        {
            return GameTimeCalculator.FormatMinutes(time);
        }
    }
}
