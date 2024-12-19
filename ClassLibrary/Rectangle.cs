using System.Diagnostics;

namespace ClassLibrary
{
    public class Rectangle : IFigure, IQuantityCorner
    {
        public int CoalQuantity { get; } = 4;

        public void UniqueTask()
        {
            Debug.WriteLine("Соединяю дороги.");
        }

        public string Ability()
        {
            return "Cоединяю дороги, обеспечивая доступность передвижения другим фигурам.";
        }
    }
}
