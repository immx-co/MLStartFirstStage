using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Serilog;
using System.Linq;
using System.Reactive;
using ThirdStage.Database;

namespace ThirdStage.ViewModels;

public partial class AutorizationWindowViewModel : BaseAuthRegisterViewModel
{
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }

    public AutorizationWindowViewModel(IScreen screen, IConfiguration configuration, PasswordHasher hasher) : base(screen, configuration, hasher)
    {
        LoginCommand = ReactiveCommand.Create(Login);
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
            this.Password = string.Empty;
            ShowMessageBox("Invalid password", "Неверный пароль! Попробуйте еще раз.");
            Log.Logger.Warning($"Неверный пароль у {Nickname}");
            return;
        }
        HostScreen.Router.Navigate.Execute(new MainWindowViewModel(HostScreen));
        //HostScreen.Router.Navigate.Execute(servicesProvider);
    }
}
