namespace Velocity.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _CurrentView;
        public ViewModelBase CurrentView
        {
            get => _CurrentView;
            set => SetProperty(ref _CurrentView, value);
        }

        public MainWindowViewModel()
        {
            InitLogin();
            //CLIbuild();
        }

        public void InitLogin() => CurrentView = new LoginViewModel(this);
          
        public void NavHome() => CurrentView = new HomeViewModel(this);

        public void NavTut() => CurrentView = new TutViewModel(this);
        public void VelCLI() => CurrentView = new VelCLIViewModel();
        public void DefCLI() => CurrentView = new DefCLIViewModel();
        

    }
}
