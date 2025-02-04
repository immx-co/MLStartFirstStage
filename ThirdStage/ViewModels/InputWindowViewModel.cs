using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Reactive;
using ClassLibrary.Database;
using Avalonia.Media;
using System.Reactive.Linq;

namespace ThirdStage.ViewModels
{
    public class InputWindowViewModel : ReactiveObject
    {
        #region ViewModel Settings
        private readonly IServiceProvider _serviceProvider;

        public RoutingState Router { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> BackToInputWindow { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoApplication { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> Input { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> Registration { get; }
        #endregion

        private bool _isVerifiedEmail = false;
        public bool IsVerifiedEmail
        {
            get => _isVerifiedEmail;
            set => this.RaiseAndSetIfChanged(ref _isVerifiedEmail, value);
        }

        private readonly ObservableAsPropertyHelper<IBrush> _emailStatusColor;
        public IBrush EmailStatusColor => _emailStatusColor.Value;

        public InputWindowViewModel(PasswordHasher hasher, IConfiguration configuration, IServiceProvider serviceProvider, IScreen screenRealization)
        {
            this.WhenAnyValue(x => x.IsVerifiedEmail)
                .Select(isVerified => isVerified ? Brushes.Green : Brushes.Red)
                .ToProperty(this, x => x.EmailStatusColor, out _emailStatusColor);

            Router = screenRealization.Router;
            _serviceProvider = serviceProvider;

            Router.Navigate.Execute(_serviceProvider.GetRequiredService<InputMainPageViewModel>());
            BackToInputWindow = ReactiveCommand.CreateFromObservable(() =>
            {
                return Router.Navigate.Execute(_serviceProvider.GetRequiredService<InputMainPageViewModel>());
            });
            Input = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(_serviceProvider.GetRequiredService<AutorizationWindowViewModel>()));
            Registration = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(_serviceProvider.GetRequiredService<RegistrationViewModel>()));
            GoApplication = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(_serviceProvider.GetRequiredService<MainWindowViewModel>()));
        }
    }
}
