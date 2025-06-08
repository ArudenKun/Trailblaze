using Trailblaze.ViewModels;
using Trailblaze.Views.Abstractions;

namespace Trailblaze.Views;

public sealed partial class MainView : UserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
    }
}
