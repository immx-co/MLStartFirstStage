using Microsoft.Extensions.Configuration;
using MsBox.Avalonia.Base;
using ReactiveUI;
using Serilog;
using System.IO;
using System.Reactive;
using ThirdStage.Database;

namespace ThirdStage.ViewModels
{
    public class InputWindowViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();

        public ReactiveCommand<Unit, IRoutableViewModel> BackToInputWindow { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoApplication { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> Input { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> Registration { get; }

        public InputWindowViewModel(PasswordHasher hasher)
        {
            string fileName = "MLstartConfig.json";
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory
                .GetCurrentDirectory())
                .AddJsonFile(fileName).Build();

            Log.Logger.Information("Конфигурация загружена успешно.");

            Router.Navigate.Execute(new InputMainPageViewModel(this));
            BackToInputWindow = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new InputMainPageViewModel(this)));
            Input = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new AutorizationWindowViewModel(this, configuration, hasher)));
            Registration = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new RegistrationViewModel(this, configuration, hasher)));
            GoApplication = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new MainWindowViewModel(this)));
        }
    }
}
