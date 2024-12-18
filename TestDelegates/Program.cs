using System.Diagnostics;

namespace TestDelegates
{
    internal class Program
    {
        delegate void ProcessText(string text);

        static ProcessText pt;

        static void Main(string[] args)
        {
            pt = WriteConsole;
            pt += WriteDebug;
            pt("Andrew");
            pt("slevik");

        }

        private static void WriteConsole(string text)
        {
            Console.WriteLine(text);
        }

        private static void WriteDebug(string text)
        {
            Debug.WriteLine(text);
        }
    }
}
