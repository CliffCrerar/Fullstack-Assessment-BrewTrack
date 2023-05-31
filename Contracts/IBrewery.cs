namespace BrewTrack.Contracts.IBrewery
{
    public interface ICoordinates
    {
        double Longitude { get; set; }
        double Latitude { get; set; }
    }

    public interface IBrewPub : ICoordinates
    {
        int Id { get; set; }
        string Name { get; set; }
        string City { get; set; }
        string Website_Uri { get; set; }
        string Phone { get; set; }
        string Type { get; set; }
    }
}
