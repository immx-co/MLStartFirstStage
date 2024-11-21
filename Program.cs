using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Drawing;
using Serilog;


namespace MLStartFirstStage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("logs/verbose.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
                .WriteTo.File("logs/debug.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
                .WriteTo.File("logs/info.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.File("logs/warning.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
                .WriteTo.File("logs/error.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
                .WriteTo.File("logs/fatal.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Fatal)
                .WriteTo.Console()
                .CreateLogger();

            var fileName = "MLstartConfig.json";
            var content = @"{""N"": ""8"", ""L"": ""5""}";
            File.WriteAllText(fileName, content);

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory
                .GetCurrentDirectory())
                .AddJsonFile(fileName).Build();

            string N = configuration["N"];
            string L = configuration["L"];
            Console.WriteLine(N);
            Console.WriteLine(L);

            int[] oddNumbers = CreateOddNumbersArray();
            double[] randomValues = CreateRandomValuesArray();
            double[,] k = CreateTwoDimensionalArray(8, 13, oddNumbers, randomValues);
        }

        static int[] CreateOddNumbersArray()
        {
            int start = 5;
            int end = 19;
            int oddNumbersSize = (end - start) / 2 + 1;
            int[] oddNumbers = new int[oddNumbersSize];

            for(int i = 0, num = start; i < oddNumbersSize; i++, num += 2)
            {
                oddNumbers[i] = num;
            }

            return oddNumbers;
        }

        static double[] CreateRandomValuesArray()
        {
            double min = -12.0;
            double max = 15.0;
            int randomValuesSize = 13;
            double[] randomValues = new double[randomValuesSize];

            Random random = new Random();
            
            for(int i = 0; i < randomValuesSize; i++)
            {
                randomValues[i] = min + random.NextDouble() * (max - min);
            }

            return randomValues;
        }

        static double[,] CreateTwoDimensionalArray(int rows, int columns, int[] oddNumbers, double[] randomValues)
        {
            double[,] k = new double[rows, columns];
            
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    double x = randomValues[j];
                    if (oddNumbers[i] == 9)
                    {
                        k[i, j] = Math.Sin(Math.Sin(Math.Pow(x / (x + 1 / 2), x)));
                    }
                    else if(Array.Exists(new int[] { 5, 7, 11, 15 }, element => element == oddNumbers[i])) {
                        k[i, j] = Math.Pow(Math.Pow((0.5 / (Math.Tan(2 * x) + 2 / 3)), 1 / 3), 1 / 3);
                    }
                    else
                    {
                        k[i, j] = Math.Tan(Math.Pow(((Math.Pow(Math.E, (1 - x) / Math.PI)) / 3) / 4, 3));
                    }
                }
            }

            return k;
        }
    }
}
