using Avalonia.Controls;
using Avalonia.Input;
using Velocity.ViewModels;

namespace Velocity.Views
{
    public partial class DefLabView : UserControl
    {
        public DefLabView()
        {
            InitializeComponent();
        }

        private void PressEnter(object? sender, KeyEventArgs e)
        {
            if (DataContext is not DefLabViewModel vm)
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
