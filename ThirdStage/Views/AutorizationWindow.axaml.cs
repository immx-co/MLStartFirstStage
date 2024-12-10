using Avalonia.Controls;
using ThirdStage.ViewModels;

namespace ThirdStage.Views;

public partial class AutorizationWindow : Window
{
    public AutorizationWindow()
    {
        InitializeComponent();

        DataContext = new AutorizationWindowViewModel(
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