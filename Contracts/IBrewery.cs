namespace BrewTrack.Contracts.IBrewery
{
    public interface ICoordinates
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }

    public interface IBrewPub : ICoordinates
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Website_Url { get; set; }
        public string Phone { get; set; }
        public string Brewery_Type { get; set; }
    }
}
