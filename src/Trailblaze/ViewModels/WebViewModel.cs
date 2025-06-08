using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Trailblaze.Common.Helpers;
using Trailblaze.Common.Settings;
using Trailblaze.Models.Messages;

namespace Trailblaze.ViewModels;

public sealed partial class WebViewModel : ViewModel, ITransientViewModel
{
    private const string EmptyPageUrl = "about:blank";

    public WebViewModel(AppSettings appSettings)
        : base(appSettings)
    {
        CreationProperties = new CoreWebView2CreationProperties
        {
            UserDataFolder = PathHelper.CacheDirectory,
        };
    }

    public CoreWebView2CreationProperties CreationProperties { get; }

    [ObservableProperty]
    public partial Uri Url { get; set; } = new(EmptyPageUrl);

    [RelayCommand]
    private void Close()
    {
        Messenger.Send(new CloseWebViewMessage());
    }
}
