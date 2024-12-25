using ReactiveUI;
using System;
using ThirdStage.ViewModels;
using ThirdStage.Views;

namespace ThirdStage
{
    public class AppViewLocator : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
        {
            MainWindowViewModel context => new MainWindow { ViewModel = context },
            InputMainPageViewModel context => new InputMainPageWindow { ViewModel = context },
            AutorizationWindowViewModel context => new AutorizationWindow { ViewModel = context },
            RegistrationViewModel context => new RegistrationWindow { ViewModel = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}
