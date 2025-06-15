using System.Net;

namespace Trailblaze.Core.HoyoPlay;

public class HoyoPlayClient
{
    private readonly HttpClient _httpClient;

    public HoyoPlayClient(HttpClient? httpClient = null)
    {
        _httpClient =
            httpClient
            ?? new HttpClient(
                new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All }
            )
            {
                DefaultRequestVersion = HttpVersion.Version20,
            };
    }
}
