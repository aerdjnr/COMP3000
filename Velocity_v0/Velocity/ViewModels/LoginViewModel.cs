using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Velocity.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        // Transition initiation
        private readonly MainWindowViewModel _main;
        public LoginViewModel(MainWindowViewModel main)
        {
            _main = main;
        }

        public override double? SetWidth => 400;
        public override double? SetHeight => 400;
        


        // Properties/Variables
        [ObservableProperty]
        public string _Username;

        [ObservableProperty]
        public string _Password;


        // Commands/Functions
        [RelayCommand]
        private async Task Login()
        {
            // Error checking, only at the bottom should "OpenMain" be run
            Debug.WriteLine($"Username: {Username+"@users.local"}, Password: {Password}");
            //bool result = await VerifyLDAP();
            //Debug.WriteLine(result);
            _main.NavHome();
        }

        [RelayCommand]
        private async Task<bool> VerifyLDAP()
        {
            // Sends to DC for check
            string DC = "users.local";
            int port = 389;

            using var LDAPquery= new LdapConnection(new LdapDirectoryIdentifier(DC, port));

            var Creds = new NetworkCredential(Username+"@users.local", Password);
            return await Task.Run(() =>
            {
                try
                {
                    LDAPquery.AuthType = AuthType.Basic;
                    LDAPquery.Bind(Creds);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
            
        }

    }

}
