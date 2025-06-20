using System.Collections.Frozen;
using Vogen;

namespace Trailblaze.Core.HoyoPlay;

[ValueObject<string>]
public sealed partial record LauncherId
{
    public static readonly LauncherId China = From("jGHBHlcOq1");

    public static readonly LauncherId Global = From("VYTpXlbWo8");

    public static readonly LauncherId BilibiliGenshin = From("umfgRO5gh5");

    public static readonly LauncherId BilibiliStarRail = From("6P5gHMNyK3");

    public static readonly LauncherId BilibiliZenless = From("xV0f4r1GT0");

    public bool IsChina => this == China;
    public bool IsGlobal => this == Global;

    public bool IsBilibili =>
        this == BilibiliGenshin || this == BilibiliStarRail || this == BilibiliZenless;

    public static LauncherId FromGameBiz(GameBiz gameBiz)
    {
        switch (gameBiz)
        {
            case var _ when gameBiz == GameBiz.GenshinChina:
            case var _ when gameBiz == GameBiz.StarRailChina:
            case var _ when gameBiz == GameBiz.ZenlessChina:
                return China;
            case var _ when gameBiz == GameBiz.GenshinGlobal:
            case var _ when gameBiz == GameBiz.StarRailGlobal:
            case var _ when gameBiz == GameBiz.ZenlessGlobal:
                return Global;
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

    public static LauncherId FromGameId(GameId gameId) => FromGameBiz(gameId.GameBiz);

    public static FrozenSet<(GameBiz GameBiz, LauncherId LauncherId)> BilibiliLaunchers { get; } =
        [
            (GameBiz.GenshinBilibili, BilibiliGenshin),
            (GameBiz.StarRailBilibili, BilibiliStarRail),
            (GameBiz.ZenlessBilibili, BilibiliZenless),
        ];
}
