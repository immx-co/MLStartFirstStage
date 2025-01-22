using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using System.Linq;
using ThirdStage.ViewModels;

using Microsoft.Extensions.DependencyInjection;
using ThirdStage.Database;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

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

            string fileName = "MLstartConfig.json";
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory
                .GetCurrentDirectory())
                .AddJsonFile(fileName).Build();

            Log.Logger.Information("Конфигурация загружена успешно.");

            ServiceProvider servicesProvider = ServicesRegister(configuration);

            desktop.MainWindow = new InputWindow
            {
                DataContext = servicesProvider.GetService<IScreenRealization>()
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
        servicesProvider.AddSingleton<AutorizationWindowViewModel>();
        servicesProvider.AddSingleton<FigureViewModel>();
        servicesProvider.AddSingleton<InputMainPageViewModel>();
        servicesProvider.AddSingleton<MainWindowViewModel>();
        servicesProvider.AddSingleton<RegistrationViewModel>();

        servicesProvider.AddSingleton<PasswordHasher>();

        servicesProvider.AddDbContext<ApplicationContext>(options => options.UseNpgsql(configuration.GetConnectionString("stringConnection")));

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