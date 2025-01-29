using Avalonia.Controls.Templates;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using ClassLibrary;

namespace ThirdStage.DataTemplates;

public class FigureDataTemplate : IDataTemplate
{
    public Control Build(object figure)
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

    public bool Match(object data)
    {
        return data is IFigure;
    }
}