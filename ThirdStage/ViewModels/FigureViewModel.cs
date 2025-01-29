using System.Windows.Input;
using ClassLibrary;

public class FigureViewModel
{
    public IFigure Figure { get; set; } = null!;

    public ICommand ActionCommand { get; set; } = null!;
}