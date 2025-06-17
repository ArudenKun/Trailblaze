using Ardalis.SmartEnum;

namespace Trailblaze.Core;

public abstract class GameBiz : SmartEnum<GameBiz, string>
{
    #region Genshin

    public static readonly GameBiz Genshin = new GenshinBiz("hk4e", GameServer.None);
    public static readonly GameBiz GenshinGlobal = new GenshinBiz("hk4e_global", GameServer.Global);
    public static readonly GameBiz GenshinChina = new GenshinBiz("hk4e_cn", GameServer.China);

    public static readonly GameBiz GenshinBilibili = new GenshinBiz(
        "hk4e_bilibili",
        GameServer.Bilibili
    );

    #endregion

    #region StarRail

    public static readonly GameBiz StarRail = new StarRailBiz("hkrpg", GameServer.None);

    public static readonly GameBiz StarRailGlobal = new StarRailBiz(
        "hkrpg_global",
        GameServer.Global
    );

    public static readonly GameBiz StarRailChina = new StarRailBiz("hkrpg_cn", GameServer.China);

    public static readonly GameBiz StarRailBilibili = new StarRailBiz(
        "hkrpg_bilibili",
        GameServer.Bilibili
    );

    #endregion

    #region Zenless

    public static readonly GameBiz Zenless = new ZenlessBiz("nap", GameServer.None);
    public static readonly GameBiz ZenlessGlobal = new ZenlessBiz("nap_global", GameServer.Global);
    public static readonly GameBiz ZenlessChina = new ZenlessBiz("nap_cn", GameServer.China);

    public static readonly GameBiz ZenlessBilibili = new ZenlessBiz(
        "nap_bilibili",
        GameServer.Bilibili
    );

    #endregion

    private GameBiz(string value, GameServer server)
        : base(value, value)
    {
        Server = server;
    }

    public abstract Game Game { get; }

    public GameServer Server { get; }

    public bool IsKnown() =>
        this == GenshinGlobal
        || this == GenshinChina
        || this == GenshinBilibili
        || this == StarRailGlobal
        || this == StarRailChina
        || this == StarRailBilibili
        || this == ZenlessGlobal
        || this == ZenlessChina
        || this == ZenlessBilibili;

    private sealed class GenshinBiz : GameBiz
    {
        public GenshinBiz(string value, GameServer server)
            : base(value, server) { }

        public override Game Game => Game.Genshin;
    }

    private sealed class StarRailBiz : GameBiz
    {
        public StarRailBiz(string value, GameServer server)
            : base(value, server) { }

        public override Game Game => Game.StarRail;
    }

    private sealed class ZenlessBiz : GameBiz
    {
        public ZenlessBiz(string value, GameServer server)
            : base(value, server) { }

        public override Game Game => Game.Zenless;
    }
}
