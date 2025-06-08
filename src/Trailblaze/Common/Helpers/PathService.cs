using System;
using System.IO;
using Trailblaze.Common.Extensions;
using Velopack.Locators;

namespace Trailblaze.Common.Helpers;

public static class PathHelper
{
    private static IVelopackLocator Locator => VelopackLocator.Current;

    /// <summary>
    ///     Returns the directory from which the application is run.
    /// </summary>
    public static string AppDirectory => Locator.RootAppDir ?? AppContext.BaseDirectory;

    public static string AppContentDirectory => Locator.AppContentDir ?? AppContext.BaseDirectory;

    public static string AppTempDirectory =>
        Locator.AppTempDir ?? Path.GetTempPath().CombinePath(AppInformation.Name);

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
        Locator.IsPortable
            ? AppDirectory.CombinePath("data")
            : RoamingDirectory.CombinePath(AppInformation.Name);

    public static string CacheDirectory => DataDirectory.CombinePath("cache");

    /// <summary>
    ///     Returns the directory of the downloads directory
    /// </summary>
    public static string DownloadsDirectory => UserDirectory.CombinePath("Downloads");

    public static string LogsDirectory => DataDirectory.CombinePath("logs");

    public static string SettingsPath => DataDirectory.CombinePath("settings.json");

    public static string DatabasePath =>
        $"Data Source={DataDirectory.CombinePath($"{AppInformation.Name.ToLowerInvariant()}.db")}";
}
