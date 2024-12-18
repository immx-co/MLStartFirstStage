using System.Runtime.InteropServices;

namespace AsyncPractice
{
    class Program
    {
        async static Task Main(string[] args)
        {
            //var tomTask = PrintNameAsync("Tom");
            //var bobTask = PrintNameAsync("Bob");
            //var samTask = PrintNameAsync("Sam");

            //await tomTask;
            //await bobTask;
            //await samTask;

            Func<string, Task> printer = async (message) =>
            {
                await Task.Delay(3000);
                Console.WriteLine(message);
            };

            await printer("Hello world!");
            await printer("Hello METAINIT.COM");

            void Print()
            {
                Console.WriteLine("Hello!");
            }

            async Task PrintAsync()
            {
                Console.WriteLine("Начало метода PrintAsync");
                await Task.Delay(3000);
                Console.WriteLine("Конец метода PrintAsync");
            }

            async Task PrintNameAsync(string name)
            {
                await Task.Delay(3000);
                Console.WriteLine(name);
            }
        }
    }
}
