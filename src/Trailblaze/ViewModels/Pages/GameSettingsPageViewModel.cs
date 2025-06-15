using Lucide.Avalonia;

namespace Trailblaze.ViewModels.Pages;

public class GameSettingsPageViewModel : PageViewModel, ITransientViewModel
{
    public override string DisplayName => "Game Settings";
    public override long Order => 2;
    public override LucideIconKind Icon => LucideIconKind.MonitorCog;
}
