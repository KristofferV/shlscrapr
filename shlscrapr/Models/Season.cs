using System.Collections.Generic;

namespace shlscrapr.Models
{
    public class Season
    {
        public int Id { get; set; }
        public int StartGame { get; set; }
        public int LastGame { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
    
        public static List<Season> Seasons
        {
            get
            {
                var brands = new List<Season>
                {
                    new Season() {Id = 20122013, Name = "Elitserien 12/13", StartGame = 3793, LastGame = 3995, FileName = "shl_2012_2013"},
                    new Season() {Id = 20132014, Name = "SHL 13/14", StartGame = 3996, LastGame = 4413, FileName = "shl_2013_2014"},
                    new Season() {Id = 20142015, Name = "SHL 14/15", StartGame = 4420, LastGame = 4681, FileName = "shl_2014_2015"},
                };
                return brands;
            }
        }

    }
}
