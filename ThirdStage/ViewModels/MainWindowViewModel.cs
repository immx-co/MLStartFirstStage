﻿using Avalonia.Controls.Shapes;
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

namespace ThirdStage.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly Kingdom _kingdom = new();

    public ICommand AddCircleCommand { get; init; }
    public ICommand AddSquareCommand { get; init; }
    public ICommand AddTriangleCommand { get; init; }
    public ICommand AddRectangleCommand { get; init; }

    private ObservableCollection<FigureViewModel> _dynamicFigures = new();
    public ObservableCollection<FigureViewModel> DynamicFigures
    {
        get => _dynamicFigures;
        set
        {
            _dynamicFigures = value;
            OnPropertyChanged(nameof(DynamicFigures));
        }
    }

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

    private string _userHistory = "Пользовательская история:";
    public string UserHistory
    {
        get => _userHistory;
        set
        {
            _userHistory = value;
            OnPropertyChanged(nameof(UserHistory));
        }
    }

    private string _history = "Программная история:";
    public string History
    {
        get => _history;
        set
        {
            _history = value;
            OnPropertyChanged(nameof(History));
        }
    }

    public MainWindowViewModel()
    {
        AddCircleCommand = ReactiveCommand.Create(AddCircle);
        AddSquareCommand = ReactiveCommand.Create(AddSquare);
        AddTriangleCommand = ReactiveCommand.Create(AddTriangle);
        AddRectangleCommand = ReactiveCommand.Create(AddRectangle);

        Task.Run(GenerateProgrammHistoryAsync);
    }

    private void AddCircle()
    {
        Circle circle = new Circle();
        _kingdom.AddFigure(circle);
        CountFigures = _kingdom.GetLenFigures();
        LastAction = "Circle added.";
        UserHistory += Environment.NewLine + Environment.NewLine + LastAction;
        AddFigureContent(circle);
        Debug.WriteLine("Circle added.");
    }

    private void AddSquare()
    {
        Square square = new Square();
        _kingdom.AddFigure(square);
        CountFigures = _kingdom.GetLenFigures();
        LastAction = "Square added.";
        UserHistory += Environment.NewLine + Environment.NewLine + LastAction;
        AddFigureContent(square);
        Debug.WriteLine("Sqaure added.");
    }

    private void AddTriangle()
    {
        Triangle triangle = new Triangle();
        _kingdom.AddFigure(triangle);
        CountFigures = _kingdom.GetLenFigures();
        LastAction = "Triangle added.";
        UserHistory += Environment.NewLine + Environment.NewLine + LastAction;
        AddFigureContent(triangle);
        Debug.WriteLine("Triangle added.");
    }

    private void AddRectangle()
    {
        ClassLibrary.Rectangle rectangle = new ClassLibrary.Rectangle();
        _kingdom.AddFigure(rectangle);
        CountFigures = _kingdom.GetLenFigures();
        LastAction = "Rectangle added.";
        UserHistory += Environment.NewLine + Environment.NewLine + LastAction;
        AddFigureContent(rectangle);
        Debug.WriteLine("Rectangle added.");
    }

    private void ExecuteFigureAction(IFigure figure)
    {
        LastAction = figure.Ability();
        UserHistory += Environment.NewLine + Environment.NewLine + LastAction;
        figure.UniqueTask();
    }

    private void AddFigureContent(IFigure figure)
    {
        Shape figureType = GetFigureType(figure);

        DynamicFigures.Add(new FigureViewModel
        {
            Figure = figureType,
            ActionCommand = ReactiveCommand.Create(() => ExecuteFigureAction(figure))
        });

        OnPropertyChanged(nameof(DynamicFigures));
    }

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
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                History += Environment.NewLine + Environment.NewLine + randomFigure.Ability();
            });
        }
        
    }

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
