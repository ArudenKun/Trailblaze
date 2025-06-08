using Trailblaze.ViewModels;

namespace Trailblaze.Views.Abstractions;

public interface IView;

public interface IView<out TViewModel> : IView
    where TViewModel : ViewModel
{
    TViewModel DataContext { get; }
}
