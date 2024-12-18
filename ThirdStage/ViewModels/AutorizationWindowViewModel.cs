using MsBox.Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ThirdStage.Database;
using System.Text.RegularExpressions;
using System.Reactive.Joins;

namespace ThirdStage.ViewModels;

public partial class AutorizationWindowViewModel : ViewModelBase
{
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


    public AutorizationWindowViewModel(Action openMainWindow, Action closeThisWindow)
    {
        _openMainWindow = openMainWindow;
        _closeWindow = closeThisWindow;
        OpenMainWindowCommand = ReactiveCommand.Create(OpenMainWindow);
        CloseWindowCommand = ReactiveCommand.Create(CloseWindow);
        LoginCommand = ReactiveCommand.Create(Login);
        RegistrationCommand = ReactiveCommand.Create(Registration);

        _hasher = new PasswordHasher();
    }

    private void Login()
    {
        using ApplicationContext db = new ApplicationContext();
        User? dbUser = db.Users.SingleOrDefault(user => user.Name == Nickname);
        if (dbUser is null)
        {
            Nickname = string.Empty;
            Password = string.Empty;
            ShowMessageBox("Invalid username", "Такого имени пользователя не существует!");
            return;
        }
        bool unHashedPassword = _hasher.VerifyPassword(Password, dbUser.HashPassword);
        if (!unHashedPassword)
        {
            Password = string.Empty;
            ShowMessageBox("Invalid password", "Неверный пароль! Попробуйте еще раз.");
            return;
        }
        OpenMainWindow();
    }
    
    private void Registration()
    {
        if (Nickname.Length < 3)
        {
            string wrongNickname = Nickname;
            Nickname = string.Empty;
            Password = string.Empty;
            ShowMessageBox($"Invalid username: {wrongNickname}", "Имя пользователя не может состоять меньше чем из 3 символов! Попробуйте еще раз.");
            return;
        }
        if (!Regex.IsMatch(Nickname, nicknamePattern)) {
            string wrongNickname = Nickname;
            Nickname = string.Empty;
            ShowMessageBox($"Invalid username: {wrongNickname}", $"Имя пользователя {wrongNickname} содержит недопустимые символы! Попробуйте еще раз.");
            return;
        }
        if (Password.Length < 3)
        {
            Password = string.Empty;
            ShowMessageBox("Invalid password", "Пароль должен быть не менее 3х символов! Попробуйте еще раз.");
            return;
        }
        using ApplicationContext db = new ApplicationContext();
        List<User> dbUsers = db.Users.ToList();
        if (dbUsers.Any(user => user.Name == Nickname))
        {
            ShowMessageBox($"Invalid username {Nickname}", $"Пользователь {Nickname} уже существует! Попробуйте выбрать другое имя!");
            Nickname = string.Empty;
            return;
        }
        string hashedPassword = _hasher.HashPassword(Password);
        User authUser = new User { Name = Nickname, HashPassword = hashedPassword };
        db.Users.AddRange(authUser);
        db.SaveChanges();
        ShowMessageBoxSuccessRegistration(Nickname);
    }

    private void ShowMessageBox(string caption, string message)
    {
        var messageBoxStandardWindow = MessageBoxManager.GetMessageBoxStandard(caption, message);
        messageBoxStandardWindow.ShowAsync();
    }

    private void ShowMessageBoxSuccessRegistration(string username)
    {
        ShowMessageBox("Success", $"Регистрация пользователя {username} прошла успешно!");
    }

    private void OpenMainWindow()
    {
        _openMainWindow?.Invoke();
    }

    private void CloseWindow()
    {
        _closeWindow?.Invoke();
    }
}
