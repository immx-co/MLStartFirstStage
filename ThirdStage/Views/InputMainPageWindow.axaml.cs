using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ThirdStage.ViewModels;

namespace ThirdStage;

public partial class InputMainPageWindow : ReactiveUserControl<InputMainPageViewModel>
{
    public InputMainPageWindow()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}