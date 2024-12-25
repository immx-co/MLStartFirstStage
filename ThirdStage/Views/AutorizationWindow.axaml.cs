using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Microsoft.Extensions.Configuration;
using ThirdStage.ViewModels;

namespace ThirdStage.Views;

public partial class AutorizationWindow : ReactiveUserControl<AutorizationWindowViewModel>
{
    public AutorizationWindow()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}