using Avalonia.Controls;
using System.Windows.Input;

public class FigureViewModel
{
    public Control Figure { get; set; }
    public ICommand ActionCommand { get; set; }
}