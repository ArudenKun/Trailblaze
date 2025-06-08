using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Trailblaze.Controls;

public partial class DelayedContentControl : UserControl
{
    private readonly DispatcherTimer _timer;

    public static readonly StyledProperty<TimeSpan> DelayProperty = AvaloniaProperty.Register<
        DelayedContentControl,
        TimeSpan
    >(nameof(Delay), TimeSpan.FromSeconds(1), defaultBindingMode: BindingMode.OneTime);

    public static readonly StyledProperty<object?> ActualContentProperty =
        AvaloniaProperty.Register<DelayedContentControl, object?>(nameof(ActualContent));

    public DelayedContentControl()
    {
        InitializeComponent();

        _timer = new DispatcherTimer { Interval = Delay };
        _timer.Tick += OnTimerTick;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        _timer.Start();
    }

    public TimeSpan Delay
    {
        get => GetValue(DelayProperty);
        set => SetValue(DelayProperty, value);
    }

    public new object? Content
    {
        get => GetValue(ContentProperty);
        private set => SetValue(ContentProperty, value);
    }

    public object? ActualContent
    {
        get => GetValue(ActualContentProperty);
        set => SetValue(ActualContentProperty, value);
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        Content = ActualContent;
        _timer.Stop();
    }
}
