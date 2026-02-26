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
            //InitLogin();
            CLIbuild();
        }

        public void InitLogin() => CurrentView = new LoginViewModel(this);
          
        public void NavHome() => CurrentView = new HomeViewModel();

        public void CLIbuild() => CurrentView = new CLIViewModel();
        // public void NavTut() => CurrentView = new TutViewModel();
        // public void NavLabSelect() => CurrentView = new LabSelectViewModel();

    }
}
