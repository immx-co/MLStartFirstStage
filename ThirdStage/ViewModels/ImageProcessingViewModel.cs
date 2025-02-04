using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ThirdStage.ViewModels
{
    public class ImageProcessingViewModel : ReactiveObject, IRoutableViewModel
    {
        #region Default Settings Region
        public IServiceProvider _servicesProvider;

        public IScreen HostScreen { get; }

        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
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
        #endregion

        #region Commands Region
        public ICommand FlipLeftCommand => HostScreen.Router.NavigateBack;

        public ReactiveCommand<Unit, Unit> SelectImageCommand { get; }
        #endregion

        public ImageProcessingViewModel(IScreen screen, IServiceProvider servicesProvider)
        {
            _servicesProvider = servicesProvider;
            HostScreen = screen;

            SelectImageCommand = ReactiveCommand.CreateFromTask(SelectImageAsync);
        }

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
    }
}
