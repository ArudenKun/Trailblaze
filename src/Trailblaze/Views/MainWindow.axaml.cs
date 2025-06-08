using Trailblaze.ViewModels;
using Trailblaze.Views.Abstractions;

namespace Trailblaze.Views;

public partial class MainWindow : SukiWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
