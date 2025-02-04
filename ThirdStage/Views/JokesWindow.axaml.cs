using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ThirdStage.ViewModels;

namespace ThirdStage;

public partial class JokesWindow : ReactiveUserControl<JokesWindowViewModel>
{
    public JokesWindow()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
