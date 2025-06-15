using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Media;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lucide.Avalonia;
using SukiUI;
using SukiUI.Models;
using Trailblaze.Models;

namespace Trailblaze.ViewModels.Pages;

public sealed partial class SettingsPageViewModel : PageViewModel, ITransientViewModel
{
    public SettingsPageViewModel()
    {
        Theme = SukiTheme.GetInstance();
        AvailableColors =
        [
            .. Theme.ColorThemes,
            new SukiColorTheme("Pink", new Color(255, 255, 20, 147), new Color(255, 255, 192, 203)),
            new SukiColorTheme("White", new Color(255, 255, 255, 255), new Color(255, 0, 0, 0)),
            new SukiColorTheme("Black", new Color(255, 0, 0, 0), new Color(255, 255, 255, 255)),
        ];

        IsSystemTheme = AppSettings.Theme is ApplicationTheme.Default;
        IsLightTheme = AppSettings.Theme is ApplicationTheme.Light;
        IsDarkTheme = AppSettings.Theme is ApplicationTheme.Dark;

        IsVisibleOnSideMenu = false;
    }

    public override string DisplayName => "Settings";
    public override long Order => long.MaxValue;
    public override LucideIconKind Icon => LucideIconKind.Settings;
    public override bool AutoHideOnSideMenu => true;

    public SukiTheme Theme { get; }

    [ObservableProperty]
    public partial bool IsSystemTheme { get; set; }

    [ObservableProperty]
    public partial bool IsLightTheme { get; set; }

    [ObservableProperty]
    public partial bool IsDarkTheme { get; set; }

    public IReadOnlyList<SukiColorTheme> AvailableColors { get; }

    [RelayCommand]
    public void SwitchToColorTheme(SukiColorTheme color)
    {
        AppSettings.ThemeColor = color.DisplayName;
        Theme.ChangeColorTheme(color);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(IsSystemTheme):
            {
                if (IsSystemTheme)
                    ChangeBaseTheme(ApplicationTheme.Default);
                break;
            }
            case nameof(IsLightTheme):
            {
                if (IsLightTheme)
                    ChangeBaseTheme(ApplicationTheme.Light);
                break;
            }
            case nameof(IsDarkTheme):
            {
                if (IsDarkTheme)
                    ChangeBaseTheme(ApplicationTheme.Dark);
                break;
            }
        }

        base.OnPropertyChanged(e);
    }

    private void ChangeBaseTheme(ApplicationTheme theme)
    {
        AppSettings.Theme = theme;
        switch (theme)
        {
            case ApplicationTheme.Default:
                Theme.ChangeBaseTheme(ThemeVariant.Default);
                break;
            case ApplicationTheme.Light:
                Theme.ChangeBaseTheme(ThemeVariant.Light);
                break;
            case ApplicationTheme.Dark:
                Theme.ChangeBaseTheme(ThemeVariant.Dark);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
        }
    }
}
