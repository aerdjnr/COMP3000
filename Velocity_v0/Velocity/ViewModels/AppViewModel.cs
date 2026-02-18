using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.DirectoryServices.Protocols;
using System.Net;
using System;

namespace Velocity.ViewModels
{
    public partial class AppViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _CurrentViewModel;

        public AppViewModel()
        {
            CurrentViewModel = new LoginViewModel(this);
        }
        public void NavToHome()
        {
            CurrentViewModel = new HomeViewModel(this);
        }
    }

}
