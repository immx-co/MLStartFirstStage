using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Reactive;
using ClassLibrary.Database;

namespace ThirdStage.ViewModels
{
    public class InputWindowViewModel : ReactiveObject
    {
        private readonly IServiceProvider _serviceProvider;

        public RoutingState Router { get;  }

        public ReactiveCommand<Unit, IRoutableViewModel> BackToInputWindow { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoApplication { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> Input { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> Registration { get; }

        public InputWindowViewModel(PasswordHasher hasher, IConfiguration configuration, IServiceProvider serviceProvider, IScreen screenRealization)
        {
            Router = screenRealization.Router;
            _serviceProvider = serviceProvider;

            Router.Navigate.Execute(_serviceProvider.GetRequiredService<InputMainPageViewModel>());
            BackToInputWindow = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(_serviceProvider.GetRequiredService<InputMainPageViewModel>()));
            Input = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(_serviceProvider.GetRequiredService<AutorizationWindowViewModel>()));
            Registration = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(_serviceProvider.GetRequiredService<RegistrationViewModel>()));
            GoApplication = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(_serviceProvider.GetRequiredService<MainWindowViewModel>()));

        }
    }
}
