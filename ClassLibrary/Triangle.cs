namespace ClassLibrary
{
    public class Triangle : IFigure, IQuantityCoal
    { 
        public int CoalQuantity { get; } = 3;

        public void UniqueTask()
        {
            Console.WriteLine("Поднимаюсь на горным вершинам.");
        }

        public string Ability()
        {
            return $"Благодаря своим {CoalQuantity} углам, обеспечивая стабильность конструкций.";
        }
    }
}
