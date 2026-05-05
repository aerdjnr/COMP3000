using Avalonia.Controls;
using Avalonia.Input;
using Velocity.ViewModels;

namespace Velocity.Views
{
    public partial class OffLabView : UserControl
    {
        public OffLabView()
        {
            InitializeComponent();
        }

        private void PressEnter(object? sender, KeyEventArgs e)
        {
            if (DataContext is not OffLabViewModel vm)
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
