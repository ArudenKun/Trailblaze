using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Trailblaze;

public class TrailblazeTheme : Styles
{
    public TrailblazeTheme()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
