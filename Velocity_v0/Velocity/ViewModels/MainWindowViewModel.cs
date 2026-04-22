using CommunityToolkit.Mvvm.ComponentModel;
using Renci.SshNet;
using System.Diagnostics;

namespace Velocity.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            InitLogin();
            //DefCLI();
            //IntroCLI();
            //NavTut();
            //OffCLI();
        }

        [ObservableProperty]
        private ViewModelBase _CurrentView;


        public void placeholder_SSH(string host, string username, string password, string command)
        {
            using (var client = new SshClient(host, username, password))
            {
                client.Connect();

                var test = client.CreateCommand(command);
                string result = test.Execute();
                Debug.WriteLine(result);
            }
        }
        public void InitLogin() => CurrentView = new LoginViewModel(this);
        public void NavHome() => CurrentView = new HomeViewModel(this);
        public void NavTut() => CurrentView = new TutViewModel(this);
        public void NavRem() => CurrentView = new RemViewModel(this);
        public void IntroCLI() => CurrentView = new IntroCLIViewModel(this);
        public void DefCLI() => CurrentView = new DefCLIViewModel(this);
        public void DefLab() => CurrentView = new DefLabViewModel(this);
        public void InformCLI() => CurrentView = new InformCLIViewModel(this);
        public void InformLab() => CurrentView = new InformLabViewModel(this);
        public void OffCLI() => CurrentView = new OffCLIViewModel(this);
        public void OffLab() => CurrentView = new OffLabViewModel(this);
    }
}
