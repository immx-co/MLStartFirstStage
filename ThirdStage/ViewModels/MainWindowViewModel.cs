using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using ClassLibrary;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace ThirdStage.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly Kingdom _kingdom = new();

    public ICommand AddCircleCommand { get; init; }
    public ICommand AddSquareCommand { get; init; }
    public ICommand AddTriangleCommand { get; init; }
    public ICommand AddRectangleCommand { get; init; }

    private ObservableCollection<Control> _dynamicFigures = [];

    public ObservableCollection<Control> DynamicFigures
    {
        get => _dynamicFigures;
        set
        {
            _dynamicFigures = value;
            OnPropertyChanged(nameof(DynamicFigures));
        }
    }

    private static Shape figureType;

    private int _countsFigures = 0;
    public int CountFigures
    {
        get => _countsFigures;
        set
        {
            _countsFigures = value;
            OnPropertyChanged(nameof(CountFigures));
        }
    }

    private string _lastAction = string.Empty;
    public string LastAction
    {
        get => _lastAction;
        set
        {
            _lastAction = value;
            OnPropertyChanged(nameof(LastAction));
        }
    }

    public MainWindowViewModel()
    {
        AddCircleCommand = ReactiveCommand.Create(AddCircle);
        AddSquareCommand = ReactiveCommand.Create(AddSquare);
        AddTriangleCommand = ReactiveCommand.Create(AddTriangle);
        AddRectangleCommand = ReactiveCommand.Create(AddRectangle);
    }

    private void AddCircle()
    {
        Circle circle = new Circle();
        _kingdom.AddFigure(circle);
        CountFigures = _kingdom.GetLenFigures();
        LastAction = "Circle added.";
        AddFigureContent(circle);
        Debug.WriteLine("Circle added.");
    }

    private void AddSquare()
    {
        Square square = new Square();
        _kingdom.AddFigure(square);
        CountFigures = _kingdom.GetLenFigures();
        LastAction = "Square added.";
        AddFigureContent(square);
        Debug.WriteLine("Sqaure added.");
    }

    private void AddTriangle()
    {
        Triangle triangle = new Triangle();
        _kingdom.AddFigure(triangle);
        CountFigures = _kingdom.GetLenFigures();
        LastAction = "Triangle added.";
        AddFigureContent(triangle);
        Debug.WriteLine("Triangle added.");
    }

    private void AddRectangle()
    {
        ClassLibrary.Rectangle rectangle = new ClassLibrary.Rectangle();
        _kingdom.AddFigure(rectangle);
        CountFigures = _kingdom.GetLenFigures();
        LastAction = "Rectangle added.";
        AddFigureContent(rectangle);
        Debug.WriteLine("Rectangle added.");
    }

    private void AddFigureContent(IFigure figure)
    {
        Debug.WriteLine($"Called AddFigureContent {figure}");
        Shape figureType = GetFigureType(figure);
        DynamicFigures.Add(figureType);
    }

    private static Shape GetFigureType(IFigure figure)
    {
        if (figure.GetType() == typeof(Circle))
        {
            var figureType = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.OrangeRed,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
        }
        else if (figure.GetType() == typeof(Square))
        {
            var figureType = new Avalonia.Controls.Shapes.Rectangle
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Firebrick,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
        }
        else if (figure.GetType() == typeof(Triangle))
        {
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Avalonia.Point(0, 100);
            LineSegment lineSegment1 = new LineSegment();
            lineSegment1.Point = new Avalonia.Point(50, 0);
            LineSegment lineSegment2 = new LineSegment();
            lineSegment2.Point = new Avalonia.Point(100, 100);

            pathFigure.Segments.Add(lineSegment1);
            pathFigure.Segments.Add(lineSegment2);

            pathFigure.IsClosed = true;

            pathGeometry.Figures.Add(pathFigure);

            Path figureType = new Path();
            figureType.Data = pathGeometry;
            figureType.Fill = Brushes.GreenYellow;
            figureType.Stroke = Brushes.Black;
            figureType.StrokeThickness = 2;
            figureType.Width = 10;
            figureType.Height = 10;
        }
        else
        {
            var figureType = new Avalonia.Controls.Shapes.Rectangle
            {
                Width = 15,
                Height = 7,
                Fill = Brushes.Cyan,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
        }
        return figureType;
    }
}
