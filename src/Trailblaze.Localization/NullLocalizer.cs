namespace Trailblaze.Localization;

public sealed class NullLocalizer : BaseLocalizer
{
    private NullLocalizer() { }

    public static readonly NullLocalizer Instance = new();

    protected override void OnLanguageChanged() { }

    public override void Reload() { }

    public override string Get(string key) => $"Key: {key}";
}
