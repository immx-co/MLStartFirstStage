using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ThirdStage.ViewModels;

namespace ThirdStage;

public partial class ImageProcessingWindow : ReactiveUserControl<ImageProcessingViewModel>
{
    public ImageProcessingWindow()
    {
        AvaloniaXamlLoader.Load(this);
    }
}