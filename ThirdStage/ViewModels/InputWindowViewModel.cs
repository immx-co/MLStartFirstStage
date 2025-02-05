using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Reactive;
using ClassLibrary.Database;
using Avalonia.Media;
using System.Reactive.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace ThirdStage.ViewModels
{
    public class InputWindowViewModel : ReactiveObject
    {
        #region ViewModel Settings
        private readonly IServiceProvider _serviceProvider;

        public RoutingState Router { get; }

        public ReactiveCommand<Unit, Unit> BackToInputWindow { get; }

        public ReactiveCommand<Unit, Unit> Input { get; }

        public ReactiveCommand<Unit, Unit> Registration { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoApplication { get; }
        #endregion

        #region Property Region
        private bool _isVerifiedEmail = false;
        public bool IsVerifiedEmail
        {
            get => _isVerifiedEmail;
            set => this.RaiseAndSetIfChanged(ref _isVerifiedEmail, value);
        }

        //private bool _isEmailVerificationPending = true;
        //public bool IsEmailVerificationPending
        //{
        //    get => _isEmailVerificationPending;
        //    set => this.RaiseAndSetIfChanged(ref _isEmailVerificationPending, value);
        //}
        #endregion

        private ObservableAsPropertyHelper<IBrush> _emailStatusColor;
        public IBrush EmailStatusColor => _emailStatusColor.Value;

        public InputWindowViewModel(PasswordHasher hasher, IConfiguration configuration, IServiceProvider serviceProvider, IScreen screenRealization)
        {
            this.WhenAnyValue(x => x.IsVerifiedEmail)
                .Select(isVerified => isVerified ? Brushes.Green : Brushes.Red)
                .ToProperty(this, x => x.EmailStatusColor, out _emailStatusColor);

            Router = screenRealization.Router;
            _serviceProvider = serviceProvider;

            Router.Navigate.Execute(_serviceProvider.GetRequiredService<InputMainPageViewModel>());
            BackToInputWindow = ReactiveCommand.Create(NavigateToInputWindow);
            Input = ReactiveCommand.Create(NavigateToLoginWindow);
            Registration = ReactiveCommand.Create(NavigateToRegistrationWindow);
            GoApplication = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(_serviceProvider.GetRequiredService<MainWindowViewModel>()));
        }

        private void NavigateToInputWindow()
        {
            CheckDisposedCancelletionToken();
            Router.Navigate.Execute(_serviceProvider.GetRequiredService<InputMainPageViewModel>());
        }

        private void NavigateToLoginWindow()
        {
            CheckDisposedCancelletionToken();
            Router.Navigate.Execute(_serviceProvider.GetRequiredService<AutorizationWindowViewModel>());
        }

        private void NavigateToRegistrationWindow()
        {
            CheckDisposedCancelletionToken();
            Router.Navigate.Execute(_serviceProvider.GetRequiredService<RegistrationViewModel>());
        }

        private void CheckDisposedCancelletionToken()
        {
            if (Router.NavigationStack.Count > 0)
            {
                var currentViewModel = Router.NavigationStack.Last();
                if (currentViewModel is IDisposable disposableViewModel)
                {
                    disposableViewModel.Dispose();
                }
            }
        }
    }
}
