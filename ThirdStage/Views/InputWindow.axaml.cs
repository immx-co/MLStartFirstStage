using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using ThirdStage.ViewModels;

namespace ThirdStage;

public partial class InputWindow : ReactiveWindow<InputWindowViewModel>
{
    public InputWindow()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }

    private void WindowPointerMoved(object sender, PointerPressedEventArgs e)
    {
        var position = e.GetPosition(this);
        double topAreaHeight = 35;

        if (position.Y <= topAreaHeight && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
}