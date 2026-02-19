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
            NavToLogin();
        }
        public void NavToLogin()
        {
            CurrentView = new LoginViewModel();
        }
        public void NavHome()
        {
            // CurrentView = new homeViewModelName()
        }
    }
}
