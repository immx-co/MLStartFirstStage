using Microsoft.Extensions.Configuration;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ThirdStage.Views;

namespace ThirdStage.ViewModels
{
    public class InputWindowViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();

        public ReactiveCommand<Unit, IRoutableViewModel> Input { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> Registration { get; }

        public InputWindowViewModel()
        {
            ;
        }
    }
}
