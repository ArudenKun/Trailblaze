using System.Collections.ObjectModel;
using System.Globalization;

namespace Trailblaze.Localization;

public interface ILocalizer
{
    void Reload();
    IReadOnlyList<CultureInfo> Languages { get; }
    ObservableCollection<string> DisplayLanguages { get; }
    CultureInfo Language { get; set; }
    int LanguageIndex { get; set; }
    CultureInfo FallbackLanguage { get; set; }
    string Get(string key);
    event EventHandler? LanguageChanged;
}
