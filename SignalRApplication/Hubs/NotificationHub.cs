using ClassLibrary.Database;
using ClassLibrary.Database.Models;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.Tracing;

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

    public Task SendMessage(string message)
    {
        return Clients.Caller.SendAsync("Send", "eshkere");
        ;
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

        return Clients.Caller.SendAsync("Ok");
    }

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
