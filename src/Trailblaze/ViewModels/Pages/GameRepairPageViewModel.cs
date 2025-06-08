using Lucide.Avalonia;
using Trailblaze.Common.Settings;

namespace Trailblaze.ViewModels.Pages;

public sealed class GameRepairPageViewModel : PageViewModel, ITransientViewModel
{
    public GameRepairPageViewModel(AppSettings appSettings)
        : base(appSettings) { }

    public override string DisplayName => "Game Repair";
    public override long Order => 3;
    public override LucideIconKind Icon => LucideIconKind.Wrench;
}
