using Avalonia.Controls;
using Microsoft.Extensions.Configuration;
using ThirdStage.ViewModels;

namespace ThirdStage.Views;

public partial class AutorizationWindow : Window
{
    public AutorizationWindow(IConfiguration configuration)
    {
        InitializeComponent();

        DataContext = new AutorizationWindowViewModel(
                configuration,
                openMainWindow: () =>
                {
                    var mainWindow = new MainWindow
                    {
                        DataContext = new MainWindowViewModel()
                    };
                    mainWindow.Show();
                    this.Close();
                },
                closeThisWindow: () => this.Close()
            );
    }
}