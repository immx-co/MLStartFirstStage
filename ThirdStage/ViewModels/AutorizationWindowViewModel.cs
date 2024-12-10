using ReactiveUI;
using System.Reactive;
using System;

namespace ThirdStage.ViewModels;

public partial class AutorizationWindowViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> OpenMainWindowCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseWindowCommand { get; }

    private readonly Action _openMainWindow;
    private readonly Action _closeWindow;

    public AutorizationWindowViewModel(Action openMainWindow, Action closeThisWindow)
    {
        _openMainWindow = openMainWindow;
        _closeWindow = closeThisWindow;
        OpenMainWindowCommand = ReactiveCommand.Create(OpenMainWindow);
        CloseWindowCommand = ReactiveCommand.Create(CloseWindow);
    }

    private void OpenMainWindow()
    {
        _openMainWindow?.Invoke();
    }

    private void CloseWindow()
    {
        _closeWindow?.Invoke();
    }
}
