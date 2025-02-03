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

namespace ThirdStage.ViewModels
{
    public class RegistrationViewModel : BaseAuthRegisterViewModel
    {
        public ReactiveCommand<Unit, Unit> RegistrationCommand { get; }

        public RegistrationViewModel(IScreen screen, IConfiguration configuration, PasswordHasher hasher, IServiceProvider servicesProvider) : base(screen, configuration, hasher, servicesProvider)
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

            using ApplicationContext db = _servicesProvider.GetRequiredService<ApplicationContext>();
            List<User> dbUsers = db.Users.ToList();
            if (dbUsers.Any(user => user.Name == Nickname))
            {
                ShowMessageBox($"Invalid username {Nickname}", $"Пользователь {Nickname} уже существует! Попробуйте выбрать другое имя!");
                Log.Logger.Warning($"Пользователь {Nickname} уже существует");
                Nickname = string.Empty;
                return;
            }

            if (!IsValidEmail(Email))
            {
                ShowMessageBox($"Invalid email address {Email}", $"Email адрес не существует! Попробуйте ввести email адрес еще раз.");
                Log.Logger.Warning($"Email адрес {Email} не существует!");
                Email = string.Empty;
                return;
            }

            string hashedPassword = _hasher.HashPassword(Password);
            Log.Logger.Verbose("Пароль захеширован успешно.");
            User authUser = new User { Name = Nickname, HashPassword = hashedPassword, Email = Email, EmailConfirmationToken = GenerateEmailConfirmationToken() };
            SendConfirmationEmail(authUser.Email, authUser.Name, authUser.EmailConfirmationToken);
            db.Users.AddRange(authUser);
            db.SaveChanges();
            ShowMessageBoxSuccessRegistration(Nickname);
            Log.Logger.Debug($"Пользователь {Nickname} зарегистрирован успешно.");
            
            Nickname = string.Empty;
            Password = string.Empty;
            
            HostScreen.Router.Navigate.Execute(_servicesProvider.GetRequiredService<AutorizationWindowViewModel>());
        }

        /// <summary>
        /// Сообщение, показывающееся после успешной регистрации пользователя.
        /// </summary>
        /// <param name="username"></param>
        private void ShowMessageBoxSuccessRegistration(string username)
        {
            this.ShowMessageBox("Success", $"Регистрация пользователя {username} прошла успешно!");
        }

        #region Email Confirmation Region
        /// <summary>
        /// Проверяет, валидный ли email адрес.
        /// </summary>
        /// <param name="email">Email, введенный пользователем.</param>
        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        /// <summary>
        /// Генерирует подтверждающий токен.
        /// </summary>
        /// <returns></returns>
        public static string GenerateEmailConfirmationToken()
        {
            var token = Guid.NewGuid().ToString("N");
            return token;
        }

        /// <summary>
        /// Отправляет Email для верификации аккаунта при успешной регистрации пользователя.
        /// </summary>
        /// <param name="email">Email пользователя.</param>
        /// <param name="username">Зарегестрированное имя пользователя.</param>
        /// <param name="token">Токен подтверждения.</param>
        public void SendConfirmationEmail(string email, string username, string token)
        {
            IConfigurationSection smtpSettings = _servicesProvider.GetRequiredService<IConfiguration>().GetSection("SmtpSettings");

            var smtpClient = new SmtpClient(smtpSettings["Server"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
            };

            var confirmationLink = $"https://localhost:7576/api/EmailConfirmation/confirmEmail?username={username}&token={token}";

            var mailMessage = new MailMessage
            {
                From = new MailAddress("immxxx@yandex.ru"),
                Subject = "Подтверждение email",
                Body = $"Пожалуйста, подтвердите ваш email, чтобы полностью активировать возможности приложения, перейдя по ссылке: {confirmationLink}",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (SmtpException)
            {
                ShowMessageBox("Failed", "Не удалось отправить письмо для подтверждения учетной записи.");
            }
            
        }
        #endregion
    }
}
