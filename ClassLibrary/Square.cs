namespace ClassLibrary
{
    public class Square : IFigure, IQuantityCoal
    {
        public int CoalQuantity { get; } = 4;

        public void UniqueTask()
        {
            Console.WriteLine("Создаю надежность.");
        }

        public string Ability()
        {
            return "Создаю надежность.";
        }
    }
}
