using System.Text.Json;
using System.Text.Json.Serialization;

namespace Trailblaze.Core.JsonConverters;

public sealed class GameBizJsonConverter : JsonConverter<GameBiz>
{
    public override GameBiz? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) => GameBiz.FromName(reader.GetString());

    public override void Write(
        Utf8JsonWriter writer,
        GameBiz value,
        JsonSerializerOptions options
    ) => writer.WriteStringValue(value.Name);
}
