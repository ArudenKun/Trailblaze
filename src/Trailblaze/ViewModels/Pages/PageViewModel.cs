using CommunityToolkit.Mvvm.ComponentModel;
using Lucide.Avalonia;
using Trailblaze.Common.Settings;

namespace Trailblaze.ViewModels.Pages;

public interface IPageViewModel : IViewModel
{
    /// <summary>
    /// The display name of the page.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// The index of the page.
    /// </summary>
    long Order { get; }

    /// <summary>
    /// The icon of the page.
    /// </summary>
    LucideIconKind Icon { get; }

    /// <summary>
    /// The visibility of the page on the side menu.
    /// </summary>
    bool IsVisibleOnSideMenu { get; }

    /// <summary>
    /// Set to true to auto hide the page on the side menu.
    /// </summary>
    bool AutoHideOnSideMenu { get; }
}

public abstract partial class PageViewModel : ViewModel, IPageViewModel
{
    public PageViewModel(AppSettings appSettings)
        : base(appSettings) { }

    /// <inheritdoc />
    public abstract string DisplayName { get; }

    /// <inheritdoc />
    public abstract long Order { get; }

    /// <inheritdoc />
    public abstract LucideIconKind Icon { get; }

    /// <inheritdoc />
    [ObservableProperty]
    public partial bool IsVisibleOnSideMenu { get; protected set; } = true;

    /// <inheritdoc />
    public virtual bool AutoHideOnSideMenu => false;
}
