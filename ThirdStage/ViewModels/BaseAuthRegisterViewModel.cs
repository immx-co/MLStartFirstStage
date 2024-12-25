using ClassLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MsBox.Avalonia;
using ReactiveUI;
using Serilog;
using System;
using System.Linq;
using System.Reactive;
using ThirdStage.Database;

namespace ThirdStage.ViewModels
{
    public class BaseAuthRegisterViewModel : ReactiveObject, IRoutableViewModel
    {
        public IScreen HostScreen { get; }

        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public DbContextOptionsBuilder<ApplicationContext> optionsBuilder;

        public IConfiguration configuration;

        public readonly PasswordHasher _hasher;

        private string _nickname;
        private string _password;
        private string _email;

        public string Nickname
        {
            get => _nickname;
            set => this.RaiseAndSetIfChanged(ref _nickname, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        public string nicknamePattern = @"^[а-яА-Яa-zA-Z0-9]+$";

        /// <summary>
        /// Конструктор класса AutorizationWindowViewModel.
        /// </summary>
        /// <param name="configuration">Конфигурация приложения.</param>
        /// <param name="openMainWindow">Делегат открытия главного окна.</param>
        /// <param name="closeThisWindow">Делегат закрытия окна.</param>
        public BaseAuthRegisterViewModel(IScreen screen, IConfiguration configuration)
        {
            this.configuration = configuration;

            HostScreen = screen;
            Log.Logger = LoggerSetup.CreateLogger();

            optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseNpgsql(configuration["stringConnection"]);

            _hasher = new PasswordHasher();
        }

        /// <summary>
        /// Показывает предупреждающее сообщение, если что-то пошло не так.
        /// </summary>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <param name="message">Сообщение пользователю.</param>
        public void ShowMessageBox(string caption, string message)
        {
            var messageBoxStandardWindow = MessageBoxManager.GetMessageBoxStandard(caption, message);
            messageBoxStandardWindow.ShowAsync();
        }

        /// <summary>
        /// Сообщение, показывающееся после успешной регистрации пользователя.
        /// </summary>
        /// <param name="username"></param>
        private void ShowMessageBoxSuccessRegistration(string username)
        {
            ShowMessageBox("Success", $"Регистрация пользователя {username} прошла успешно!");
        }
    }
}
