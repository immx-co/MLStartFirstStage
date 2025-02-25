using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using System.Linq;
using ThirdStage.ViewModels;

using Microsoft.Extensions.DependencyInjection;
using ClassLibrary.Database;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;

namespace ThirdStage;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            string fileName = "appsettings.json";

            if (!File.Exists(fileName))
            {
                var defaultConfiguration = new
                {
                    N = "8",
                    L = "5",
                    ConnectionStrings = new
                    {
                        stringConnection = "Host=localhost;Port=5432;Database=avaloniadb4;Username=postgres;Password=default"
                    },
                    SmtpSettings = new
                    {
                        Server = "smtp.yandex.ru",
                        Port = 587,
                        Username = "immxxx@yandex.ru",
                        Password = "default",
                        EnableSsl = true
                    }
                };
                string jsonConfiguration = JsonSerializer.Serialize(defaultConfiguration, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(fileName, jsonConfiguration);

                Log.Logger.Information("Конфигурация была сохранена по умолчанию успешно. Пароль от почты с рассылками уточните у администратора!");
            }

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory
                .GetCurrentDirectory())
                .AddJsonFile(fileName).Build();
            Log.Logger.Information("Конфигурация загружена успешно.");

            ServiceProvider servicesProvider = ServicesRegister(configuration);

            servicesProvider.GetRequiredService<HubConnectionWrapper>().Start();
            Log.Logger.Information("Оформлено подкючение к хабу.");

            desktop.MainWindow = new InputWindow
            {
                DataContext = servicesProvider.GetService<InputWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private ServiceProvider ServicesRegister(IConfiguration configuration)
    {
        IServiceCollection servicesProvider = new ServiceCollection();

        servicesProvider.AddSingleton(configuration);

        servicesProvider.AddSingleton<IScreen, IScreenRealization>();

        servicesProvider.AddSingleton<InputWindowViewModel>();
        servicesProvider.AddTransient<FigureViewModel>();
        servicesProvider.AddSingleton<InputMainPageViewModel>();
        servicesProvider.AddTransient<MainWindowViewModel>();
        servicesProvider.AddTransient<JokesWindowViewModel>();
        servicesProvider.AddTransient<ImageProcessingViewModel>();
        servicesProvider.AddTransient<RegistrationViewModel>();
        servicesProvider.AddSingleton<AutorizationWindowViewModel>();

        servicesProvider.AddSingleton<PasswordHasher>();

        servicesProvider.AddDbContext<ApplicationContext>(options => options.UseNpgsql(configuration.GetConnectionString("stringConnection")), ServiceLifetime.Transient);

        servicesProvider.AddSingleton<HubConnectionWrapper>();

        return servicesProvider.BuildServiceProvider();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}