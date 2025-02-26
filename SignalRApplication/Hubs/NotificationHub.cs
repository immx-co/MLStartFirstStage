using ClassLibrary.Database;
using ClassLibrary.Database.Models;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using System.Diagnostics.Tracing;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SignalRApplication.Hubs;

public class NotificationHub : Hub
{
    IServiceProvider _serviceProvider;
    PasswordHasher _hasher;

    public NotificationHub(IServiceProvider serviceProvider, PasswordHasher hasher)
    {
        _serviceProvider = serviceProvider;
        _hasher = hasher;
    }

    public Task Login(string nickName, string password)
    {
        using ApplicationContext db = _serviceProvider.GetRequiredService<ApplicationContext>();

        User? dbUser = db.Users.SingleOrDefault(user => user.Name == nickName);
        if (dbUser is null)
        {
            return Clients.Caller.SendAsync("UserNotFound");
        }

        bool unHashedPassword = _hasher.VerifyPassword(password, dbUser.HashPassword);
        if (!unHashedPassword)
        {
            return Clients.Caller.SendAsync("InvalidPassword");
        }

        return Clients.Caller.SendAsync("OkAuthorization");
    }

    public async Task Registration(string nickName, string password, string email, string nickNamePattern)
    {
        using ApplicationContext db = _serviceProvider.GetRequiredService<ApplicationContext>();

        if (nickName.Length < 3)
        {
            await Clients.Caller.SendAsync("TooShortNickname");
            return;
        }

        if (!Regex.IsMatch(nickName, nickNamePattern))
        {
            await Clients.Caller.SendAsync("WrongPattern");
            return;
        }

        if (password.Length < 3)
        {
            await Clients.Caller.SendAsync("TooShortPassword");
            return;
        }

        List<User> dbUsers = db.Users.ToList();
        if (dbUsers.Any(user => user.Name == nickName))
        {
            await Clients.Caller.SendAsync("UserExists");
            return;
        }

        if (!IsValidEmail(email))
        {
            await Clients.Caller.SendAsync("InvalidEmail");
            return;
        }

        string hashedPassword = _hasher.HashPassword(password);
        User authUser = new User { Name = nickName, HashPassword = hashedPassword, Email = email, EmailConfirmationToken = GenerateEmailConfirmationToken() };
        try
        {
            SendConfirmationEmail(authUser.Email, authUser.Name, authUser.EmailConfirmationToken);
        }
        catch (SmtpException)
        {
            await Clients.Caller.SendAsync("FailedMailSendMessage");
        }
        db.Users.AddRange(authUser);
        db.SaveChanges();
        await Clients.Caller.SendAsync("OkRegistration");
        return;
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
    /// Скидывает ссылку для верификации аккаунта на введенную пользователем почту.
    /// </summary>
    /// <param name="email">Почта.</param>
    /// <param name="username">Имя пользователя.</param>
    /// <param name="token">Уникальный токен подтверждения.</param>
    public void SendConfirmationEmail(string email, string username, string token)
    {
        IConfigurationSection smtpSettings = _serviceProvider.GetRequiredService<IConfiguration>().GetSection("SmtpSettings");

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
        smtpClient.Send(mailMessage);
    }
    #endregion

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
        await base.OnDisconnectedAsync(exception);
    }
}
