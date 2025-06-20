using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Trailblaze.Models.Messages;
using Trailblaze.ViewModels.Pages;
using ZLinq;

namespace Trailblaze.ViewModels;

public sealed partial class MainViewModel
    : ViewModel,
        IRecipient<OpenSettingsMessage>,
        ISingletonViewModel
{
    public MainViewModel(IEnumerable<PageViewModel> pages, ILogger<MainViewModel> logger)
    {
        Messenger.Register(this);
        Pages = [.. pages.AsValueEnumerable().OrderBy(s => s.Order)];

        foreach (var (i, page) in Pages.AsValueEnumerable().Index())
        {
            logger.LogInformation("Page {Index}: {DisplayName}", i + 1, page.DisplayName);
        }

        ActivePage = Pages[0];
    }

    public IReadOnlyList<PageViewModel> Pages { get; }

    [ObservableProperty]
    public partial PageViewModel ActivePage { get; set; }

    public void Receive(OpenSettingsMessage message)
    {
        ShowSettings();
    }

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
