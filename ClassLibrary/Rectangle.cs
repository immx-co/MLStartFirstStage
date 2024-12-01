namespace ClassLibrary
{
    public class Rectangle : IFigure, IQuantityCoal
    {
        public int CoalQuantity { get; } = 4;

        public void UniqueTask()
        {
            Console.WriteLine("Соединяю дороги.");
        }

        public string Ability()
        {
            return "Cоединяю дороги, обеспечивая доступность передвижения по королевству другим фигурам";
        }
    }
}
