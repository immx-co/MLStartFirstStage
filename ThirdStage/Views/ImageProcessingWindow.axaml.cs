using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ThirdStage.ViewModels;

namespace ThirdStage;

public partial class ImageProcessingWindow : ReactiveUserControl<ImageProcessingViewModel>
{
    public ImageProcessingWindow()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}