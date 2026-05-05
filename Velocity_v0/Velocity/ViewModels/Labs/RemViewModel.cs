using CommunityToolkit.Mvvm.Input;
using Renci.SshNet;
using System.Diagnostics;
using Tmds.DBus.Protocol;

namespace Velocity.ViewModels
{
    public partial class RemViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _main;
        public RemViewModel(MainWindowViewModel main)
        {
            _main = main;
            //_main.placeholder_SSH("192.168.15.10", "defense", "defense", "echo cat");
        }
        public override double? SetWidth => 700;
        public override double? SetHeight => 500;

        
        [RelayCommand]
        private void OpenDefense()
        {
            _main.DefLab();
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
