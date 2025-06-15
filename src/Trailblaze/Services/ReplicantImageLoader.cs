using System.Threading.Tasks;
using AsyncImageLoader;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Replicant;

namespace Trailblaze.Services;

public sealed class ReplicantImageLoader : IAsyncImageLoader
{
    private readonly HttpCache _httpCache;

    public ReplicantImageLoader(HttpCache httpCache)
    {
        _httpCache = httpCache;
    }

    public async Task<Bitmap?> ProvideImageAsync(string url)
    {
        try
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (AssetLoader.Exists(uri))
                return new Bitmap(AssetLoader.Open(uri));
        }
        catch (Exception)
        {
            // ignored
        }

        await using var imageStream = await _httpCache.StreamAsync(url).ConfigureAwait(false);
        return new Bitmap(imageStream);
    }

    public void Dispose() { }
}
