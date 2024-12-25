using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text.RegularExpressions;
using ThirdStage.Database;

namespace ThirdStage.ViewModels
{
    public class RegistrationViewModel : BaseAuthRegisterViewModel
    {
        public ReactiveCommand<Unit, Unit> RegistrationCommand { get; }

        public RegistrationViewModel(IScreen screen, IConfiguration configuration) : base(screen, configuration)
        {
            RegistrationCommand = ReactiveCommand.Create(Registration);
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
                Email = string.Empty;
                ShowMessageBox($"Invalid username: {wrongNickname}", "Имя пользователя не может состоять меньше чем из 3 символов! Попробуйте еще раз.");
                Log.Logger.Warning($"Имя пользователя не может состоять меньше чем из 3 символов: {wrongNickname}.");
                return;
            }
            if (!Regex.IsMatch(Nickname, nicknamePattern))
            {
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
            User authUser = new User { Name = Nickname, HashPassword = hashedPassword, Email = Email };
            db.Users.AddRange(authUser);
            db.SaveChanges();
            ShowMessageBoxSuccessRegistration(Nickname);
            Log.Logger.Debug($"Пользователь {Nickname} зарегистрирован успешно.");
            Nickname = string.Empty;
            Password = string.Empty;
            HostScreen.Router.Navigate.Execute(new AutorizationWindowViewModel(HostScreen, configuration));
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
