using CommunityToolkit.Mvvm.ComponentModel;

namespace Velocity.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            //InitLogin();
            DefCLI();
            //VelCLI();
            //NavTut();
        }

        [ObservableProperty]
        private ViewModelBase _CurrentView;


        public void InitLogin() => CurrentView = new LoginViewModel(this);
        public void NavHome() => CurrentView = new HomeViewModel(this);
        public void NavTut() => CurrentView = new TutViewModel(this);
        public void VelCLI() => CurrentView = new VelCLIViewModel(this);
        public void DefCLI() => CurrentView = new DefCLIViewModel(this);
    }
}
