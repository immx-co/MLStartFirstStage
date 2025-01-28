using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Serilog;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace ThirdStage.ViewModels
{
    public class JokesWindowViewModel : ReactiveObject, IRoutableViewModel
    {
        public IServiceProvider _servicesProvider;

        public IScreen HostScreen { get; }

        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ICommand FlipLeftCommand { get; init; }

        public JokesWindowViewModel(IScreen screen, IServiceProvider servicesProvider)
        {
            _servicesProvider = servicesProvider;
            HostScreen = screen;

            FlipLeftCommand = ReactiveCommand.Create(FlipLeft);
        }

        private void FlipLeft()
        {
            Debug.WriteLine("Нажата кнопка налево");
            Log.Logger.Information("Нажата кнопка '<'");
            HostScreen.Router.Navigate.Execute(_servicesProvider.GetRequiredService<MainWindowViewModel>());
        }
    }
}
