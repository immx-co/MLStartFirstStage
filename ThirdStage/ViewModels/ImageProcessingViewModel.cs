using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ClassLibrary;
using ReactiveUI;
using Serilog;
using System;
using System.Net.Http;
using System.Reactive;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ThirdStage.ViewModels
{
    public class ImageProcessingViewModel : BaseMainWindowViewModel
    {
        #region Default Settings Region
        public IServiceProvider _servicesProvider;

        #endregion

        #region Property Region
        private string _inputUrl;
        public string InputUrl
        {
            get => _inputUrl;
            set => this.RaiseAndSetIfChanged(ref _inputUrl, value);
        }

        private Bitmap _selectedImage;
        public Bitmap SelectedImage
        {
            get => _selectedImage;
            set => this.RaiseAndSetIfChanged(ref _selectedImage, value);
        }

        private IBrush _healthStatusColor;
        public IBrush HealthStatusColor
        {
            get => _healthStatusColor;
            set => this.RaiseAndSetIfChanged(ref _healthStatusColor, value);
        }

        private bool _isSelectImageButtonEnabled;
        public bool IsSelectImageButtonEnabled
        {
            get => _isSelectImageButtonEnabled;
            set => this.RaiseAndSetIfChanged(ref _isSelectImageButtonEnabled, value);
        }

        private bool _isSendImageButtonEnabled;
        public bool IsSendImageButtonEnabled
        {
            get => _isSendImageButtonEnabled;
            set => this.RaiseAndSetIfChanged(ref _isSendImageButtonEnabled, value);
        }
        #endregion

        #region Commands Region
        public ICommand FlipLeftCommand => HostScreen.Router.NavigateBack;

        public ReactiveCommand<Unit, Unit> SelectImageCommand { get; }
        public ReactiveCommand<Unit, Unit> ConnectToUrlCommand { get; }
        #endregion

        public ImageProcessingViewModel(IScreen screen, IServiceProvider servicesProvider) : base(screen)
        {
            _servicesProvider = servicesProvider;
            Log.Logger = LoggerSetup.CreateLogger();

            SelectImageCommand = ReactiveCommand.CreateFromTask(SelectImageAsync);
            ConnectToUrlCommand = ReactiveCommand.CreateFromTask(ConnectToUrlAsync);

            HealthStatusColor = Brushes.Gray;
            IsSelectImageButtonEnabled = false;
            IsSendImageButtonEnabled = false;
        }

        #region Button Processing Functions
        private async Task SelectImageAsync()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Выберите изображение",
                Filters = { new FileDialogFilter { Name = "Images", Extensions = { "png", "jpg", "jpeg", "bmp" } } },
                AllowMultiple = false
            };

            var result = await openFileDialog.ShowAsync(new Window());

            if (result != null && result.Length > 0)
            {
                var filePath = result[0];
                SelectedImage = new Bitmap(filePath);
            }
        }

        private async Task ConnectToUrlAsync()
        {
            IsButtonsEnabled(false);
            HealthStatusColor = Brushes.Gray;
            if (string.IsNullOrEmpty(InputUrl))
            {
                HealthStatusColor = Brushes.Red;
                return;
            }

            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{InputUrl}/health");
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<HealthCheckResponse>(json);

                    if (result?.StatusCode == 200)
                    {
                        Log.Logger.Information("Успешное подключение к нейросетевому сервису.");
                        HealthStatusColor = Brushes.Green;
                        IsButtonsEnabled(true);
                    }
                    else
                    {
                        Log.Logger.Information("Подключиться к нейросетевому сервису не удалось. StatusCode в json ответе != 200.");
                        HealthStatusColor = Brushes.Red;
                        IsButtonsEnabled(false);
                    }
                }
                else
                {
                    Log.Logger.Warning($"Не удалось обратиться к нейросетевому сервису по URL: {InputUrl}");
                    HealthStatusColor = Brushes.Red;
                    IsButtonsEnabled(false);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Warning($"Возникло непредвиденное исключение при попытке обратиться к нейросетевому сервису. ex: {ex}");
                HealthStatusColor = Brushes.Red;
                IsButtonsEnabled(false);
            }
        }

        private void IsButtonsEnabled(bool buttonsState)
        {
            IsSelectImageButtonEnabled = buttonsState;
            IsSendImageButtonEnabled = buttonsState;
        }
        #endregion

        private class HealthCheckResponse
        {
            [JsonPropertyName("status_code")]
            public int StatusCode { get; set; }

            [JsonPropertyName("datetime")]
            public DateTime Datetime {  get; set; }
        }
    }
}
