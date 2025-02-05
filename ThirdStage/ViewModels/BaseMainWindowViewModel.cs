using ReactiveUI;
using System;
using System.Threading;

namespace ThirdStage.ViewModels;

public class BaseMainWindowViewModel : ReactiveObject, IRoutableViewModel, IDisposable
{
    public IScreen HostScreen { get; }

    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public BaseMainWindowViewModel(IScreen screen)
    {
        HostScreen = screen;
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }
}
