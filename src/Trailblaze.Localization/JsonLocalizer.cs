using System.Text.Json;

namespace Trailblaze.Localization;

public class JsonLocalizer : BaseLocalizer
{
    private readonly string _languageJsonDirectory;

    private Dictionary<string, string>? _languageStrings;

    public JsonLocalizer(string languageJsonDirectory)
    {
        _languageJsonDirectory = languageJsonDirectory;
    }

    public override void Reload()
    {
        _languageStrings = null;
        CurrentLanguages.Clear();

        if (!Directory.Exists(_languageJsonDirectory))
            throw new FileNotFoundException(_languageJsonDirectory);

        foreach (var file in Directory.GetFiles(_languageJsonDirectory, "*.json"))
        {
            var language = Path.GetFileNameWithoutExtension(file);
            CurrentLanguages.Add(language);
        }

        ValidateLanguage();

        var languageFile = Path.Combine(_languageJsonDirectory, CurrentLanguage + ".json");
        if (!File.Exists(languageFile))
            throw new FileNotFoundException($"No language file ${languageFile}");

        var json = File.ReadAllText(languageFile);
        _languageStrings = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

        HasLoaded = true;

        UpdateDisplayLanguages();
    }

    protected override void OnLanguageChanged()
    {
        Reload();
    }

    public override string Get(string key)
    {
        if (!HasLoaded)
            Reload();

        if (_languageStrings == null)
            throw new Exception("No language strings loaded.");

        if (_languageStrings.TryGetValue(key, out var langStr))
            return langStr.Replace("\\n", "\n");

        return $"{Language}:{key}";
    }
}
