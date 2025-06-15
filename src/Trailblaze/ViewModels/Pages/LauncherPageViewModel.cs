using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Lucide.Avalonia;
using R3;
using R3.ObservableEvents;
using Trailblaze.Core;
using Trailblaze.Localization;
using Trailblaze.Models.Messages;

namespace Trailblaze.ViewModels.Pages;

public sealed partial class LauncherPageViewModel : PageViewModel, ITransientViewModel
{
    private readonly IDisposable _subscriptions;

    public LauncherPageViewModel()
    {
        Games = GameBiz
            .List.Where(s => s.Server != GameServer.None)
            .DistinctBy(s => s.Game)
            .ToList();

        Servers = Games.DistinctBy(s => s.Server).Select(s => s.Server).ToList();

        foreach (var gameBiz in Games)
        {
            Console.WriteLine(gameBiz.Value);
        }

        foreach (var server in Servers)
        {
            Console.WriteLine($"Server: {server.Value}");
        }

        var d = Disposable.CreateBuilder();

        AppSettings
            .ObservePropertyChanged(s => s.Game)
            .Subscribe(OnSelectedGameChanged)
            .AddTo(ref d);

        Localizer
            .Current.Events()
            .LanguageChanged.Subscribe(_ => OnAllPropertiesChanged())
            .AddTo(ref d);

        _subscriptions = d.Build();
    }

    public override string DisplayName => "Launcher";
    public override long Order => 1;
    public override LucideIconKind Icon => LucideIconKind.House;

    [ObservableProperty]
    public partial IReadOnlyList<GameBiz> Games { get; set; }

    [ObservableProperty]
    public partial IReadOnlyList<GameServer> Servers { get; set; }

    public override void OnUnloaded()
    {
        _subscriptions.Dispose();
    }

    [RelayCommand]
    private void OpenDailyCheckIn()
    {
        Messenger.Send(new OpenWebViewMessage("https://github.com/Cysharp/ZLinq"));
    }

    private void OnSelectedGameChanged(GameBiz biz)
    {
        Console.WriteLine($"GameBiz Changed: {biz.Value} | {biz.Server}");
    }
}
