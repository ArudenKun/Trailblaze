using System.IO;
using Trailblaze.Common.Extensions;
using Velopack.Locators;

namespace Trailblaze.Common.Helpers;

public static class PathHelper
{
    /// <summary>
    ///     Returns the directory from which the application is run.
    /// </summary>
    public static string AppDirectory =>
        VelopackLocator.Current.RootAppDir ?? AppContext.BaseDirectory;

    public static string AppContentDirectory =>
        VelopackLocator.Current.AppContentDir ?? AppContext.BaseDirectory;

    public static string AppTempDirectory =>
        VelopackLocator.Current.AppTempDir ?? Path.GetTempPath().CombinePath(AppHelper.Name);

    /// <summary>
    ///     Returns the path of the roaming directory.
    /// </summary>
    public static string RoamingDirectory =>
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    /// <summary>
    ///     Returns the directory of the user directory (ex: C:\Users\[the name of the user])
    /// </summary>
    public static string UserDirectory =>
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    /// <summary>
    ///     Returns the path of the ApplicationData.
    /// </summary>
    public static string DataDirectory =>
        // File.Exists(".portable") || Directory.Exists("data")
        VelopackLocator.Current.IsPortable || AppHelper.IsDebug
            ? AppDirectory.CombinePath("data")
            : RoamingDirectory.CombinePath(AppHelper.Name);

    public static string CacheDirectory => DataDirectory.CombinePath("cache");

    /// <summary>
    ///     Returns the directory of the downloads directory
    /// </summary>
    public static string DownloadsDirectory => UserDirectory.CombinePath("Downloads");

    public static string LogsDirectory => DataDirectory.CombinePath("logs");

    public static string SettingsPath => DataDirectory.CombinePath("settings.json");

    public static string DatabasePath =>
        $"Data Source={DataDirectory.CombinePath($"{AppHelper.Name}.db")}";
}
