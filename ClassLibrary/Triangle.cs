using System.Diagnostics;

namespace ClassLibrary
{
    public class Triangle : IFigure, IQuantityCorner
    { 
        public int CornerQuantity { get; } = 3;

        public void UniqueTask()
        {
            Debug.WriteLine("Поднимаюсь на горным вершинам.");
        }

        public string Ability()
        {
            return $"Благодаря своим {CornerQuantity} углам, обеспечиваю стабильность конструкций.";
        }
    }
}
