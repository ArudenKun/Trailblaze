using System.Text.Json.Serialization;
using Ardalis.SmartEnum.SystemTextJson;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using SukiUI.Enums;
using Trailblaze.Common.Helpers;
using Trailblaze.Core;
using Trailblaze.Models;

namespace Trailblaze.Common.Settings;

[INotifyPropertyChanged]
public sealed partial class AppSettings : JsonFile, IDisposable
{
    public AppSettings()
        : base(PathHelper.SettingsPath, AppJsonContext.Default) { }

    [ObservableProperty]
    [JsonConverter(typeof(SmartEnumNameConverter<GameBiz, string>))]
    public partial GameBiz GameBiz { get; set; } = GameBiz.GenshinGlobal;

    [ObservableProperty]
    public partial AppTheme Theme { get; set; } = AppTheme.Default;

    [ObservableProperty]
    public partial string ThemeColor { get; set; } = nameof(SukiColor.Blue);

    [ObservableProperty]
    public partial bool CheckForUpdates { get; set; } = true;

    [ObservableProperty]
    public partial DateTime LastUpdateDateTimeCheck { get; set; } = DateTime.MinValue;

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

    public LoggerSettings Logger { get; init; } = new();

    // public override void Save()
    // {
    //     _logger.LogInformation("Saving {Name}", nameof(AppSettings));
    //     base.Save();
    //     _logger.LogInformation("Saved {Name}", nameof(AppSettings));
    // }
    //
    // public override bool Load()
    // {
    //     _logger.LogInformation("Loading {Name}", nameof(AppSettings));
    //     try
    //     {
    //         var isSuccess = base.Load();
    //         if (isSuccess)
    //         {
    //             _logger.LogInformation("Loaded {Name}", nameof(AppSettings));
    //         }
    //         else
    //         {
    //             _logger.LogWarning("Failed to load {Name}, Using defaults", nameof(AppSettings));
    //         }
    //
    //         return isSuccess;
    //     }
    //     catch (Exception)
    //     {
    //         _logger.LogWarning("Failed to load {Name}, Using defaults", nameof(AppSettings));
    //         Reset();
    //         Save();
    //         return false;
    //     }
    // }
    public void Dispose() => Save();
}
