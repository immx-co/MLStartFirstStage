using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.Json;
using System.Collections.Generic;
using ClassLibrary.Database;
using ClassLibrary.Database.Models;
using System.Linq;
using Serilog;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ThirdStage.ViewModels
{
    public class JokesWindowViewModel : BaseMainWindowViewModel
    {
        HubConnectionWrapper _hubConnectionWrapper;

        #region Default Settings Region
        public IServiceProvider _servicesProvider;
        public InputWindowViewModel _inputWindowViewModel;

        #endregion

        #region Avalonia Commands Region
        public ICommand FlipLeftCommand { get; set; }
        public ICommand FlipRightCommand { get; init; }

        private string _displayedJoke = "There will be a random joke here.";
        public string DisplayedJoke
        {
            get => _displayedJoke;
            set => this.RaiseAndSetIfChanged(ref _displayedJoke, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public ICommand RandomJokeCommand { get; init; }
        public ICommand RandomTenCommand { get; init; }
        public ICommand RandomJokesCommand { get; init; }
        public ICommand TenJokesCommand { get; init; }
        #endregion

        public JokesWindowViewModel(IScreen screen, IServiceProvider servicesProvider, InputWindowViewModel inputWindowViewModel, HubConnectionWrapper hubConnectionWrapper) : base(screen)
        {
            _servicesProvider = servicesProvider;
            _inputWindowViewModel = inputWindowViewModel;
            _hubConnectionWrapper = hubConnectionWrapper;

            _inputWindowViewModel.AreNavigationButtonsEnabled = false;

            RandomJokeCommand = ReactiveCommand.Create(GetRandomJoke);
            RandomTenCommand = ReactiveCommand.Create(GetRandomTen);
            RandomJokesCommand = ReactiveCommand.Create(GetRandomJokes);
            TenJokesCommand = ReactiveCommand.Create(GetTenJokes);

            FlipRightCommand = ReactiveCommand.Create(FlipRight);
            FlipLeftCommand = ReactiveCommand.Create(FlipLeft);

            #region Get Random Joke Methods
            _hubConnectionWrapper.Connection.On<Joke>("GetRandomJokeOk", (joke) =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    DisplayedJoke = $"{joke.Setup} {joke.Punchline}";
                });
            });

            _hubConnectionWrapper.Connection.On("GetRandomJokeNotOk", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    DisplayedJoke = "Ошибка при GetRandomJoke :(";
                });
            });
            #endregion

            #region Get Random Ten Methods
            _hubConnectionWrapper.Connection.On("GetRandomTenClear", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    DisplayedJoke = string.Empty;
                });
            });

            _hubConnectionWrapper.Connection.On<int, Joke>("GetRandomTenPostOneJoke", (index, joke) =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    DisplayedJoke += $"{index + 1}. {joke.Setup} {joke.Punchline}\n\n";
                });
            });

            _hubConnectionWrapper.Connection.On("GetRandomTenNotOk", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    DisplayedJoke = "Ошибка при GetRandomTen :(";
                });
            });
            #endregion

            #region Get Random Jokes Methods
            _hubConnectionWrapper.Connection.On<Joke>("GetRandomJokesOk", (joke) =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    DisplayedJoke = $"{joke.Setup} {joke.Punchline}";
                });
            });

            _hubConnectionWrapper.Connection.On("GetRandomJokesNotOk", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    DisplayedJoke = "Ошибка при GetRandomJokes :(";
                });
            });
            #endregion

            #region Get Ten Jokes Methods
            _hubConnectionWrapper.Connection.On("GetTenJokesClear", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    DisplayedJoke = string.Empty;
                });
            });

            _hubConnectionWrapper.Connection.On<int, Joke>("GetTenJokesPostOneJoke", (index, joke) =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    DisplayedJoke += $"{index + 1}. {joke.Setup} {joke.Punchline}\n\n";
                });
            });

            _hubConnectionWrapper.Connection.On("GetTenJokesNotOk", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    DisplayedJoke = "Ошибка при GetTenJokes :(";
                });
            });
            #endregion
        }

        #region Move Windows Region
        private void FlipRight()
        {
            Log.Logger.Information("JokesWindowViewModel: Нажата кнопка '>'");
            HostScreen.Router.Navigate.Execute(_servicesProvider.GetRequiredService<ImageProcessingViewModel>());
        }

        private void FlipLeft()
        {
            Log.Logger.Information("JokesWindowViewModel: Нажата кнопка '<'");
            _inputWindowViewModel.AreNavigationButtonsEnabled = true;
            HostScreen.Router.NavigateBack.Execute();
        }
        #endregion

        #region Jokes Functions Region
        private async Task GetRandomJoke()
        {
            IsLoading = true;
            try
            {
                await _hubConnectionWrapper.GetRandomJoke();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GetRandomTen()
        {
            IsLoading = true;
            try
            {
                await _hubConnectionWrapper.GetRandomTen();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GetRandomJokes()
        {
            IsLoading = true;
            try
            {
                await _hubConnectionWrapper.GetRandomJokes();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GetTenJokes()
        {
            IsLoading = true;
            try
            {
                await _hubConnectionWrapper.GetTenJokes();
            }
            finally
            {
                IsLoading = false;
            }
        }
        #endregion
    }
}
