namespace BrewTrack.Helpers
{
    internal class DataPage<T>
    {
        public int Page { get; set; }   
        public int OfPages { get; set; }
        public T Data { get; set; }
    }
    public class Pages
    {
        public static void Create()
        {

        }
    }
}
