using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MsBox.Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ClassLibrary;
using System.Text.RegularExpressions;
using ThirdStage.Database;
using Serilog;

namespace ThirdStage.ViewModels;

public partial class AutorizationWindowViewModel : ViewModelBase
{
    DbContextOptionsBuilder<ApplicationContext> optionsBuilder;

    public ReactiveCommand<Unit, Unit> OpenMainWindowCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseWindowCommand { get; }
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }
    public ReactiveCommand<Unit, Unit> RegistrationCommand { get; }

    private readonly PasswordHasher _hasher;
    private readonly Action _openMainWindow;
    private readonly Action _closeWindow;

    private string _nickname;
    private string _password;

    public string Nickname
    {
        get => _nickname;
        set
        {
            _nickname = value;
            OnPropertyChanged(nameof(Nickname));
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    string nicknamePattern = @"^[а-яА-Яa-zA-Z0-9]+$";

    /// <summary>
    /// Конструктор класса AutorizationWindowViewModel.
    /// </summary>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="openMainWindow">Делегат открытия главного окна.</param>
    /// <param name="closeThisWindow">Делегат закрытия окна.</param>
    public AutorizationWindowViewModel(IConfiguration configuration, Action openMainWindow, Action closeThisWindow)
    {
        Log.Logger = LoggerSetup.CreateLogger();

        optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseNpgsql(configuration["stringConnection"]);

        _openMainWindow = openMainWindow;
        _closeWindow = closeThisWindow;
        OpenMainWindowCommand = ReactiveCommand.Create(OpenMainWindow);
        CloseWindowCommand = ReactiveCommand.Create(CloseWindow);
        LoginCommand = ReactiveCommand.Create(Login);
        RegistrationCommand = ReactiveCommand.Create(Registration);

        _hasher = new PasswordHasher();
    }

    /// <summary>
    /// Вход в приложение по пользователю и паролю.
    /// </summary>
    private void Login()
    {
        using ApplicationContext db = new ApplicationContext(optionsBuilder.Options);
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
            Password = string.Empty;
            ShowMessageBox("Invalid password", "Неверный пароль! Попробуйте еще раз.");
            Log.Logger.Warning($"Неверный пароль у {Nickname}");
            return;
        }
        OpenMainWindow();
    }
    
    /// <summary>
    /// Метод регистрации пользователя.
    /// </summary>
    private void Registration()
    {
        if (Nickname.Length < 3)
        {
            string wrongNickname = Nickname;
            Nickname = string.Empty;
            Password = string.Empty;
            ShowMessageBox($"Invalid username: {wrongNickname}", "Имя пользователя не может состоять меньше чем из 3 символов! Попробуйте еще раз.");
            Log.Logger.Warning($"Имя пользователя не может состоять меньше чем из 3 символов: {wrongNickname}.");
            return;
        }
        if (!Regex.IsMatch(Nickname, nicknamePattern)) {
            string wrongNickname = Nickname;
            Nickname = string.Empty;
            ShowMessageBox($"Invalid username: {wrongNickname}", $"Имя пользователя {wrongNickname} содержит недопустимые символы! Попробуйте еще раз.");
            Log.Logger.Warning($"Имя пользователя {wrongNickname} содержит недопустимые символы.");
            return;
        }
        if (Password.Length < 3)
        {
            Password = string.Empty;
            ShowMessageBox("Invalid password", "Пароль должен быть не менее 3х символов! Попробуйте еще раз.");
            Log.Logger.Warning("Пароль должен быть не менее 3х символов.");
            return;
        }
        using ApplicationContext db = new ApplicationContext(optionsBuilder.Options);
        List<User> dbUsers = db.Users.ToList();
        if (dbUsers.Any(user => user.Name == Nickname))
        {
            ShowMessageBox($"Invalid username {Nickname}", $"Пользователь {Nickname} уже существует! Попробуйте выбрать другое имя!");
            Log.Logger.Warning($"Пользователь {Nickname} уже существует");
            Nickname = string.Empty;
            return;
        }
        string hashedPassword = _hasher.HashPassword(Password);
        Log.Logger.Verbose("Пароль захеширован успешно.");
        User authUser = new User { Name = Nickname, HashPassword = hashedPassword };
        db.Users.AddRange(authUser);
        db.SaveChanges();
        ShowMessageBoxSuccessRegistration(Nickname);
        Log.Logger.Debug($"Пользователь {Nickname} зарегистрирован успешно.");
        Nickname = string.Empty;
        Password = string.Empty;
    }

    /// <summary>
    /// Показывает предупреждающее сообщение, если что-то пошло не так.
    /// </summary>
    /// <param name="caption">Заголовок сообщения.</param>
    /// <param name="message">Сообщение пользователю.</param>
    private void ShowMessageBox(string caption, string message)
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

    /// <summary>
    /// Открывает главное окно.
    /// </summary>
    private void OpenMainWindow()
    {
        _openMainWindow?.Invoke();
    }

    /// <summary>
    /// Закрывает окно авторизации.
    /// </summary>
    private void CloseWindow()
    {
        _closeWindow?.Invoke();
    }
}
