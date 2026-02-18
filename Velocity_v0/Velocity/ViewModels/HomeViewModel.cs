using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.DirectoryServices.Protocols;
using System.Net;
using System;

namespace Velocity.ViewModels
{
    public partial class HomeViewModel : ViewModelBase
    {
        private readonly AppViewModel _app;
        public HomeViewModel(AppViewModel app)
        {
            _app = app;
        }
    }

}
