namespace ClassLibrary
{
    public class Kingdom
    {
        private readonly List<IFigure> figures;

        public Kingdom()
        {
            figures = new List<IFigure>();
        }

        public void AddFigure(IFigure figure)
        {
            if (figure == null)
            {
                throw new ArgumentNullException(nameof(figure), "Figure cannot be null.");
            }

            figures.Add(figure);
        }

        public IFigure[] GetFigures()
        {
            return figures.ToArray();
        }

        public int GetLenFigures()
        {
            return figures.Count;
        }
    }
}
