using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Trailblaze.Common.Settings;
using Trailblaze.Models.Messages;
using Trailblaze.Services;

namespace Trailblaze.ViewModels;

public sealed partial class MainWindowViewModel
    : ViewModel,
        ISingletonViewModel,
        IRecipient<OpenWebViewMessage>,
        IRecipient<CloseWebViewMessage>
{
    private readonly MainViewModel _mainViewModel;
    private readonly ViewModelFactory _viewModelFactory;

    public MainWindowViewModel(
        AppSettings appSettings,
        MainViewModel mainViewModel,
        ViewModelFactory viewModelFactory
    )
        : base(appSettings)
    {
        Messenger.Register<OpenWebViewMessage>(this);
        Messenger.Register<CloseWebViewMessage>(this);

        _mainViewModel = mainViewModel;
        _viewModelFactory = viewModelFactory;

        ActiveContent = mainViewModel;
    }

    [ObservableProperty]
    public partial ViewModel ActiveContent { get; set; }

    [ObservableProperty]
    public partial bool IsTransitionReversed { get; set; } = true;

    public void Receive(OpenWebViewMessage message)
    {
        IsTransitionReversed = false;
        ActiveContent = _viewModelFactory.CreateWebViewModel(message.Url);
    }

    public void Receive(CloseWebViewMessage message)
    {
        IsTransitionReversed = true;
        ActiveContent = _mainViewModel;
    }
}
