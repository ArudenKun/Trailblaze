using Avalonia.Controls;
using Trailblaze.ViewModels;
using Trailblaze.Views.Abstractions;

namespace Trailblaze.Views;

public partial class MainWindow : SukiWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();

        Closing += OnClosing;
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (DataContext.IsConfirmedClose)
            return;

        e.Cancel = true;
        DataContext.TryCloseCommand.Execute(null);
    }
}
