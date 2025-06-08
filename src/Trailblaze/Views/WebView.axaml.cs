using Trailblaze.ViewModels;
using Trailblaze.Views.Abstractions;

namespace Trailblaze.Views;

public sealed partial class WebView : UserControl<WebViewModel>
{
    public WebView()
    {
        InitializeComponent();
    }
}
