using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text.RegularExpressions;
using ClassLibrary.Database;
using ClassLibrary.Database.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.SignalR.Client;

namespace ThirdStage.ViewModels
{
    public class RegistrationViewModel : BaseAuthRegisterViewModel
    {
        public ReactiveCommand<Unit, Unit> RegistrationCommand { get; }

        HubConnectionWrapper _hubConnectionWrapper;

        public RegistrationViewModel(IScreen screen, IConfiguration configuration, PasswordHasher hasher, IServiceProvider servicesProvider, HubConnectionWrapper hubConnectionWrapper): base(screen, configuration, hasher, servicesProvider)
        {
            _hubConnectionWrapper = hubConnectionWrapper;

            _hubConnectionWrapper.Connection.On("TooShortNickname", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    string wrongNickname = Nickname;
                    Nickname = string.Empty;
                    Password = string.Empty;
                    Email = string.Empty;
                    ShowMessageBox($"Invalid username: {wrongNickname}", "Имя пользователя не может состоять меньше чем из 3 символов! Попробуйте еще раз.");
                    Log.Logger.Warning($"Имя пользователя не может состоять меньше чем из 3 символов: {wrongNickname}.");
                    return;
                });
            });

            _hubConnectionWrapper.Connection.On("WrongPattern", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    string wrongNickname = Nickname;
                    Nickname = string.Empty;
                    ShowMessageBox($"Invalid username: {wrongNickname}", $"Имя пользователя {wrongNickname} содержит недопустимые символы! Попробуйте еще раз.");
                    Log.Logger.Warning($"Имя пользователя {wrongNickname} содержит недопустимые символы.");
                    return;
                });
            });

            _hubConnectionWrapper.Connection.On("TooShortPassword", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    Password = string.Empty;
                    ShowMessageBox("Invalid password", "Пароль должен быть не менее 3х символов! Попробуйте еще раз.");
                    Log.Logger.Warning("Пароль должен быть не менее 3х символов.");
                    return;
                });
            });

            _hubConnectionWrapper.Connection.On("UserExists", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    ShowMessageBox($"Invalid username {Nickname}", $"Пользователь {Nickname} уже существует! Попробуйте выбрать другое имя!");
                    Log.Logger.Warning($"Пользователь {Nickname} уже существует");
                    Nickname = string.Empty;
                    return;
                });
            });

            _hubConnectionWrapper.Connection.On("InvalidEmail", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    ShowMessageBox($"Invalid email address {Email}", $"Email адрес не существует! Попробуйте ввести email адрес еще раз.");
                    Log.Logger.Warning($"Email адрес {Email} не существует!");
                    Email = string.Empty;
                    return;
                });
            });

            _hubConnectionWrapper.Connection.On("FailedMailSendMessage", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    ShowMessageBox("Failed", "Не удалось отправить письмо для подтверждения учетной записи.");
                });
            });

            _hubConnectionWrapper.Connection.On("OkRegistration", () =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    ShowMessageBoxSuccessRegistration(Nickname);
                    Log.Logger.Debug($"Пользователь {Nickname} зарегистрирован успешно.");
                    Nickname = string.Empty;
                    Password = string.Empty;
                    HostScreen.Router.Navigate.Execute(_servicesProvider.GetRequiredService<AutorizationWindowViewModel>());
                });
            });

            RegistrationCommand = ReactiveCommand.Create(Registration);
        }

        /// <summary>
        /// Метод регистрации пользователя.
        /// </summary>
        private async void Registration()
        {
            await _hubConnectionWrapper.Registration(Nickname, Password, Email, nicknamePattern);
        }

        /// <summary>
        /// Сообщение, показывающееся после успешной регистрации пользователя.
        /// </summary>
        /// <param name="username"></param>
        private void ShowMessageBoxSuccessRegistration(string username)
        {
            this.ShowMessageBox("Success", $"Регистрация пользователя {username} прошла успешно!");
        }
    }
}
