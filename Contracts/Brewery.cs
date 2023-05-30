namespace BrewTrack.Contracts
{
    public interface IBrewPub
    {
        string Id { get; set; }
        string Name { get; set; }
        string City { get; set; }
        string Website_Uri { get; set; }
        string Phone { get; set; }
        string Type { get; set; }
    }

    public interface IBrewPubApi: IBrewPub
    {
        string Longitude { get; set; }
        string Latitude { get; set; }
    }
}
