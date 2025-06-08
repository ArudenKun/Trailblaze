using Microsoft.Extensions.DependencyInjection;
using Trailblaze.ViewModels;

namespace Trailblaze.Services;

public sealed class ViewModelFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ViewModelFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TViewModel Create<TViewModel>()
        where TViewModel : ViewModel => _serviceProvider.GetRequiredService<TViewModel>();

    public WebViewModel CreateWebViewModel(Url url)
    {
        var viewModel = Create<WebViewModel>();
        viewModel.Url = url.ToUri();
        return viewModel;
    }
}
