using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using Velocity.ViewModels;

namespace Velocity.Views
{
    public partial class CLIView : UserControl
    {
        public CLIView()
        {
            InitializeComponent();
        }

        private void PressEnter(object? sender, KeyEventArgs e)
        {
            if (DataContext is not CLIViewModel vm)
                return;
            switch (e.Key)
            {
                case Key.Enter:
                    vm.ExecuteCommand.Execute(null);
                    e.Handled = true;
                    Debug.WriteLine("Reaches axaml.cs");
                    break;
            }
        }
    }

}
