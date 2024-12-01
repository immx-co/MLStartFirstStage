using ClassLibrary;
using Serilog;
using System.Runtime.InteropServices;

namespace SecondStage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();

            double[] someValues = CreateRandomValuesArray();

            Kingdom kingdom = new();
            Circle circle = new Circle();
            Triangle triangle = new Triangle();
            Square square = new Square();
            Rectangle rectangle = new Rectangle();
            kingdom.AddFigure(circle);
            kingdom.AddFigure(triangle);
            kingdom.AddFigure(square);
            kingdom.AddFigure(rectangle);

            IFigure[] allFigures = kingdom.GetFigures();
            Console.WriteLine(kingdom.GetFigures());

            circle.UniqueTask();
            triangle.UniqueTask();
            square.UniqueTask();
            rectangle.UniqueTask();
        }

        static double[] CreateRandomValuesArray()
        {
            Log.Verbose("Start CreateRandomValuesArray method.");
            double min = -12.0;
            double max = 15.0;
            int randomValuesSize = 13;
            double[] randomValues = new double[randomValuesSize];

            Random random = new Random();

            for (int i = 0; i < randomValuesSize; i++)
            {
                randomValues[i] = min + random.NextDouble() * (max - min);
            }

            Log.Verbose("End CreateRandomValuesArray method.");
            return randomValues;
        }
    }
}
