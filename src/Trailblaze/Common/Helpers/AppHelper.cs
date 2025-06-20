using Velopack.Locators;

namespace Trailblaze.Common.Helpers;

public static class AppHelper
{
    public static string Name => VelopackLocator.Current.AppId ?? "Trailblaze";

    public static bool IsDebug
#if DEBUG
        => true;
#else
        => false;
#endif
}
