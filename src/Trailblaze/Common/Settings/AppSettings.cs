using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SukiUI.Enums;
using Trailblaze.Common.Helpers;
using Trailblaze.Core;
using Trailblaze.Models;
using ZLogger;

namespace Trailblaze.Common.Settings;

[INotifyPropertyChanged]
public sealed partial class AppSettings : JsonFile, IDisposable
{
    private readonly ILogger<AppSettings> _logger;

    public AppSettings(ILogger<AppSettings> logger)
        : base(PathHelper.SettingsPath, AppJsonContext.Default)
    {
        _logger = logger;

        Load();
    }

    [ObservableProperty]
    public partial GameBiz Game { get; set; } = GameBiz.Genshin;

    [ObservableProperty]
    public partial ApplicationTheme Theme { get; set; } = ApplicationTheme.Default;

    [ObservableProperty]
    public partial string ThemeColor { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool CheckForUpdates { get; set; } = true;

    [ObservableProperty]
    public partial DateTime LastUpdateDateTimeCheck { get; set; } = AppInformation.Born;

    [ObservableProperty]
    public partial bool IsSideMenuExpanded { get; set; } = true;

    [ObservableProperty]
    public partial bool BackgroundAnimations { get; set; } = true;

    [ObservableProperty]
    public partial bool BackgroundTransitions { get; set; } = true;

    [ObservableProperty]
    public partial SukiBackgroundStyle BackgroundStyle { get; set; } =
        SukiBackgroundStyle.GradientSoft;

    [ObservableProperty]
    public partial WindowState LastWindowState { get; set; } = WindowState.Normal;

    public override void Save()
    {
        _logger.ZLogInformation($"Saving {nameof(AppSettings)}");
        base.Save();
        _logger.ZLogInformation($"Saved {nameof(AppSettings)}");
    }

    public override bool Load()
    {
        _logger.ZLogInformation($"Loading {nameof(AppSettings)}");
        try
        {
            _logger.ZLogInformation($"Loaded {nameof(AppSettings)}");
            return base.Load();
        }
        catch (Exception)
        {
            _logger.ZLogWarning($"Failed to load {nameof(AppSettings)}. Using default settings");
            Reset();
            Save();
            return false;
        }
    }

    public void Dispose() => Save();
}
