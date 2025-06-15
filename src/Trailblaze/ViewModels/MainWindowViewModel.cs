using Avalonia;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SukiUI.Dialogs;
using Trailblaze.Common.Extensions;
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

    public MainWindowViewModel(MainViewModel mainViewModel, ViewModelFactory viewModelFactory)
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

    public bool IsConfirmedClose { get; private set; }

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

    [RelayCommand]
    private void OpenSetting()
    {
        Messenger.Send<OpenSettingsMessage>();
    }

    [RelayCommand]
    private void TryClose()
    {
        DialogManager.DismissDialog();
        DialogManager
            .CreateDialog()
            .OfType(NotificationType.Warning)
            .WithTitle("Close")
            .WithContent("Do you really want to exit?")
            .WithActionButton(
                "Yes",
                _ =>
                {
                    IsConfirmedClose = true;
                    Application.Current?.ApplicationLifetime?.TryShutdown();
                }
            )
            .WithActionButton("No", _ => { }, true)
            .TryShow();
    }
}
