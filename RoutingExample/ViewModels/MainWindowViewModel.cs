using ReactiveUI;
using System.Reactive;

namespace RoutingExample.ViewModels
{
    public partial class MainWindowViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();

        public ReactiveCommand<Unit, IRoutableViewModel> GoNext { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoBack => Router.NavigateBack;

        public ReactiveCommand<Unit, IRoutableViewModel> Login { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> Registration { get; }

        public MainWindowViewModel()
        {
            GoNext = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new FirstViewModel(this)));
            Login = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new LoginViewModel(this)));
            Registration = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new RegistrationViewModel(this)));
        }
    }
}
