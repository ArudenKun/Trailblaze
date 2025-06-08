using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Trailblaze.Common.Settings;
using Trailblaze.ViewModels.Pages;
using ZLinq;
using ZLogger;

namespace Trailblaze.ViewModels;

public sealed partial class MainViewModel : ViewModel, ISingletonViewModel
{
    public MainViewModel(
        AppSettings appSettings,
        IEnumerable<PageViewModel> pages,
        ILogger<MainViewModel> logger
    )
        : base(appSettings)
    {
        Pages = [.. pages.AsValueEnumerable().OrderBy(s => s.Order)];
        foreach (var (i, page) in Pages.AsValueEnumerable().Index())
        {
            logger.ZLogInformation($"Page {i + 1}: {page.DisplayName}");
        }

        ActivePage = Pages[0];
    }

    public IReadOnlyList<PageViewModel> Pages { get; }

    [ObservableProperty]
    public partial PageViewModel ActivePage { get; set; }

    [RelayCommand]
    private void ShowSettings()
    {
        var settings = Pages
            .AsValueEnumerable()
            .FirstOrDefault(page => page is SettingsPageViewModel);

        if (settings is null)
            return;

        ActivePage = settings;
    }
}
