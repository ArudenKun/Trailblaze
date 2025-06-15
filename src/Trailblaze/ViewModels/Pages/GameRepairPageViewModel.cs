using Lucide.Avalonia;

namespace Trailblaze.ViewModels.Pages;

public sealed class GameRepairPageViewModel : PageViewModel, ITransientViewModel
{
    public GameRepairPageViewModel() { }

    public override string DisplayName => "Game Repair";
    public override long Order => 3;
    public override LucideIconKind Icon => LucideIconKind.Wrench;
}
