using Avalonia.Controls;
using Avalonia.Input;
using Velocity.ViewModels;

namespace Velocity.Views
{
    public partial class InformLabView : UserControl
    {
        public InformLabView()
        {
            InitializeComponent();
        }

        private void PressEnter(object? sender, KeyEventArgs e)
        {
            if (DataContext is not InformLabViewModel vm)
                return;
            switch (e.Key)
            {
                case Key.Enter:
                    vm.ExecuteCommand.Execute(null);
                    e.Handled = true;
                    break;
            }
        }
    }
}
