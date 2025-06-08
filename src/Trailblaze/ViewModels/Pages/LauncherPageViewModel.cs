using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Lucide.Avalonia;
using Trailblaze.Common.Settings;
using Trailblaze.Models.Messages;

namespace Trailblaze.ViewModels.Pages;

public sealed partial class LauncherPageViewModel : PageViewModel, ITransientViewModel
{
    public LauncherPageViewModel(AppSettings appSettings)
        : base(appSettings) { }

    public override string DisplayName => "Launcher";
    public override long Order => 1;
    public override LucideIconKind Icon => LucideIconKind.House;

    [RelayCommand]
    private void OpenDailyCheckIn()
    {
        Messenger.Send(new OpenWebViewMessage("https://github.com/Cysharp/ZLinq"));
    }
}
