using System.Diagnostics;

namespace ClassLibrary
{
    public class Triangle : IFigure, IQuantityCorner
    { 
        public int CoalQuantity { get; } = 3;

        public void UniqueTask()
        {
            Debug.WriteLine("Поднимаюсь на горным вершинам.");
        }

        public string Ability()
        {
            return $"Благодаря своим {CoalQuantity} углам, обеспечиваю стабильность конструкций.";
        }
    }
}
