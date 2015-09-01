using System.Collections.Generic;
using System.Linq;

namespace shlscrapr.Models
{
    public abstract class BaseModel
    {
        public abstract int Id { get; }
    }

    public class LiveEvent : BaseModel
    {
        public List<Event> Events { get; set; }
        public override int Id { get { return Events.Any() ? Events.First().GameId : -1; } }
    }

    //public class Event
    //{
    //    public int eventId { get; set; }
    //    public int revision { get; set; }
    //    public string hash { get; set; }
    //    public object channel { get; set; }
    //    public string gametime { get; set; }
    //    public int timePeriod { get; set; }
    //    public int gameId { get; set; }
    //    public string realTime { get; set; }
    //    public string time { get; set; }
    //    public int period { get; set; }
    //    public string _class { get; set; }
    //    public string type { get; set; }
    //    public string description { get; set; }
    //    public object extra { get; set; }
    //    public string action { get; set; }
    //    public string source { get; set; }
    //    public string sourceport { get; set; }
    //    public string team { get; set; }
    //    public string messagetype { get; set; }
    //    public string actiontype { get; set; }
    //    public string status { get; set; }
    //    public Location location { get; set; }
    //    public Extra ExtraInfo { get; set; }

    //}

    //public class Location
    //{
    //    public int x { get; set; }
    //    public int y { get; set; }
    //}

    public class Event
    {
        public int EventId { get; set; }
        public int GameId { get; set; }
        public string GameTime { get; set; }
        public int TimePeriod { get; set; }
        public long RealTime { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Team { get; set; }
        public Location Location { get; set; }
        public object Extra { get; set; }
        public string Time { get; set; }
        public int Period { get; set; }
        public Extra ExtraInfo { get; set; }

        //Type : Period
        //Type : Livefeed
        //Type : Målvakt in
        //Type : Skott
        //Type : Utvisning
        //Type : 1-0
        //Type : 2-0
        //Type : 3-0
        //Type : 4-0
        //Type : Timeout
        //Type : 5-0
        //Type : Målvakt ut
        //Type : 6-0
        //Type : 6-1
        //Type : 6-2
        //Type : Ramträff

        //Class : Period
        //Class : Livefeed
        //Class : GoolkeeperEvent
        //Class : ShotWide
        //Class : ShotBlocked
        //Class : Shot
        //Class : Penalty
        //Class : Goal
        //Class : Timeout
        //Class : ShotIron

        //ClassAndType : Goal-1-0
        //ClassAndType : Goal-2-0
        //ClassAndType : Goal-3-0
        //ClassAndType : Goal-4-0
        //ClassAndType : Goal-5-0
        //ClassAndType : Goal-6-0
        //ClassAndType : Goal-6-1
        //ClassAndType : Goal-6-2
        //ClassAndType : GoolkeeperEvent-Målvakt in
        //ClassAndType : GoolkeeperEvent-Målvakt ut
        //ClassAndType : Livefeed-Livefeed
        //ClassAndType : Penalty-Utvisning
        //ClassAndType : Period-Period
        //ClassAndType : Shot-Skott
        //ClassAndType : ShotBlocked-Skott
        //ClassAndType : ShotIron-Ramträff
        //ClassAndType : ShotWide-Skott
        //ClassAndType : Timeout-Timeout

    }

    public class Extra
    {
        public string GameStatus { get; set; }
        public string Pop { get; set; }
        public string Nep { get; set; }
        public string Assist { get; set; }
        public int HomeForward { get; set; }
        public int HomeAgainst { get; set; }
        public string TeamAdvantage { get; set; }
        public string ScorerLong { get; set; }
        public string AssistOneLong { get; set; }
        public string AssistTwoLong { get; set; }

        //"pop" : "POP: 4, 17, 19, 35, 37, 55",
        //"nep" : "NEP: 11, 23, 30, 38, 41, 51",
        //"assist" : "37 J Norman, 19 A Calof",
        //"homeForward" : 1,
        //"homeAgainst" : 0,
        //"teamAdvantage" : "EQ",
        //"scorerLong" : "17 P\u00e4r Lindholm",
        //"assistOneLong" : "37 John Norman",
        //"assistTwoLong" : "19 Andrew Calof"
    }

    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class PlayerStats : BaseModel
    {
        public Playerstatistic[] PlayerStatistics { get; set; }
        public Recap Recap { get; set; }

        public override int Id { get { return PlayerStatistics.Any() ? PlayerStatistics.First().game : -1; } }
    }

    public class Recap
    {
        public Total Total { get; set; }
        public Period[] Periods { get; set; }
    }

    public class Total
    {
        public int periodNumber { get; set; }
        public int gameStatsId { get; set; }
        public string status { get; set; }
        public string awayTeamId { get; set; }
        public string awayTeamName { get; set; }
        public int awayG { get; set; }
        public int awayPIM { get; set; }
        public int awayFOW { get; set; }
        public int awaySOG { get; set; }
        public int awaySPG { get; set; }
        public int awaySaves { get; set; }
        public int awayGA { get; set; }
        public float awaySavesPerShot { get; set; }
        public float awayPP_perc { get; set; }
        public float awaySH_perc { get; set; }
        public int awayPPG { get; set; }
        public int awaySHG { get; set; }
        public int awayPPGA { get; set; }
        public int awaySHGA { get; set; }
        public int awayNumPP { get; set; }
        public int awayNumSH { get; set; }
        public int awayHits { get; set; }
        public int awayPPSOG { get; set; }
        public string awayactiontype { get; set; }
        public string startDateTime { get; set; }
        public string endDateTime { get; set; }
        public string statusString { get; set; }
        public int statsId { get; set; }
        public int round { get; set; }
        public int attendance { get; set; }
        public string homeTeamId { get; set; }
        public string homeTeamName { get; set; }
        public int homeG { get; set; }
        public int homePIM { get; set; }
        public int homeFOW { get; set; }
        public int homeSOG { get; set; }
        public int homeSPG { get; set; }
        public int homeSaves { get; set; }
        public int homeGA { get; set; }
        public float homeSavesPerShot { get; set; }
        public float homePP_perc { get; set; }
        public float homeSH_perc { get; set; }
        public int homePPG { get; set; }
        public int homeSHG { get; set; }
        public int homePPGA { get; set; }
        public int homeSHGA { get; set; }
        public int homeNumPP { get; set; }
        public int homeNumSH { get; set; }
        public int homeHits { get; set; }
        public int homePPSOG { get; set; }
        public string homeactiontype { get; set; }

        public string FormatString()
        {
            return string.Format("Game {10} {0} vs {1} {2}-{3} H-Shots {4} PP {5} A-Shots {6} PP {7} SVP {8:0.00} {9:0.00}", homeTeamId, awayTeamId, homeG, awayG, homeSOG, homePPSOG, awaySOG, awayPPSOG, homeSavesPerShot * 100, awaySavesPerShot * 100, gameStatsId);
        }
    }

    public class Period
    {
        public int periodNumber { get; set; }
        public int gameStatsId { get; set; }
        public string status { get; set; }
        public string awayTeamId { get; set; }
        public string awayTeamName { get; set; }
        public int awayG { get; set; }
        public int awayPIM { get; set; }
        public int awayFOW { get; set; }
        public int awaySOG { get; set; }
        public int awaySPG { get; set; }
        public int awaySaves { get; set; }
        public int awayGA { get; set; }
        public float awaySavesPerShot { get; set; }
        public float awayPP_perc { get; set; }
        public float awaySH_perc { get; set; }
        public int awayPPG { get; set; }
        public int awaySHG { get; set; }
        public int awayPPGA { get; set; }
        public int awaySHGA { get; set; }
        public int awayNumPP { get; set; }
        public int awayNumSH { get; set; }
        public int awayHits { get; set; }
        public int awayPPSOG { get; set; }
        public string awayactiontype { get; set; }
        public string startDateTime { get; set; }
        public string endDateTime { get; set; }
        public string statusString { get; set; }
        public string homeTeamId { get; set; }
        public string homeTeamName { get; set; }
        public int homeG { get; set; }
        public int homePIM { get; set; }
        public int homeFOW { get; set; }
        public int homeSOG { get; set; }
        public int homeSPG { get; set; }
        public int homeSaves { get; set; }
        public int homeGA { get; set; }
        public float homeSavesPerShot { get; set; }
        public float homePP_perc { get; set; }
        public float homeSH_perc { get; set; }
        public int homePPG { get; set; }
        public int homeSHG { get; set; }
        public int homePPGA { get; set; }
        public int homeSHGA { get; set; }
        public int homeNumPP { get; set; }
        public int homeNumSH { get; set; }
        public int homeHits { get; set; }
        public int homePPSOG { get; set; }
        public string homeactiontype { get; set; }
    }

    public class Playerstatistic
    {
        public int player { get; set; }
        public int game { get; set; }
        public string team { get; set; }
        public int jersey { get; set; }
        public int line { get; set; }
        public string position { get; set; }
        public string firstName { get; set; }
        public string familyName { get; set; }
        public int pim { get; set; }
        public string actiontype { get; set; }
        public int tot_ga { get; set; }
        public int tot_soga { get; set; }
        public int tot_spga { get; set; }
        public int tot_svs { get; set; }
        public float tot_svs_perc { get; set; }
        public float tot_nonso_svs_perc { get; set; }
        public int hits { get; set; }
        public int g { get; set; }
        public int a { get; set; }
        public string toi { get; set; }
        public int shg { get; set; }
        public int ppg { get; set; }
        public float fo_perc { get; set; }
        public int sw { get; set; }
        public int sog { get; set; }
        public int ppsog { get; set; }
        public int nep { get; set; }
        public int pop { get; set; }
        public int netPlusMinus { get; set; }
        public int tp { get; set; }
        public int fo_tot { get; set; }
        public int fow { get; set; }
        public int fol { get; set; }
    }
    
    public class LiveReport : BaseModel
    {
        public Gamereport GameReport { get; set; }

        public override int Id { get { return GameReport.games.Any() ? GameReport.games.First().gameId : -1; } }
    }

    public class Gamereport
    {
        public string action { get; set; }
        public string hash { get; set; }
        public string channel { get; set; }
        public string source { get; set; }
        public string sourceport { get; set; }
        public string team { get; set; }
        public string messagetype { get; set; }
        public string description { get; set; }
        public string actiontype { get; set; }
        public string Class { get; set; }
        public string type { get; set; }
        public Game[] games { get; set; }
    }

    public class Game
    {
        public int revision { get; set; }
        public string gametime { get; set; }
        public int timePeriod { get; set; }
        public int gameId { get; set; }
        public int isaId { get; set; }
        public string time { get; set; }
        public int period { get; set; }
        public int round { get; set; }
        public string homeTeamCode { get; set; }
        public int homeScore { get; set; }
        public string awayTeamCode { get; set; }
        public int awayScore { get; set; }
        public string arena { get; set; }
        public int attendance { get; set; }
        public string gameState { get; set; }
        public string status { get; set; }
        public string gameType { get; set; }
        public string statusString { get; set; }
        public int updateStatus { get; set; }
        public string messagetype { get; set; }
        public string statusStringShort { get; set; }

        public string FormatString()
        {
            return string.Format("Game {8} {0} vs {1} {2}-{3} {4} {5} {6} {7}", homeTeamCode, awayTeamCode, homeScore, awayScore, gameType, arena, attendance, round, gameId);
        }
    }

}
