using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velocity.ViewModels
{
    public partial class CLIViewModel : ViewModelBase
    {
        private readonly Commands _givenCommand = new();
        private string _commandInput = string.Empty;
        public ObservableCollection<string> OutputLines { get; } = new();

        public string CommandInput 
        { 
            get => _commandInput;
            set => SetProperty(ref _commandInput, value);
        }

        [RelayCommand]
        private void Execute() 
        { 
            if (string.IsNullOrWhiteSpace(CommandInput))
            {
                return;
            }
            Debug.WriteLine("Reaches execute command");
            foreach (var line in _givenCommand.Process(CommandInput))
            {
                OutputLines.Add(line);
            }
            CommandInput = string.Empty;
        }
    }
    public class Commands
    {
        public IEnumerable<string> Process(string command)
        {
            var cd = command.Trim().ToLowerInvariant();
            return cd switch
            {
                "test" => new[]
                {
                    "Commands:",
                    "Command1 - Description",
                    "Command2 - Description",
                },
                _ => new[]
                {
                    $"Unkown command: {command}"
                }
            };
        }
    }
}
