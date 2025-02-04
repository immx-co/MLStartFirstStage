using ReactiveUI;
using System;

namespace ThirdStage.ViewModels;

public class BaseMainWindowViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel
{
    public IScreen HostScreen { get; }

    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public ViewModelActivator Activator { get; }

    public event EventHandler<bool> SuccessfullLogoutEvent;

    public BaseMainWindowViewModel(IScreen screen)
    {
        HostScreen = screen;

        Activator = new();
    }
}
