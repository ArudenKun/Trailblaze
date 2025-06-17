using System.Text.Json.Serialization;
using Trailblaze.Core.JsonConverters;

namespace Trailblaze.Core.HoyoPlay;

public sealed record GameId(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("biz"), JsonConverter(typeof(GameBizJsonConverter))] GameBiz GameBiz
)
{
    public static GameId FromGameBiz(GameBiz gameBiz) =>
        gameBiz switch
        {
            _ when gameBiz == GameBiz.GenshinGlobal => new GameId(
                "gopR6Cufr3",
                GameBiz.GenshinGlobal
            ),
            _ when gameBiz == GameBiz.GenshinChina => new GameId(
                "1Z8W5NHUQb",
                GameBiz.GenshinChina
            ),
            _ when gameBiz == GameBiz.GenshinBilibili => new GameId(
                "T2S0Gz4Dr2",
                GameBiz.GenshinBilibili
            ),
            _ when gameBiz == GameBiz.StarRailGlobal => new GameId(
                "4ziysqXOQ8",
                GameBiz.StarRailGlobal
            ),
            _ when gameBiz == GameBiz.StarRailChina => new GameId(
                "64kMb5iAWu",
                GameBiz.StarRailChina
            ),
            _ when gameBiz == GameBiz.StarRailBilibili => new GameId(
                "EdtUqXfCHh",
                GameBiz.StarRailBilibili
            ),
            _ when gameBiz == GameBiz.StarRailGlobal => new GameId(
                "U5hbdsT9W7",
                GameBiz.ZenlessGlobal
            ),
            _ when gameBiz == GameBiz.ZenlessChina => new GameId(
                "x6znKlJ0xK",
                GameBiz.ZenlessChina
            ),
            _ when gameBiz == GameBiz.ZenlessBilibili => new GameId(
                "HXAFlmYa17",
                GameBiz.ZenlessBilibili
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(gameBiz), "Unknown GameBiz"),
        };
}
