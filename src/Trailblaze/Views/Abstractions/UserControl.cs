using Avalonia.Controls;
using Trailblaze.ViewModels;

namespace Trailblaze.Views.Abstractions;

public abstract class UserControl<TViewModel> : UserControl, IView<TViewModel>
    where TViewModel : ViewModel
{
    public new TViewModel DataContext
    {
        get =>
            base.DataContext as TViewModel
            ?? throw new InvalidCastException(
                $"DataContext is null or not of the expected type '{typeof(TViewModel).FullName}'."
            );
        set => base.DataContext = value;
    }
}
