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
        public override double? SetWidth => 700;
        public override double? SetHeight => 500;
        
    }   
}
