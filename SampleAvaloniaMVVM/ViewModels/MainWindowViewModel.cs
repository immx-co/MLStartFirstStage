namespace SampleAvaloniaMVVM.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";

        public SimpleViewModel SimpleViewModel { get; } = new SimpleViewModel();

        public ReactiveViewModel ReactiveViewModel { get; } = new ReactiveViewModel();
    }
}
