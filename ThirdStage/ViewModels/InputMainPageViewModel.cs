using ReactiveUI;
using System;

namespace ThirdStage.ViewModels
{
    public class InputMainPageViewModel : ReactiveObject, IRoutableViewModel
    {
        public IScreen HostScreen { get; }

        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public InputMainPageViewModel(IScreen screen) => HostScreen = screen;
    }
}
