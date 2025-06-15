using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.VisualTree;

namespace Trailblaze.Common.Extensions;

public static class AvaloniaExtensions
{
    public static Window GetMainWindow(this IApplicationLifetime? lifetime) =>
        lifetime?.TryGetMainWindow()
        ?? throw new InvalidOperationException(
            "MainWindow not found. Ensure that the application lifetime is set correctly and a main window is defined."
        );

    public static TopLevel GetTopLevel(this IApplicationLifetime? lifetime) =>
        lifetime?.TryGetTopLevel()
        ?? throw new InvalidOperationException(
            "TopLevel not found. Ensure that the application lifetime is set correctly and a main window is defined."
        );

    public static Window? TryGetMainWindow(this IApplicationLifetime lifetime) =>
        lifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime
            ? desktopLifetime.MainWindow
            : null;

    public static TopLevel? TryGetTopLevel(this IApplicationLifetime lifetime) =>
        lifetime.TryGetMainWindow()
        ?? (lifetime as ISingleViewApplicationLifetime)?.MainView?.GetVisualRoot() as TopLevel;

    public static bool TryShutdown(this IApplicationLifetime lifetime, int exitCode = 0)
    {
        switch (lifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktopLifetime:
                return desktopLifetime.TryShutdown(exitCode);
            case IControlledApplicationLifetime controlledLifetime:
                controlledLifetime.Shutdown(exitCode);
                return true;
            default:
                return false;
        }
    }
}
