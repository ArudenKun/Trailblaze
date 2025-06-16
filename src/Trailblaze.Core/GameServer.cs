using Ardalis.SmartEnum;

namespace Trailblaze.Core;

public sealed class GameServer : SmartEnum<GameServer, string>
{
    public static readonly GameServer None = new(nameof(None), string.Empty);
    public static readonly GameServer Global = new(nameof(Global), "global");
    public static readonly GameServer China = new(nameof(China), "cn");
    public static readonly GameServer Bilibili = new(nameof(Bilibili), "bilibili");

    #region Special

    public static readonly GameServer Overseas = new(nameof(Overseas), "os");
    public static readonly GameServer Japan = new(nameof(Japan), "jp");
    public static readonly GameServer Korea = new(nameof(Korea), "kr");
    public static readonly GameServer Asia = new(nameof(Asia), "asia");

    #endregion

    private GameServer(string name, string value)
        : base(name, value) { }
}
