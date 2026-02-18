using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Velocity.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
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
            bool result = await VerifyLDAP();
            Debug.WriteLine(result);
        }

        [RelayCommand]
        private async Task<bool> VerifyLDAP()
        {
            // Sends to DC for check
            string DN = "users.local";
            int port = 389;

            using var LDAPquery= new LdapConnection(new LdapDirectoryIdentifier(DN, port));

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

        [RelayCommand]
        private void OpenMain()
        {
            // Where I will transition ownership of "main" attribute to the core app window
        }

    }



}
