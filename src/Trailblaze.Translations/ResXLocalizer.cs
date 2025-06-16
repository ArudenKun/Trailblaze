using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Trailblaze.Localization;

namespace Trailblaze.Translations;

public class ResXLocalizer : BaseLocalizer
{
    private static readonly CultureInfo EnglishCulture = CultureInfo.GetCultureInfo("en-US");

    public override void Reload()
    {
        if (CurrentLanguages.Count == 0)
        {
            CurrentLanguages.Add(EnglishCulture);
            var assembly = Assembly.GetExecutingAssembly();

            string baseDir =
                Path.GetDirectoryName(assembly.Location) ?? AppDomain.CurrentDomain.BaseDirectory;
            string assemblyName = assembly.GetName().Name;

            foreach (string dir in Directory.GetDirectories(baseDir))
            {
                try
                {
                    string dirName = Path.GetFileName(dir);
                    if (dirName.Length > 0 && dirName.ToLower() != "bin")
                    {
                        CultureInfo ci = CultureInfo.GetCultureInfo(dirName);
                        string resourcePath = Path.Combine(dir, $"{assemblyName}.resources.dll");
                        if (File.Exists(resourcePath))
                        {
                            CurrentLanguages.Add(ci);
                        }
                    }
                }
                catch (ArgumentException)
                {
                    // Not a valid culture name
                }
            }
        }

        ValidateLanguage();

        Translation.Culture = CurrentLanguage;

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
