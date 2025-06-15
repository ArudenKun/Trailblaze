using System.Collections.Concurrent;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Trailblaze.ViewModels;
using Trailblaze.Views.Abstractions;
using ZLinq;

namespace Trailblaze;

public sealed class ViewLocator : IDataTemplate
{
    private static readonly ConcurrentDictionary<Type, Type?> ViewTypeCache = new();
    private static readonly TextBlock ViewNotFoundControl = new();

    private readonly IServiceProvider _serviceProvider;

    public ViewLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Control Build(object? data) =>
        data is ViewModel viewModel ? TryBindView(viewModel) : ViewNotFoundControl;

    public bool Match(object? data) => data is ViewModel;

    public Control TryBindView(ViewModel viewModel)
    {
        if (TryCreateView(viewModel) is not { } view)
        {
            ViewNotFoundControl.Text = $"Could not find view for {viewModel.GetType().FullName}";
            return ViewNotFoundControl;
        }

        view.DataContext ??= viewModel;
        BindEvents(view, viewModel);
        return view;
    }

    private Control? TryCreateView(ViewModel viewModel)
    {
        var vmType = viewModel.GetType();
        var viewType = ViewTypeCache.GetOrAdd(vmType, FindViewTypeForViewModel);
        return viewType is null ? null : _serviceProvider.GetService(viewType) as Control;
    }

    private Type? FindViewTypeForViewModel(Type vmType) =>
        _serviceProvider
            .GetServices<IView>()
            .AsValueEnumerable()
            .Select(t => t.GetType())
            .Select(candidateViewType =>
                (
                    ViewType: candidateViewType,
                    Interface: candidateViewType
                        .GetInterfaces()
                        .AsValueEnumerable()
                        .FirstOrDefault(i =>
                            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IView<>)
                        )
                )
            )
            .Where(t => t.Interface is not null)
            .Select(t => (t.ViewType, ViewModelType: t.Interface!.GetGenericArguments()[0]))
            .FirstOrDefault(t => t.ViewModelType.IsAssignableFrom(vmType))
            .ViewType;

    public static void BindEvents(Control control, ViewModel viewModel)
    {
        control.Loaded += Loaded;
        control.Unloaded += Unloaded;
        return;

        void Loaded(object? sender, RoutedEventArgs e) => viewModel.OnLoaded();

        void Unloaded(object? sender, RoutedEventArgs e)
        {
            viewModel.OnUnloaded();
            control.Loaded -= Loaded;
            control.Unloaded -= Unloaded;
        }
    }
}
