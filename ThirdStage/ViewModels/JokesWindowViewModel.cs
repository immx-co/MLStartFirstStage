using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Serilog;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ThirdStage.ViewModels
{
    public class JokesWindowViewModel : ReactiveObject, IRoutableViewModel
    {
        #region Default Settings Region
        public IServiceProvider _servicesProvider;

        public IScreen HostScreen { get; }

        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
        #endregion

        #region Http Region
        public HttpClient client = new HttpClient();
        #endregion

        #region Avalonia Commands Region
        public ICommand FlipLeftCommand { get; init; }

        private string _displayedJoke = "There will be a random joke here)))";
        public string DisplayedJoke
        {
            get => _displayedJoke;
            set => this.RaiseAndSetIfChanged(ref _displayedJoke, value);
        }

        public ICommand RandomJokeCommand { get; init; }
        public ICommand RandomTenCommand { get; init; }
        public ICommand RandomJokesCommand { get; init; }
        public ICommand TenJokesCommand { get; init; }
        #endregion

        public JokesWindowViewModel(IScreen screen, IServiceProvider servicesProvider)
        {
            _servicesProvider = servicesProvider;
            HostScreen = screen;

            FlipLeftCommand = ReactiveCommand.Create(FlipLeft);

            RandomJokeCommand = ReactiveCommand.Create(GetRandomJoke);
            RandomTenCommand = ReactiveCommand.Create(GetRandomTen);
            RandomJokesCommand = ReactiveCommand.Create(GetRandomJokes);
            TenJokesCommand = ReactiveCommand.Create(GetTenJokes);
        }

        private void FlipLeft()
        {
            HostScreen.Router.Navigate.Execute(_servicesProvider.GetRequiredService<MainWindowViewModel>());
            Log.Logger.Information("Нажата кнопка '<'");
        }

        #region Jokes Functions Region
        private async Task GetRandomJoke()
        {
            string? joke = await GetRequestData("random_joke");
            if (joke != null)
            {
                DisplayedJoke = joke;
            }
            else
            {
                DisplayedJoke = "Ошибка при GetRandomJoke :(";
            }
        }

        private async Task GetRandomTen()
        {
            string? joke = await GetRequestData("random_ten");
            if (joke != null)
            {
                DisplayedJoke = joke;
            }
            else
            {
                DisplayedJoke = "Ошибка при GetRandomTen :(";
            }
        }

        private async Task GetRandomJokes()
        {
            string? joke = await GetRequestData("jokes/random");
            if (joke != null)
            {
                DisplayedJoke = joke;
            }
            else
            {
                DisplayedJoke = "Ошибка при GetRandomJokes :(";
            }
        }

        private async Task GetTenJokes()
        {
            string? joke = await GetRequestData("jokes/ten");
            if (joke != null)
            {
                DisplayedJoke = joke;
            }
            else
            {
                DisplayedJoke = "Ошибка при GetTenJokes :(";
            }
        }

        private async Task<string?> GetRequestData(string endpoint)
        {
            try
            {
                using HttpResponseMessage response = await client.GetAsync($"https://official-joke-api.appspot.com/{endpoint}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
        }
        #endregion
    }
}
