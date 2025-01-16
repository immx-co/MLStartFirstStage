using Avalonia.Controls;
using System.Windows.Input;

public class FigureViewModel
{
    public Control Figure { get; set; } = null!;
    public ICommand ActionCommand { get; set; } = null!;
}