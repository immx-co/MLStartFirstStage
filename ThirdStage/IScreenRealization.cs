using ReactiveUI;

namespace ThirdStage
{
    public class IScreenRealization : ReactiveObject, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();
    }
}
