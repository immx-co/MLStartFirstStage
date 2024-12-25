using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RoutingExample.ViewModels;

namespace RoutingExample;

public partial class LoginWindow : ReactiveUserControl<LoginViewModel>
{
    public LoginWindow()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}