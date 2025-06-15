using Avalonia;
using Avalonia.Markup.Xaml;

namespace Trailblaze.Localization.Avalonia;

// https://github.com/tifish/Jeek.Avalonia.Localization
public class LocalizeMarkupExtension(string key) : MarkupExtension, IObservable<string>, IDisposable
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this.ToBinding();

    private IObserver<string>? _observer;

    public IDisposable Subscribe(IObserver<string> observer)
    {
        _observer = observer;
        _observer.OnNext(Localizer.Get(key));
        Localizer.LanguageChanged += OnLanguageChanged;

        return this;
    }

    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        _observer?.OnNext(Localizer.Get(key));
    }

    /// <inheritdoc cref="IDisposable"/>>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        Localizer.LanguageChanged -= OnLanguageChanged;
        _observer = null;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~LocalizeMarkupExtension()
    {
        Dispose(false);
    }
}
