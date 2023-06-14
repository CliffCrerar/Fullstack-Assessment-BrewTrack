namespace BrewTrack.Dto
{
    public class WeatherForeCastApiRequestDto
    {
        public IList<WeatherForeCastEntry> Hours { get; set; }
        public WeatherForecastPageMeta Meta { get; set; }
    }

    public class WeatherForeCastEntry
    {
        public NoaaSource AirTemperature { get; set; }
        public string Time { get; set; }

    }

    public class NoaaSource
    {
        public decimal Noaa { get; set; }
    }

    public class WeatherForecastPageMeta
    {
        public int Cost { get; set; }
        public int DailyQuota { get; set; }
        public string End { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get;set; }
        public string[] Params { get; set; }
        public int RequestCount { get; set; }
        public string[] Source { get; set; }
        public string Start { get; set; }
    }

}
