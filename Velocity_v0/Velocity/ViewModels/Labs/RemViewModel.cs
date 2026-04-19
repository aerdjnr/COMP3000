using CommunityToolkit.Mvvm.Input;

namespace Velocity.ViewModels
{
    public partial class RemViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _main;
        public RemViewModel(MainWindowViewModel main)
        {
            _main = main;
        }
        public override double? SetWidth => 700;
        public override double? SetHeight => 500;

        [RelayCommand]
        private void OpenIntroduction()
        {
            _main.IntroCLI();
        }

        [RelayCommand]
        private void OpenDefense()
        {
            _main.DefCLI();
        }

        [RelayCommand]
        private void OpenInformation()
        {
            _main.InformCLI();
        }
        [RelayCommand]
        private void OpenOffense()
        {
            _main.OffCLI();
        }

        [RelayCommand]
        private void GoBack()
        {
            _main.NavHome();
        }
    }
}
