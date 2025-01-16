using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using System.Linq;
using ThirdStage.ViewModels;

using Microsoft.Extensions.DependencyInjection;
using ThirdStage.Database;

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

            ServiceProvider servicesProvider = ServicesRegister();

            desktop.MainWindow = new InputWindow
            {
                DataContext = servicesProvider.GetService<InputWindowViewModel>(),
            };

            //desktop.MainWindow = new AutorizationWindow(configuration);

            //desktop.MainWindow = new MainWindow
            //{
            //    DataContext = new MainWindowViewModel(),
            //};
        }

        base.OnFrameworkInitializationCompleted();
    }

    private ServiceProvider ServicesRegister()
    {
        IServiceCollection servicesProvider = new ServiceCollection();

        servicesProvider.AddSingleton<AutorizationWindowViewModel>();
        servicesProvider.AddSingleton<FigureViewModel>();
        servicesProvider.AddSingleton<InputMainPageViewModel>();
        servicesProvider.AddSingleton<InputWindowViewModel>();
        servicesProvider.AddSingleton<MainWindowViewModel>();
        servicesProvider.AddSingleton<RegistrationViewModel>();

        servicesProvider.AddSingleton<PasswordHasher>();

        servicesProvider.AddScoped<ApplicationContext>();

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