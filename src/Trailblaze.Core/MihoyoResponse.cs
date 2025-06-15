using System.Text.Json.Serialization;

namespace Trailblaze.Core;

public sealed class MihoyoResponse<T>
    where T : class
{
    [JsonPropertyName("retcode")]
    public required int Retcode { get; init; }

    [JsonPropertyName("message")]
    public required string Message { get; init; }

    [JsonPropertyName("data")]
    public required T Data { get; init; }
}
