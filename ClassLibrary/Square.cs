using System.Diagnostics;

namespace ClassLibrary
{
    public class Square : IFigure, IQuantityCorner
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
