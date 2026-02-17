using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Velocity.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        [ObservableProperty]
        public string _Username;

        [ObservableProperty]
        public string _Password;

        [ObservableProperty]
        public string _LoginStatus;
        [RelayCommand]
        private async Task LoginAsync()
        {
            var mainWindow = new MainWindowViewModel();

        }

    }



}
