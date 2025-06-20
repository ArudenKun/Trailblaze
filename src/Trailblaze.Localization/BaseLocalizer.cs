using System.Collections.ObjectModel;
using System.Globalization;

namespace Trailblaze.Localization;

public abstract class BaseLocalizer : ILocalizer
{
    // Fallback language when the current language is not found
    public CultureInfo FallbackLanguage { get; set; } = CultureInfo.GetCultureInfo("en-US");

    protected List<CultureInfo> CurrentLanguages { get; } = [];

    // List of available languages, e.g. ["en", "zh"]
    public IReadOnlyList<CultureInfo> Languages
    {
        get
        {
            if (!HasLoaded)
                Reload();

            return CurrentLanguages;
        }
    }

    // Display names of available languages in current language, e.g. ["English", "Chinese"]
    public ObservableCollection<string> DisplayLanguages { get; } =
        new FastObservableCollection<string>();

    protected CultureInfo CurrentLanguage { get; set; } = CultureInfo.CurrentCulture;

    // Current language, e.g. "en"
    public CultureInfo Language
    {
        get => CurrentLanguage;
        set
        {
            if (EqualityComparer<CultureInfo>.Default.Equals(CurrentLanguage, value))
                return;

            CurrentLanguage = value;
            OnLanguageChanged();
            FireLanguageChanged();
        }
    }

    // Index of current language in Languages
    public int LanguageIndex
    {
        get;
        set
        {
            if (field == value)
                return;

            if (value < 0 || value >= CurrentLanguages.Count)
                return;

            field = value;

            Language = CurrentLanguages[field];
        }
    } = -1;

    // Must be called after _language or _languages are changed
    protected void ValidateLanguage()
    {
        var languageIndex = CurrentLanguages.IndexOf(CurrentLanguage);
        if (languageIndex != -1)
        {
            LanguageIndex = languageIndex;
            return;
        }

        languageIndex = CurrentLanguages.IndexOf(FallbackLanguage);
        if (languageIndex == -1)
            throw new KeyNotFoundException(CurrentLanguage.Name);

        LanguageIndex = languageIndex;
        CurrentLanguage = FallbackLanguage;
    }

    // Must be called after _languages are changed or translation is changed
    protected void UpdateDisplayLanguages()
    {
        var displayLanguages = Languages.Select(ci => ci.EnglishName).ToList();
        if (!displayLanguages.SequenceEqual(DisplayLanguages))
            ((IFastObservableCollection)DisplayLanguages).Replace(displayLanguages);
    }

    // Implementations deal with loading language strings
    protected abstract void OnLanguageChanged();

    protected bool HasLoaded { get; set; }

    // Reload language strings
    public abstract void Reload();

    // Get language string by key
    public abstract string Get(string key);

    public event EventHandler? LanguageChanged;

    // Fire language changed
    public void FireLanguageChanged()
    {
        LanguageChanged?.Invoke(null, EventArgs.Empty);
    }
}
