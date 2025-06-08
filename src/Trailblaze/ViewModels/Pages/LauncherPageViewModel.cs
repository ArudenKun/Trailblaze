using Lucide.Avalonia;
using Trailblaze.Common.Settings;

namespace Trailblaze.ViewModels.Pages;

public sealed class LauncherPageViewModel : PageViewModel, ITransientViewModel
{
    public LauncherPageViewModel(AppSettings appSettings)
        : base(appSettings) { }

    public override string DisplayName => "Launcher";
    public override long Order => 1;
    public override LucideIconKind Icon => LucideIconKind.House;
}
