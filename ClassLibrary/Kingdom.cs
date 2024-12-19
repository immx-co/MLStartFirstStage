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

        public List<IFigure> GetFigures()
        {
            return figures;
        }

        public int GetLenFigures()
        {
            return figures.Count;
        }
    }
}
