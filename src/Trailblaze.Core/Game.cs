using System.Collections.Frozen;
using Ardalis.SmartEnum;
using Trailblaze.Localization;
using Trailblaze.Translations;

namespace Trailblaze.Core;

public abstract class Game : SmartEnum<Game, string>
{
    public const string GenshinKey = "Genshin";
    public const string StarRailKey = "StarRail";
    public const string ZenlessKey = "Zenless";

    public static readonly Game Genshin = new GenshinGame();
    public static readonly Game StarRail = new StarRailGame();
    public static readonly Game Zenless = new ZenlessGame();

    // ReSharper disable once CollectionNeverQueried.Global
    public static readonly FrozenSet<Game> Games = [Genshin, StarRail, Zenless];

    protected Game(string name, string biz)
        : base(name, biz) { }

    public abstract string Title { get; }

    public virtual FrozenSet<GameServer> Servers { get; } =
        [GameServer.Global, GameServer.China, GameServer.Bilibili];

    public GameBiz ToGameBiz(GameServer gameServer) =>
        GameBiz.TryFromName($"{Value}_{gameServer.Value}", out var gameBiz)
            ? gameBiz
            : GameBiz.FromName(Value);

    private sealed class GenshinGame : Game
    {
        public GenshinGame()
            : base(GenshinKey, GameBiz.Genshin.Name) { }

        public override string Title => Localizer.Get(TranslationKey.GenshinTitle);
    }

    private sealed class StarRailGame : Game
    {
        public StarRailGame()
            : base(StarRailKey, GameBiz.StarRail.Name) { }

        public override string Title => Localizer.Get(TranslationKey.StarRailTitle);
    }

    private sealed class ZenlessGame : Game
    {
        public ZenlessGame()
            : base(ZenlessKey, GameBiz.Zenless.Name) { }

        public override string Title => Localizer.Get(TranslationKey.ZenlessTitle);
    }
}
