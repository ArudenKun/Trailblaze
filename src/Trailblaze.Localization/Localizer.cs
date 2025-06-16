using System.Collections.ObjectModel;
using System.Globalization;

namespace Trailblaze.Localization;

public static class Localizer
{
    public static ILocalizer Current { get; private set; } = NullLocalizer.Instance;

    public static void SetLocalizer(ILocalizer localizer)
    {
        Current = localizer;
    }

    public static IReadOnlyList<CultureInfo> Languages => Current.Languages;

    public static ObservableCollection<string> DisplayLanguages => Current.DisplayLanguages;

    public static CultureInfo Language
    {
        get => Current.Language;
        set => Current.Language = value;
    }

    public static int LanguageIndex
    {
        get => Current.LanguageIndex;
        set => Current.LanguageIndex = value;
    }

    public static string Get(string key)
    {
        return Current.Get(key);
    }

    public static event EventHandler? LanguageChanged
    {
        add => Current.LanguageChanged += value;
        remove => Current.LanguageChanged -= value;
    }
}
