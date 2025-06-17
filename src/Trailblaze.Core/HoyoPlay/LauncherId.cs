using System.Collections.Frozen;
using Vogen;

namespace Trailblaze.Core.HoyoPlay;

[ValueObject<string>]
public sealed partial record LauncherId
{
    public static readonly LauncherId ChinaOfficial = From("jGHBHlcOq1");

    public static readonly LauncherId GlobalOfficial = From("VYTpXlbWo8");

    public static readonly LauncherId BilibiliGenshin = From("umfgRO5gh5");

    public static readonly LauncherId BilibiliStarRail = From("6P5gHMNyK3");

    public static readonly LauncherId BilibiliZenless = From("xV0f4r1GT0");

    public static bool IsChinaOfficial(string launcherId) => From(launcherId) == ChinaOfficial;

    public static bool IsGlobalOfficial(string launcherId) => From(launcherId) == GlobalOfficial;

    public static bool IsBilibili(string launcherId) =>
        From(launcherId) == BilibiliGenshin
        || From(launcherId) == BilibiliStarRail
        || From(launcherId) == BilibiliZenless;

    public static LauncherId FromGameBiz(GameBiz gameBiz)
    {
        switch (gameBiz)
        {
            case var _ when gameBiz == GameBiz.GenshinChina:
            case var _ when gameBiz == GameBiz.StarRailChina:
            case var _ when gameBiz == GameBiz.ZenlessChina:
                return ChinaOfficial;
            case var _ when gameBiz == GameBiz.GenshinGlobal:
            case var _ when gameBiz == GameBiz.StarRailGlobal:
            case var _ when gameBiz == GameBiz.ZenlessGlobal:
                return GlobalOfficial;
            case var _ when gameBiz == GameBiz.GenshinBilibili:
                return BilibiliGenshin;
            case var _ when gameBiz == GameBiz.StarRailBilibili:
                return BilibiliStarRail;
            case var _ when gameBiz == GameBiz.ZenlessBilibili:
                return BilibiliZenless;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameBiz), "Unknown GameBiz");
        }
    }

    // public static string? FromGameId(GameId gameId) => FromGameBiz(gameId.GameBiz);

    public static FrozenSet<(GameBiz GameBiz, LauncherId LauncherId)> BilibiliLaunchers { get; } =
        [
            (GameBiz.GenshinBilibili, BilibiliGenshin),
            (GameBiz.StarRailBilibili, BilibiliStarRail),
            (GameBiz.ZenlessBilibili, BilibiliZenless),
        ];
}
