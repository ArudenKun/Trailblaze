using System.Threading;
using System.Threading.Tasks;
using Trailblaze.Core;

namespace Trailblaze.Services;

public sealed class BackgroundService
{
    private Task<string?> GetBackgroundImageUrlAsync(
        Game game,
        CancellationToken cancellationToken = default
    )
    {
        // var background = await _hoYoPlayService.GetGameBackgroundAsync(gameId, cancellationToken);
        // return background.Backgrounds.FirstOrDefault()?.Background.Url;
        throw new NotImplementedException();
    }
}
