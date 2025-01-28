using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using ClassLibrary;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace ThirdStage.ViewModels;

public partial class MainWindowViewModel : ReactiveObject, IRoutableViewModel
{
    public IServiceProvider _servicesProvider;

    public IScreen HostScreen { get; }

    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    private readonly Kingdom _kingdom = new();

    public ICommand AddCircleCommand { get; init; }
    public ICommand AddSquareCommand { get; init; }
    public ICommand AddTriangleCommand { get; init; }
    public ICommand AddRectangleCommand { get; init; }

    public ICommand FlipRightCommand { get; init; }

    private ObservableCollection<FigureViewModel> _dynamicFigures = new();
    public ObservableCollection<FigureViewModel> DynamicFigures
    {
        get => _dynamicFigures;
        set
        {
            this.RaiseAndSetIfChanged(ref _dynamicFigures, value);
        }
    }

    private int _countsFigures = 0;
    public int CountFigures
    {
        get => _countsFigures;
        set
        {
            this.RaiseAndSetIfChanged(ref _countsFigures, value);
        }
    }

    private string _lastAction = string.Empty;
    public string LastAction
    {
        get => _lastAction;
        set
        {
            this.RaiseAndSetIfChanged(ref _lastAction, value);
        }
    }

    private string _userHistory = "Пользовательская история:";
    public string UserHistory
    {
        get => _userHistory;
        set
        {
            this.RaiseAndSetIfChanged(ref _userHistory, value);
        }
    }

    private string _history = "Программная история:";
    public string History
    {
        get => _history;
        set
        {
            this.RaiseAndSetIfChanged(ref _history, value);
        }
    }

    /// <summary>
    /// Конструктор класса MainWindowViewModel. Объявляет в себе несколько реактивных команд и параллельно запускает генерацию истории.
    /// </summary>
    public MainWindowViewModel(IScreen screen, IServiceProvider servicesProvider)
    {
        _servicesProvider = servicesProvider;
        HostScreen = screen;
        Log.Logger = LoggerSetup.CreateLogger();

        AddCircleCommand = ReactiveCommand.Create(AddCircle);
        AddSquareCommand = ReactiveCommand.Create(AddSquare);
        AddTriangleCommand = ReactiveCommand.Create(AddTriangle);
        AddRectangleCommand = ReactiveCommand.Create(AddRectangle);

        FlipRightCommand = ReactiveCommand.Create(FlipRight);

        Task.Run(GenerateProgrammHistoryAsync);
    }
    
    /// <summary>
    /// Листает окно направо.
    /// </summary>
    private void FlipRight()
    {
        Debug.WriteLine("Нажата кнопка направо");
        Log.Logger.Information("Нажата кнопка '>'");
        HostScreen.Router.Navigate.Execute(_servicesProvider.GetRequiredService<JokesWindowViewModel>());
    }

    /// <summary>
    /// Добавляет Круг по нажатию на кнопку.
    /// </summary>
    private void AddCircle()
    {
        Circle circle = new Circle();
        _kingdom.AddFigure(circle);
        CountFigures = _kingdom.GetLenFigures();
        LastAction = "Circle added.";
        UserHistory += Environment.NewLine + Environment.NewLine + LastAction;
        AddFigureContent(circle);
        Log.Logger.Information("Круг добавлен.");
    }

    /// <summary>
    /// Добавляет Квадрат по нажатию на кнопку.
    /// </summary>
    private void AddSquare()
    {
        Square square = new Square();
        _kingdom.AddFigure(square);
        CountFigures = _kingdom.GetLenFigures();
        LastAction = "Square added.";
        UserHistory += Environment.NewLine + Environment.NewLine + LastAction;
        AddFigureContent(square);
        Log.Logger.Information("Квадрат добавлен.");
    }

    /// <summary>
    /// Добавляет Треугольник по нажатию на кнопку.
    /// </summary>
    private void AddTriangle()
    {
        Triangle triangle = new Triangle();
        _kingdom.AddFigure(triangle);
        CountFigures = _kingdom.GetLenFigures();
        LastAction = "Triangle added.";
        UserHistory += Environment.NewLine + Environment.NewLine + LastAction;
        AddFigureContent(triangle);
        Log.Logger.Information("Треугольник добавлен.");
    }

    /// <summary>
    /// Добавляет Прямоугольник по нажатию на кнопку.
    /// </summary>
    private void AddRectangle()
    {
        ClassLibrary.Rectangle rectangle = new ClassLibrary.Rectangle();
        _kingdom.AddFigure(rectangle);
        CountFigures = _kingdom.GetLenFigures();
        LastAction = "Rectangle added.";
        UserHistory += Environment.NewLine + Environment.NewLine + LastAction;
        AddFigureContent(rectangle);
        Log.Logger.Information("Прямоугольник добавлен.");
    }

    /// <summary>
    /// Выполняет действие фигуры, добавленной на главную панель.
    /// </summary>
    /// <param name="figure"></param>
    private void ExecuteFigureAction(IFigure figure)
    {
        LastAction = figure.Ability();
        UserHistory += Environment.NewLine + Environment.NewLine + LastAction;
        figure.UniqueTask();
        Log.Logger.Information($"Выполнено действие фигуры: {LastAction}");
    }

    /// <summary>
    /// Добавляет весь генерируемый контент фигуры.
    /// </summary>
    /// <param name="figure"></param>
    private void AddFigureContent(IFigure figure)
    {
        Shape figureType = GetFigureType(figure);

        DynamicFigures.Add(new FigureViewModel
        {
            Figure = figureType,
            ActionCommand = ReactiveCommand.Create(() => ExecuteFigureAction(figure))
        });
        Log.Logger.Information("Контент фигуры добавлен.");

        //this.RaiseAndSetIfChanged(ref _dynamicFigures, value);
    }

    /// <summary>
    /// Параллельно генерирует историю исходя из того, сколько фигур содержится в королевстве.
    /// </summary>
    /// <returns></returns>
    private async Task GenerateProgrammHistoryAsync()
    {
        while (true)
        {
            await Task.Delay(2000);
            IFigure? randomFigure = GetRandomFigure(_kingdom.GetFigures());
            if (randomFigure is null)
            {
                continue;
            }
            string figureAbility = randomFigure.Ability();
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                History += Environment.NewLine + Environment.NewLine + figureAbility;
            });
            Log.Logger.Debug($"Параллельно добавлено событие: {figureAbility}");
        }
        
    }

    /// <summary>
    /// Получает рандомно фигуру из королевства.
    /// </summary>
    /// <param name="figuresList"></param>
    /// <returns></returns>
    private static IFigure? GetRandomFigure(List<IFigure> figuresList)
    {
        if (figuresList == null || figuresList.Count == 0)
        {
            return null;
        }

        Random random = new Random();
        int randomIndex = random.Next(0, figuresList.Count);
        return figuresList[randomIndex];
    }

    /// <summary>
    /// Возвращает экземпляр класса фигуры из Avalonia в зависимости от того какая фигура передана в аргументы метода.
    /// </summary>
    /// <param name="figure"></param>
    /// <returns></returns>
    private static Shape GetFigureType(IFigure figure)
    {
        if (figure.GetType() == typeof(Circle))
        {
            return new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.OrangeRed,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Margin = new Avalonia.Thickness(5, 5)
            };
        }
        else if (figure.GetType() == typeof(Square))
        {
            return new Avalonia.Controls.Shapes.Rectangle
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Firebrick,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Margin = new Avalonia.Thickness(5, 5)
            };
        }
        else if (figure.GetType() == typeof(Triangle))
        {
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Avalonia.Point(0, 10);
            LineSegment lineSegment1 = new LineSegment();
            lineSegment1.Point = new Avalonia.Point(5, 0);
            LineSegment lineSegment2 = new LineSegment();
            lineSegment2.Point = new Avalonia.Point(10, 10);

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
            figureType.Margin = new Avalonia.Thickness(5, 5);
            return figureType;
        }
        else
        {
            return new Avalonia.Controls.Shapes.Rectangle
            {
                Width = 15,
                Height = 7,
                Fill = Brushes.Cyan,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Margin = new Avalonia.Thickness(5, 6.4)
            };
        }
    }
}
