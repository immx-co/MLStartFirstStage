using ReactiveUI;
using System;

namespace RoutingExample.ViewModels
{
    public class RegistrationViewModel : ReactiveObject, IRoutableViewModel
    {
        public IScreen HostScreen { get; }

        public string StringContent { get; } = "Registration";

        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public RegistrationViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
