using Lucide.Avalonia;
using Trailblaze.Common.Settings;

namespace Trailblaze.ViewModels.Pages;

public class GameSettingsPageViewModel : PageViewModel, ITransientViewModel
{
    public GameSettingsPageViewModel(AppSettings appSettings)
        : base(appSettings) { }

    public override string DisplayName => "Game Settings";
    public override long Order => 2;
    public override LucideIconKind Icon => LucideIconKind.MonitorCog;
}
