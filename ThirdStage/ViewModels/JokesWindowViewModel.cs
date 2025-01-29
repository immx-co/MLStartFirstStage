using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.Json;
using System.Collections.Generic;
using ThirdStage.Database.Models;
using ThirdStage.Database;
using System.Linq;

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
        public ICommand FlipLeftCommand => HostScreen.Router.NavigateBack;

        private string _displayedJoke = "There will be a random joke here.";
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

            RandomJokeCommand = ReactiveCommand.Create(GetRandomJoke);
            RandomTenCommand = ReactiveCommand.Create(GetRandomTen);
            RandomJokesCommand = ReactiveCommand.Create(GetRandomJokes);
            TenJokesCommand = ReactiveCommand.Create(GetTenJokes);
        }

        #region Jokes Functions Region
        private async Task GetRandomJoke()
        {
            string? deserializedJoke = await GetRequestData("random_joke");
            if (deserializedJoke != null)
            {
                Joke joke = JsonSerializer.Deserialize<Joke>(deserializedJoke);

                using ApplicationContext db = _servicesProvider.GetRequiredService<ApplicationContext>();

                DisplayedJoke = $"{joke.Setup} {joke.Punchline}";
                db.Jokes.AddRange(joke);
                db.SaveChanges();
            }
            else
            {
                DisplayedJoke = "Ошибка при GetRandomJoke :(";
            }
        }

        private async Task GetRandomTen()
        {
            string? deserializedJokes = await GetRequestData("random_ten");
            if (deserializedJokes != null)
            {
                List<Joke> jokes = JsonSerializer.Deserialize<List<Joke>>(deserializedJokes);

                DisplayedJoke = string.Empty;
                using ApplicationContext db = _servicesProvider.GetRequiredService<ApplicationContext>();
                foreach (var (joke, index) in jokes.Select((joke, index) => (joke, index)))
                {
                    DisplayedJoke += $"{index + 1}. {joke.Setup} {joke.Punchline}\n\n";
                    db.Jokes.AddRange(joke);
                }
                db.SaveChanges();
            }
            else
            {
                DisplayedJoke = "Ошибка при GetRandomTen :(";
            }
        }

        private async Task GetRandomJokes()
        {
            string? deserializedJoke = await GetRequestData("jokes/random");
            if (deserializedJoke != null)
            {
                Joke joke = JsonSerializer.Deserialize<Joke>(deserializedJoke);

                using ApplicationContext db = _servicesProvider.GetRequiredService<ApplicationContext>();

                DisplayedJoke = $"{joke.Setup} {joke.Punchline}";
                db.Jokes.AddRange(joke);
                db.SaveChanges();
            }
            else
            {
                DisplayedJoke = "Ошибка при GetRandomJokes :(";
            }
        }

        private async Task GetTenJokes()
        {
            string? deserializedJokes = await GetRequestData("jokes/ten");
            if (deserializedJokes != null)
            {
                List<Joke> jokes = JsonSerializer.Deserialize<List<Joke>>(deserializedJokes);

                DisplayedJoke = string.Empty;
                using ApplicationContext db = _servicesProvider.GetRequiredService<ApplicationContext>();
                foreach (var (joke, index) in jokes.Select((joke, index) => (joke, index)))
                {
                    DisplayedJoke += $"{index + 1}. {joke.Setup} {joke.Punchline}\n\n";
                    db.Jokes.AddRange(joke);
                }
                db.SaveChanges();
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
