using CommunityToolkit.Mvvm.Input;

namespace Velocity.ViewModels
{
    public partial class HomeViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _main;
        public HomeViewModel(MainWindowViewModel main)
        {
            _main = main;
        }
        public override double? SetWidth => 700;
        public override double? SetHeight => 500;

        [RelayCommand]
        private void OpenTut()
        {
            _main.NavTut();
        }

        [RelayCommand]
        private void OpenRem()
        {
            _main.NavRem();
        }
    }   
}
