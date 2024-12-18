using System.Net.Security;

namespace DelegatesPractice
{
    internal class Program
    {
        delegate void Message();
        delegate int Operation(int x, int y);
        //delegate T Operation<T, K>(K val);

        static void Main(string[] args)
        {
            //Message mes;
            //mes = Hello;
            //mes();

            //Message message1 = Welcome.Print;
            //Message message2 = Hello.Display;

            //message1();
            //message2();

            //Operation operation = Add;
            //int result = operation(4, 5);
            //Console.WriteLine(result);

            //operation = Multiply;
            //result = operation(4, 5);
            //Console.WriteLine(result);

            //Message? message = Hello;
            //message += HowAreYou;
            //message += HowAreYou;
            //message -= HowAreYou;
            //message();

            //Message mes = Hello;
            //mes.Invoke();
            //Operation op = Add;
            //int n = op.Invoke(4, 5);
            //Console.WriteLine(n);

            //Message? mes = null;
            //mes?.Invoke();

            //Operation<decimal, int> squareOperation = Square;
            //decimal result1 = squareOperation(5);
            //Console.WriteLine(result1);

            //Operation<int, int> doubleOperation = Double;
            //int result2 = doubleOperation(5);
            //Console.WriteLine(result2);

            //DoOperation(5, 2, Subtract);

            Operation operation = SelectOperation(OperationType.Add);
            Console.WriteLine(operation(4, 5));

            operation = SelectOperation(OperationType.Subtract);
            Console.WriteLine(operation(10, 5));

            operation = SelectOperation(OperationType.Multiply);
            Console.WriteLine(operation(10, 4));
        }

        private static Operation SelectOperation(OperationType opType)
        {
            switch (opType)
            {
                case OperationType.Add: return Add;
                case OperationType.Subtract: return Subtract;
                default: return Multiply;
            }
        }

        private static int Add(int x, int y) => x + y;

        private static int Subtract(int x, int y) => x - y;

        private static int Multiply(int x, int y) => x * y;

        private static void Hello() => Console.WriteLine("Hello");

        private static void HowAreYou() => Console.WriteLine("How are you?");

        private static decimal Square(int n) => n * n;

        private static int Double(int n) => n + n;

        private static void DoOperation(int a, int b, Operation op)
        {
            Console.WriteLine(op(a, b));
        }
    }

    enum OperationType
    {
        Add,
        Subtract,
        Multiply
    }

    class Welcome
    {
        public static void Print() => Console.WriteLine("Welcome");
    }

    //class Hello
    //{
    //    public static void Display() => Console.WriteLine("Привет!");
    //}
}
