using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Serilog;
using System;
using System.Reactive;
using ClassLibrary.Database;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace ThirdStage.ViewModels;

public partial class AutorizationWindowViewModel : BaseAuthRegisterViewModel
{
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }

    private string _activeUsername = "";
    public string ActiveUsername
    {
        get => _activeUsername;
        set => this.RaiseAndSetIfChanged(ref _activeUsername, value);
    }

    HubConnectionWrapper _hubConnectionWrapper;

    public AutorizationWindowViewModel(IScreen screen, IConfiguration configuration, PasswordHasher hasher, IServiceProvider servicesProvider, HubConnectionWrapper hubConnectionWrapper) : base(screen, configuration, hasher, servicesProvider)
    {
        _hubConnectionWrapper = hubConnectionWrapper;

        _hubConnectionWrapper.Connection.On("UserNotFound", () =>
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                Nickname = string.Empty;
                Password = string.Empty;
                ShowMessageBox("Invalid username", "Такого имени пользователя не существует!");
                Log.Logger.Warning($"Имени пользователя не существует: {Nickname}");
                return;
            });
        });

        _hubConnectionWrapper.Connection.On("InvalidPassword", () =>
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                this.Password = string.Empty;
                ShowMessageBox("Invalid password", "Неверный пароль! Попробуйте еще раз.");
                Log.Logger.Warning($"Неверный пароль у {Nickname}");
                return;
            });
        });

        _hubConnectionWrapper.Connection.On("OkAuthorization", () =>
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                ActiveUsername = Nickname;
                HostScreen.Router.Navigate.Execute(_servicesProvider.GetRequiredService<MainWindowViewModel>());
                Nickname = string.Empty;
                Password = string.Empty;
            });
        });

        LoginCommand = ReactiveCommand.CreateFromTask(Login);
    }

    /// <summary>
    /// Вход в приложение по пользователю и паролю.
    /// </summary>
    private async Task Login()
    {
        await _hubConnectionWrapper.Login(Nickname, Password);
    }
}
