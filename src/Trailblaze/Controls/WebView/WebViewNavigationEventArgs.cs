using System;

namespace Trailblaze.Controls.WebView;

public class WebViewNavigationEventArgs : EventArgs
{
    public Uri? Request { get; init; }
}
