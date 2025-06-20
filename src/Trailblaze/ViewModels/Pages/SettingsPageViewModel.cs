using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lucide.Avalonia;
using SukiUI.Models;
using Trailblaze.Models;
using Trailblaze.Services;

namespace Trailblaze.ViewModels.Pages;

public sealed partial class SettingsPageViewModel : PageViewModel, ITransientViewModel
{
    private readonly ThemeService _themeService;

    public SettingsPageViewModel(ThemeService themeService)
    {
        _themeService = themeService;
        ThemeService = themeService;

        IsVisibleOnSideMenu = false;
    }

    public override string DisplayName => "Settings";
    public override long Order => long.MaxValue;
    public override LucideIconKind Icon => LucideIconKind.Settings;
    public override bool AutoHideOnSideMenu => true;

    public ThemeService ThemeService { get; }

    [ObservableProperty]
    public partial bool IsSystemTheme { get; set; }

    [ObservableProperty]
    public partial bool IsLightTheme { get; set; }

    [ObservableProperty]
    public partial bool IsDarkTheme { get; set; }

    [RelayCommand]
    public void SwitchToColorTheme(SukiColorTheme color) => _themeService.SwitchColorTheme(color);

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(IsSystemTheme):
            {
                if (IsSystemTheme)
                    _themeService.ChangeBaseTheme(AppTheme.Default);
                break;
            }
            case nameof(IsLightTheme):
            {
                if (IsLightTheme)
                    _themeService.ChangeBaseTheme(AppTheme.Light);
                break;
            }
            case nameof(IsDarkTheme):
            {
                if (IsDarkTheme)
                    _themeService.ChangeBaseTheme(AppTheme.Dark);
                break;
            }
        }

        base.OnPropertyChanged(e);
    }
}
