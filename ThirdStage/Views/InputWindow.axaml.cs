using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ThirdStage.ViewModels;

namespace ThirdStage;

public partial class InputWindow : ReactiveWindow<InputWindowViewModel>
{
    public InputWindow()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}