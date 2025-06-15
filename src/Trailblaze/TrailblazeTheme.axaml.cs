using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Trailblaze;

public sealed class TrailblazeTheme : Styles
{
    public TrailblazeTheme()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
