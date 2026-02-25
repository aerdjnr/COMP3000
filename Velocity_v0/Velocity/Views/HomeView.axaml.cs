using Avalonia.Controls;

namespace Velocity.Views
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            WelcomeText.Text = "Thank you for installing Velocity! To get started and understand how the platform work, click 'tutorial'.If you are familiar (or a pro!) go ahead and click 'Labs' to get started.";


        }
    }
}