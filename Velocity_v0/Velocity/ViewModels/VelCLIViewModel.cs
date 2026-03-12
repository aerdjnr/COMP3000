using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace Velocity.ViewModels
{
    public partial class VelCLIViewModel : ViewModelBase
    {
        private readonly VelCommands _givenCommand = new();
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
            string userInput = "> " + CommandInput;
            OutputLines.Add(userInput);
            foreach (var line in _givenCommand.Process(CommandInput))
            { 
                OutputLines.Add(line);
            }
            CommandInput = string.Empty;
        }
    }
    public class VelCommands
    {
        public IEnumerable<string> Process(string command)
        {
            var cd = command.Trim().ToLowerInvariant();
            return cd switch
            {
                "help" => new[]
                {
                    "Commands:",
                    "Command1 - Description",
                    "Command2 - Description",
                },
                "command1" => new[]
                {
                    "Wow! You're a fast learner, or you already know what you're doing :D"
                },
                "command2" => new[]
                {
                    "Sorry, I haven't coded a response for this one.."
                },
                _ => new[]
                {
                    $"Unkown command: {command}"
                }
            };
        }
    }
}
