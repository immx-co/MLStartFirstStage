using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RoutingExample.ViewModels;

namespace RoutingExample;

public partial class RegistrationWindow : ReactiveUserControl<RegistrationViewModel>
{
    public RegistrationWindow()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}