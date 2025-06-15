using System.Globalization;
using Trailblaze.Localization;

namespace Trailblaze.Translations;

public class ResXLocalizer : BaseLocalizer
{
    public override void Reload()
    {
        if (CurrentLanguages.Count == 0)
        {
            CurrentLanguages.Add("en-US");
        }

        ValidateLanguage();

        Translation.Culture = new CultureInfo(CurrentLanguage);

        HasLoaded = true;

        UpdateDisplayLanguages();
    }

    protected override void OnLanguageChanged() => Reload();

    public override string Get(string key)
    {
        if (!HasLoaded)
            Reload();

        var langString = Translation.ResourceManager.GetString(key, Translation.Culture);
        return langString != null ? langString.Replace("\\n", "\n") : $"{Language}:{key}";
    }
}
