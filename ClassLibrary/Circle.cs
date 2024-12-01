namespace ClassLibrary
{
    public class Circle : IFigure, IQuantityCoal
    {
        public int CoalQuantity { get; } = 0;

        public void UniqueTask()
        {
            Console.WriteLine("Двигаюсь по маршрутам, очищая путь от препятствий.");
        }

        public string Ability()
        {
            return "Сталкиваюсь с камнями на участках путей";
        }
    }
}
