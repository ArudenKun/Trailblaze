using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Threading;

namespace Trailblaze.Controls.WebView;

public class NativeWebView : NativeControlHost, IWebView, IDisposable
{
    public static readonly Uri EmptyPageLink = new("about:blank");
    private readonly WebView2Adapter _webViewAdapter = new();

    public event EventHandler<WebViewNavigationCompletedEventArgs>? NavigationCompleted;
    public event EventHandler<WebViewNavigationStartingEventArgs>? NavigationStarted;
    public event EventHandler<WebViewDomContentLoadedEventArgs>? DomContentLoaded;

    public static readonly StyledProperty<Uri?> SourceProperty = AvaloniaProperty.Register<
        NativeWebView,
        Uri?
    >(nameof(Source), EmptyPageLink);

    public static WebViewOptions Options { get; } = new();

    public Uri? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public bool CanGoBack => _webViewAdapter.CanGoBack;

    public bool CanGoForward => _webViewAdapter.CanGoForward;

    public bool GoBack() => _webViewAdapter.GoBack();

    public bool GoForward() => _webViewAdapter.GoForward();

    public NativeWebView()
    {
        _webViewAdapter.NavigationStarted += WebViewAdapterOnNavigationStarted;
        _webViewAdapter.NavigationCompleted += WebViewAdapterOnNavigationCompleted;
        _webViewAdapter.DomContentLoaded += WebViewAdapterOnDomContentLoaded;
        Loaded += NativeWebViewOnLoaded;
    }

    ~NativeWebView()
    {
        _webViewAdapter.NavigationStarted -= WebViewAdapterOnNavigationStarted;
        _webViewAdapter.NavigationCompleted -= WebViewAdapterOnNavigationCompleted;
        _webViewAdapter.DomContentLoaded -= WebViewAdapterOnDomContentLoaded;
        Loaded -= NativeWebViewOnLoaded;
    }

    public Task<string?> InvokeScript(string scriptName)
    {
        return _webViewAdapter is null
            ? throw new InvalidOperationException("Control was not initialized")
            : _webViewAdapter.InvokeScript(scriptName);
    }

    public void Navigate(Uri url)
    {
        (
            _webViewAdapter ?? throw new InvalidOperationException("Control was not initialized")
        ).Navigate(url);
    }

    public void NavigateToString(string text)
    {
        (
            _webViewAdapter ?? throw new InvalidOperationException("Control was not initialized")
        ).NavigateToString(text);
    }

    public bool Refresh() => _webViewAdapter.Refresh();

    public bool Stop() => _webViewAdapter.Stop();

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        _webViewAdapter.SetOptions(Options);
        WaitOnDispatcherFrame(_webViewAdapter.SetParentAsync(parent.Handle));
        return _webViewAdapter;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        WaitOnDispatcherFrame(_webViewAdapter.SetParentAsync(IntPtr.Zero));
        base.OnDetachedFromVisualTree(e);
    }

    private static void WaitOnDispatcherFrame(Task task, Dispatcher? dispatcher = null)
    {
        var frame = new DispatcherFrame();
        AggregateException? capturedException = null;

        task.ContinueWith(
            t =>
            {
                capturedException = t.Exception;
                frame.Continue = false; // 结束消息循环
            },
            TaskContinuationOptions.AttachedToParent
        );

        dispatcher ??= Dispatcher.UIThread;
        dispatcher.PushFrame(frame);

        if (capturedException != null)
        {
            throw capturedException;
        }
    }

    private void WebViewAdapterOnDomContentLoaded(
        object? sender,
        WebViewDomContentLoadedEventArgs e
    )
    {
        DomContentLoaded?.Invoke(this, e);
    }

    private void WebViewAdapterOnNavigationStarted(
        object? sender,
        WebViewNavigationStartingEventArgs e
    )
    {
        NavigationStarted?.Invoke(this, e);
    }

    private void WebViewAdapterOnNavigationCompleted(
        object? sender,
        WebViewNavigationCompletedEventArgs e
    )
    {
        SetCurrentValue(SourceProperty, e.Request);
        NavigationCompleted?.Invoke(this, e);
    }

    private void NativeWebViewOnLoaded(object? sender, RoutedEventArgs e)
    {
        if (Source is not null)
        {
            _webViewAdapter.Source = Source;
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property != SourceProperty)
            return;

        _webViewAdapter.Source = change.GetNewValue<Uri?>() ?? EmptyPageLink;
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        _webViewAdapter.HandleSizeChanged(e.NewSize);
        base.OnSizeChanged(e);
    }

    public void Dispose()
    {
        _webViewAdapter.Dispose();
        GC.SuppressFinalize(this);
    }
}
