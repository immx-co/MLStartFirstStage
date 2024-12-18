using System.Runtime.InteropServices;

namespace AsyncPracticeV2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Account account = new Account();
            account.Added += PrintAsync;

            account.Put(500);

            await Task.Delay(2000);

            async void PrintAsync(object? obj, string message)
            {
                Console.WriteLine("Начало метода PrintAsync");
                await Task.Delay(3000);
                Console.WriteLine(message);
                Console.WriteLine("Конец метода PrintAsync");
            }
        }

        class Account
        {
            int sum = 0;
            public event EventHandler<string>? Added;

            public void Put(int sum)
            {
                Console.WriteLine("Начало метода Put класса Account.");
                this.sum += sum;
                Added?.Invoke(this, $"На счет поступило {sum}$");
                Console.WriteLine("Конец метода Put класса Account.");
            }
        }
    }
}
