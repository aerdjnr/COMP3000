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
        }
        public void InitLogin()
        {
            CurrentView = new LoginViewModel(this);
            
        }
        public void NavHome()
        {
            CurrentView = new HomeViewModel();
        }
    }
}
