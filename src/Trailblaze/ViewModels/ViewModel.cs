using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using R3;
using SukiUI.Dialogs;
using SukiUI.Toasts;
using Trailblaze.Avalonia.Hosting;
using Trailblaze.Common.Settings;

namespace Trailblaze.ViewModels;

public interface ISingletonViewModel : IViewModel;

public interface ITransientViewModel : IViewModel;

public interface IViewModel;

public abstract partial class ViewModel : ObservableRecipient, IDisposable
{
    private static readonly Lazy<SukiDialogManager> LazySukiDialogManager = new();
    private static readonly Lazy<SukiToastManager> LazySukiToastManager = new();

    private bool _isDisposed;

    public ISukiDialogManager DialogManager => LazySukiDialogManager.Value;
    public ISukiToastManager ToastManager => LazySukiToastManager.Value;

    // ReSharper disable once CollectionNeverQueried.Global
    public CompositeDisposable Disposables { get; } = new();

    public AppSettings AppSettings { get; } =
        App.Current.GetServiceProvider().GetRequiredService<AppSettings>();

    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    public virtual void OnLoaded() { }

    public virtual void OnUnloaded() { }

    /// <summary>
    /// Dispatches the specified action on the UI thread synchronously.
    /// </summary>
    /// <param name="action">The action to be dispatched.</param>
    protected static void Dispatch(Action action) => Dispatcher.UIThread.Invoke(action);

    /// <summary>
    /// Dispatches the specified action on the UI thread synchronously.
    /// </summary>
    /// <param name="action">The action to be dispatched.</param>
    protected static TResult Dispatch<TResult>(Func<TResult> action) =>
        Dispatcher.UIThread.Invoke(action);

    /// <summary>
    /// Dispatches the specified action on the UI thread.
    /// </summary>
    /// <param name="action">The action to be dispatched.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected static async Task DispatchAsync(Action action) =>
        await Dispatcher.UIThread.InvokeAsync(action);

    /// <summary>
    /// Dispatches the specified action on the UI thread asynchronously.
    /// </summary>
    /// <param name="action">The action to be dispatched.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected static async Task<TResult> DispatchAsync<TResult>(Func<TResult> action) =>
        await Dispatcher.UIThread.InvokeAsync(action);

    protected void OnAllPropertiesChanged() => OnPropertyChanged(string.Empty);

    ~ViewModel() => Dispose(false);

    /// <inheritdoc cref="Dispose"/>>
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (!disposing)
            return;

        Disposables.Dispose();

        _isDisposed = true;
    }

    /// <inheritdoc />>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
