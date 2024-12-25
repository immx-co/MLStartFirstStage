using ReactiveUI;
using System;

namespace RoutingExample.ViewModels
{
    public class LoginViewModel : ReactiveObject, IRoutableViewModel
    {
        public IScreen HostScreen { get; }

        public string StringContent { get; } = "Login";

        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public LoginViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
