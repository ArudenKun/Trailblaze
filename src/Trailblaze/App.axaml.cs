using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Trailblaze.ViewModels;

namespace Trailblaze;

public sealed class App : Application
{
    private readonly ViewLocator _viewLocator;
    private readonly IServiceProvider _serviceProvider;

    public App(ViewLocator viewLocator, IServiceProvider serviceProvider)
    {
        _viewLocator = viewLocator;
        _serviceProvider = serviceProvider;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        // NativeWebView.Options.UserDataFolder = PathHelper.CacheDirectory.CombinePath("webview2");
        DataTemplates.Add(_viewLocator);
    }

    [RequiresUnreferencedCode("Calls Avalonia.Data.Core.Plugins.BindingPlugins.DataValidators")]
    [SuppressMessage(
        "Trimming",
        "IL2046:\'RequiresUnreferencedCodeAttribute\' annotations must match across all interface implementations or overrides."
    )]
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            if (
                _viewLocator.TryBindView(_serviceProvider.GetRequiredService<MainWindowViewModel>())
                is not Window mainWindow
            )
            {
                throw new ArgumentNullException(
                    nameof(mainWindow),
                    "The configured main window in the view locator is null"
                );
            }

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    [RequiresUnreferencedCode("Calls Avalonia.Data.Core.Plugins.BindingPlugins.DataValidators")]
    private static void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove = BindingPlugins
            .DataValidators.OfType<DataAnnotationsValidationPlugin>()
            .ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
