using System.Diagnostics;

namespace ClassLibrary
{
    public class Square : IFigure, IQuantityCoal
    {
        public int CoalQuantity { get; } = 4;

        public void UniqueTask()
        {
            Debug.WriteLine("Создаю надежность.");
        }

        public string Ability()
        {
            return "Создаю надежность.";
        }
    }
}
