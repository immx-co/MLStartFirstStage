using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Reactive;
using ThirdStage.Database;

namespace ThirdStage.ViewModels
{
    public class InputWindowViewModel : ReactiveObject
    {
        private readonly IServiceProvider _serviceProvider;

        //public RoutingState Router { get; } = new RoutingState();

        RoutingState Router { get;  }

        public ReactiveCommand<Unit, IRoutableViewModel> BackToInputWindow { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoApplication { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> Input { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> Registration { get; }

        public InputWindowViewModel(PasswordHasher hasher, IConfiguration configuration, IServiceProvider serviceProvider, IScreenRealization screenRealization)
        {
            Router = screenRealization.Router;
            _serviceProvider = serviceProvider;

            //Router.Navigate.Execute(new InputMainPageViewModel(this));
            //BackToInputWindow = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new InputMainPageViewModel(this)));
            //Input = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new AutorizationWindowViewModel(this, configuration, hasher)));
            //Registration = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new RegistrationViewModel(this, configuration, hasher)));
            //GoApplication = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new MainWindowViewModel(this)));

            Router.Navigate.Execute(_serviceProvider.GetRequiredService<InputMainPageViewModel>());
            BackToInputWindow = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(_serviceProvider.GetRequiredService<InputMainPageViewModel>()));
            Input = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(_serviceProvider.GetRequiredService<AutorizationWindowViewModel>()));
            Registration = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(_serviceProvider.GetRequiredService<RegistrationViewModel>()));
            GoApplication = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(_serviceProvider.GetRequiredService<MainWindowViewModel>()));

        }
    }
}
