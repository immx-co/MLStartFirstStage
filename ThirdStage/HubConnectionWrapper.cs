using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdStage;

public class HubConnectionWrapper
{
    public HubConnection Connection;

    public HubConnectionWrapper()
    {
        Connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:1234/notify")
                .Build();

        Connection.Closed += async (error) =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await Connection.StartAsync();
        };
    }

    public async Task Start()
    {
        await Connection.StartAsync();
    }

    public async Task Stop()
    {
        await Connection.StopAsync();
    }

    public async Task SendMessage(string message)
    {
        await Connection.InvokeAsync("SendMessage", message);
    }

    public async Task Login(string nickName, string password)
    {
        await Connection.InvokeAsync("Login", nickName, password);
    }

    public async Task Registration(string nickName, string password, string email, string nickNamePattern)
    {
        await Connection.InvokeAsync("Registration", nickName, password, email, nickNamePattern);
    }
}
