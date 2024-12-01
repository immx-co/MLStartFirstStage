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
            kingdom.AddFigure(new Circle());
            kingdom.AddFigure(new Triangle());
            kingdom.AddFigure(new Square());
            kingdom.AddFigure(new Rectangle());

            IFigure[] allFigures = kingdom.GetFigures();
            Console.WriteLine(kingdom.GetFigures());
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
