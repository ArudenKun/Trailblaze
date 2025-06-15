using System.Collections.Frozen;
using Vogen;

namespace Trailblaze.Core;

[ValueObject<string>]
public partial record Game
{
    public static readonly Game Genshin = new(nameof(Genshin));
    public static readonly Game StarRail = new(nameof(StarRail));
    public static readonly Game Zenless = new(nameof(Zenless));

    public static readonly FrozenSet<Game> Games = [Genshin, StarRail, Zenless];
}
