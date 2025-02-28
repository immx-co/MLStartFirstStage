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

    #region Authorization View Model
    public async Task Login(string nickName, string password)
    {
        await Connection.InvokeAsync("Login", nickName, password);
    }
    #endregion

    #region Registration View Model
    public async Task Registration(string nickName, string password, string email, string nickNamePattern)
    {
        await Connection.InvokeAsync("Registration", nickName, password, email, nickNamePattern);
    }
    #endregion

    #region Jokes View Model
    public async Task GetRandomJoke()
    {
        await Connection.InvokeAsync("GetRandomJoke");
    }

    public async Task GetRandomTen()
    {
        await Connection.InvokeAsync("GetRandomTen");
    }

    public async Task GetRandomJokes()
    {
        await Connection.InvokeAsync("GetRandomJokes");
    }

    public async Task GetTenJokes()
    {
        await Connection.InvokeAsync("GetTenJokes");
    }
    #endregion
}
