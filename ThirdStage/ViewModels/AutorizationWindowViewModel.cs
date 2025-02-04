﻿using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Serilog;
using System;
using System.Linq;
using System.Reactive;
using ClassLibrary.Database;
using ClassLibrary.Database.Models;
using Microsoft.Extensions.DependencyInjection;

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

    public AutorizationWindowViewModel(IScreen screen, IConfiguration configuration, PasswordHasher hasher, IServiceProvider servicesProvider) : base(screen, configuration, hasher, servicesProvider)
    {
        LoginCommand = ReactiveCommand.Create(Login);
    }

    /// <summary>
    /// Вход в приложение по пользователю и паролю.
    /// </summary>
    private void Login()
    {
        using ApplicationContext db = _servicesProvider.GetRequiredService<ApplicationContext>();

        User? dbUser = db.Users.SingleOrDefault(user => user.Name == Nickname);
        if (dbUser is null)
        {
            Nickname = string.Empty;
            Password = string.Empty;
            ShowMessageBox("Invalid username", "Такого имени пользователя не существует!");
            Log.Logger.Warning($"Имени пользователя не существует: {Nickname}");
            return;
        }
        bool unHashedPassword = _hasher.VerifyPassword(Password, dbUser.HashPassword);
        if (!unHashedPassword)
        {
            this.Password = string.Empty;
            ShowMessageBox("Invalid password", "Неверный пароль! Попробуйте еще раз.");
            Log.Logger.Warning($"Неверный пароль у {Nickname}");
            return;
        }
        ActiveUsername = Nickname;
        HostScreen.Router.Navigate.Execute(_servicesProvider.GetRequiredService<MainWindowViewModel>());
        Nickname = string.Empty;
        Password = string.Empty;
    }
}
