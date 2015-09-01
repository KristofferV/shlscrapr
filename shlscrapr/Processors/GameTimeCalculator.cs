using System;

namespace shlscrapr.Processors
{
    public static class GameTimeCalculator
    {
        public static int Calculate(int period, int timePeriod)
        {
            return ((period - 1)*1200) + timePeriod;
        }

        public static string Format(int gameTime)
        {
            return TimeSpan.FromSeconds(gameTime).ToString(@"mm\:ss");
        }

        public static string FormatMinutes(int gameTime)
        {
            return TimeSpan.FromSeconds(gameTime).TotalMinutes.ToString("0:0.00");
        }
    }
}