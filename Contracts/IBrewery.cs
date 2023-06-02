namespace BrewTrack.Contracts.IBrewery
{
    public interface ICoordinates
    {
        string Longitude { get; set; }
        string Latitude { get; set; }
    }

    public interface IBrewPub : ICoordinates
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string City { get; set; }
        string Website_Uri { get; set; }
        string Phone { get; set; }
        string Type { get; set; }
    }
}
