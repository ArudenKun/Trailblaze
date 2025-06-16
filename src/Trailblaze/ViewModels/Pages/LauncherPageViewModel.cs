using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Lucide.Avalonia;
using R3;
using R3.ObservableEvents;
using Trailblaze.Common.Extensions;
using Trailblaze.Core;
using Trailblaze.Localization;
using Trailblaze.Models.Messages;

namespace Trailblaze.ViewModels.Pages;

public sealed partial class LauncherPageViewModel : PageViewModel, ITransientViewModel
{
    public LauncherPageViewModel()
    {
        SelectedGame = AppSettings.GameBiz.Game;
        SelectedGameServer = AppSettings.GameBiz.Server;

        this.ObservePropertyChanged(s => s.SelectedGame)
            .ObserveOnUIThreadDispatcher()
            .Skip(1)
            .Subscribe(_ => SelectedCore())
            .AddTo(this);

        this.ObservePropertyChanged(s => s.SelectedGameServer)
            .ObserveOnUIThreadDispatcher()
            .Skip(1)
            .Subscribe(_ => SelectedCore())
            .AddTo(this);

        // Localizer
        //     .Current.Events()
        //     .LanguageChanged.Subscribe(_ => OnAllPropertiesChanged())
        //     .AddTo(this);
    }

    public override string DisplayName => "Launcher";
    public override long Order => 1;
    public override LucideIconKind Icon => LucideIconKind.House;

    [ObservableProperty]
    public partial CultureInfo SelectedLanguage { get; set; } = Localizer.Language;

    partial void OnSelectedLanguageChanged(CultureInfo value)
    {
        Localizer.Language = value;
    }

    [ObservableProperty]
    public partial Game SelectedGame { get; set; }

    [ObservableProperty]
    public partial GameServer SelectedGameServer { get; set; }

    [RelayCommand]
    private void OpenDailyCheckIn()
    {
        Messenger.Send(new OpenWebViewMessage("https://github.com/Cysharp/ZLinq"));
    }

    private void SelectedCore()
    {
        AppSettings.GameBiz = SelectedGame.ToGameBiz(SelectedGameServer);
        Console.WriteLine($"Game Chaged: {SelectedGame.Title} | {AppSettings.GameBiz.Server}");
    }
}
