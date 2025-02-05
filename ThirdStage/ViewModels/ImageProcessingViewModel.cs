using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ClassLibrary;
using ReactiveUI;
using Serilog;
using SkiaSharp;
using System;
using System.IO;
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
        private string _savedInputUrl = string.Empty;

        private Bitmap? _selectedImage;
        public Bitmap? SelectedImage
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

        private string _imageInfo = string.Empty;
        public string ImageInfo
        {
            get => _imageInfo;
            set => this.RaiseAndSetIfChanged(ref _imageInfo, value);
        }
        #endregion

        #region Commands Region
        public ICommand FlipLeftCommand => HostScreen.Router.NavigateBack;

        public ReactiveCommand<Unit, Unit> SelectImageCommand { get; }
        public ReactiveCommand<Unit, Unit> ConnectToUrlCommand { get; }
        public ReactiveCommand<Unit, Unit> ProcessImageCommand { get; }
        #endregion

        public ImageProcessingViewModel(IScreen screen, IServiceProvider servicesProvider) : base(screen)
        {
            _servicesProvider = servicesProvider;
            Log.Logger = LoggerSetup.CreateLogger();

            SelectImageCommand = ReactiveCommand.CreateFromTask(SelectImageAsync);
            ConnectToUrlCommand = ReactiveCommand.CreateFromTask(ConnectToUrlAsync);
            ProcessImageCommand = ReactiveCommand.CreateFromTask(ProcessImage);

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
                SelectedImage = null;
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
                        _savedInputUrl = InputUrl;
                        IsButtonsEnabled(true);
                    }
                    else
                    {
                        Log.Logger.Information("Подключиться к нейросетевому сервису не удалось. StatusCode в json ответе != 200.");
                        HealthStatusColor = Brushes.Red;
                        SelectedImage = null;
                        ImageInfo = string.Empty;
                        IsButtonsEnabled(false);
                    }
                }
                else
                {
                    Log.Logger.Warning($"Не удалось обратиться к нейросетевому сервису по URL: {InputUrl}");
                    HealthStatusColor = Brushes.Red;
                    SelectedImage = null;
                    ImageInfo = string.Empty;
                    IsButtonsEnabled(false);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Warning($"Возникло непредвиденное исключение при попытке обратиться к нейросетевому сервису. ex: {ex}");
                HealthStatusColor = Brushes.Red;
                SelectedImage = null;
                ImageInfo = string.Empty;
                IsButtonsEnabled(false);
            }
        }

        private async Task ProcessImage()
        {
            if (SelectedImage == null || HealthStatusColor == Brushes.Red)
            {
                Log.Logger.Warning("Изображение не выбрано или нет подключения к нейросетевому сервису.");
                return;
            }

            try
            {
                using var httpClient = new HttpClient();
                using var content = new MultipartFormDataContent();

                var imageStream = new MemoryStream();
                SelectedImage.Save(imageStream);
                imageStream.Seek(0, SeekOrigin.Begin);

                var imageContent = new StreamContent(imageStream);
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                content.Add(imageContent, "image", "image.jpg");

                var response = await httpClient.PostAsync($"{_savedInputUrl}/resize_image", content);

                if (response.IsSuccessStatusCode)
                {
                    Log.Logger.Information("Изображение успешно отправлено на обработку.");

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ImageProcessingResponse>(jsonResponse);

                    if (result != null)
                    {
                        Log.Logger.Information($"Результат обработки: Width={result.Width}, Height={result.Height}, Channels={result.Channels}");
                        ImageInfo = $"Ширина: {result.Width}\nВысота: {result.Height}\nКоличество каналов: {result.Channels}";
                    }
                    else
                    {
                        Log.Logger.Warning("Не удалось десериализовать ответ от сервера.");
                        ImageInfo = "Ошибка: неверный формат ответа.";
                    }
                }
                else
                {
                    Log.Logger.Warning($"Ошибка при отправке изображения: {response.StatusCode}");
                    ImageInfo = $"Ошибка при отправке изображения: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"Ошибка при отправке изображения. ex: {ex.Message}");
                ImageInfo = $"Ошибка: {ex.Message}";
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

        private class ImageProcessingResponse
        {
            [JsonPropertyName("width")]
            public int Width { get; set; }

            [JsonPropertyName("height")]
            public int Height { get; set; }

            [JsonPropertyName("status_code")]
            public int Channels { get; set; }
        }
    }
}
