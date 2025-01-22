using ReactiveUI;
using System;
using System.Reactive;

namespace ThirdStage
{
    public class IScreenRealization : ReactiveObject, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();
    }
}
