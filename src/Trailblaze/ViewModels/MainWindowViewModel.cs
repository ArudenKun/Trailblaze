using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trailblaze.Common.Settings;
using Trailblaze.ViewModels.Pages;
using ZLinq;

namespace Trailblaze.ViewModels;

public sealed partial class MainWindowViewModel : ViewModel, ISingletonViewModel
{
    public MainWindowViewModel(AppSettings appSettings, IEnumerable<PageViewModel> pages)
        : base(appSettings)
    {
        Pages = [.. pages.OrderBy(s => s.Order)];
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
