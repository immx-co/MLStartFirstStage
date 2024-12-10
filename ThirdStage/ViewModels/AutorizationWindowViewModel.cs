using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using ThirdStage.Database;

namespace ThirdStage.ViewModels;

public partial class AutorizationWindowViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> OpenMainWindowCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseWindowCommand { get; }
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }
    public ReactiveCommand<Unit, Unit> AutorizationCommand { get; }

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

    public AutorizationWindowViewModel(Action openMainWindow, Action closeThisWindow)
    {
        _openMainWindow = openMainWindow;
        _closeWindow = closeThisWindow;
        OpenMainWindowCommand = ReactiveCommand.Create(OpenMainWindow);
        CloseWindowCommand = ReactiveCommand.Create(CloseWindow);
        LoginCommand = ReactiveCommand.Create(Login);
        AutorizationCommand = ReactiveCommand.Create(Autorization);

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
            return;
        }
        bool unHashedPassword = _hasher.VerifyPassword(Password, dbUser.HashPassword);
        if (!unHashedPassword)
        {
            Nickname = string.Empty;
            Password = string.Empty;
            return;
        }
        OpenMainWindow();
    }
    
    private void Autorization()
    {
        using ApplicationContext db = new ApplicationContext();
        string hashedPassword = _hasher.HashPassword(Password);
        User authUser = new User { Name = Nickname, HashPassword = hashedPassword };
        db.Users.AddRange(authUser);
        db.SaveChanges();
        OpenMainWindow();
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
