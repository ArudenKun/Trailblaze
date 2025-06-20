using System.Collections.Generic;
using Avalonia.Media;
using Avalonia.Styling;
using SukiUI;
using SukiUI.Models;
using Trailblaze.Common.Settings;
using Trailblaze.Models;
using ZLinq;

namespace Trailblaze.Services;

public sealed class ThemeService
{
    private readonly AppSettings _appSettings;

    public ThemeService(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    public SukiTheme SukiTheme { get; private set; } = null!;

    public IReadOnlyList<SukiColorTheme> AvailableColors { get; private set; } = [];

    public void Initialize()
    {
        SukiTheme = SukiTheme.GetInstance();
        AvailableColors =
        [
            .. SukiTheme.ColorThemes,
            new SukiColorTheme("Pink", new Color(255, 255, 20, 147), new Color(255, 255, 192, 203)),
            new SukiColorTheme("White", new Color(255, 255, 255, 255), new Color(255, 0, 0, 0)),
            new SukiColorTheme("Black", new Color(255, 0, 0, 0), new Color(255, 255, 255, 255)),
        ];

        var colorTheme = AvailableColors
            .AsValueEnumerable()
            .First(s => s.DisplayName == _appSettings.ThemeColor);

        SwitchColorTheme(colorTheme);
        ChangeBaseTheme(_appSettings.Theme);
    }

    public void SwitchColorTheme(SukiColorTheme colorTheme)
    {
        _appSettings.ThemeColor = colorTheme.DisplayName;
        SukiTheme.ChangeColorTheme(colorTheme);
    }

    public void ChangeBaseTheme(AppTheme theme)
    {
        _appSettings.Theme = theme;
        switch (theme)
        {
            case AppTheme.Default:
                SukiTheme.ChangeBaseTheme(ThemeVariant.Default);
                break;
            case AppTheme.Light:
                SukiTheme.ChangeBaseTheme(ThemeVariant.Light);
                break;
            case AppTheme.Dark:
                SukiTheme.ChangeBaseTheme(ThemeVariant.Dark);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(theme), theme, "Invalid Base Theme");
        }
    }
}
