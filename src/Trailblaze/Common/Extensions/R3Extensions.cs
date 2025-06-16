using Trailblaze.ViewModels;

namespace Trailblaze.Common.Extensions;

public static class R3Extensions
{
    public static T AddTo<T>(this T disposable, ViewModel viewModel)
        where T : IDisposable
    {
        viewModel.Disposables.Add(disposable);
        return disposable;
    }
}
