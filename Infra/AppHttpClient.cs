namespace BrewTrack.Infra;
public class AppHttpClient
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task<string> GetAsync(string url)
    {
        try
        {
            using HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await client.GetStringAsync(url);
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            return e.Message;
        }
    }
}
