using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace Velocity.ViewModels
{
    public partial class DefCLIViewModel : ViewModelBase
    {
        private readonly DefCommands _givenCommand = new();
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
    public class DefCommands
    {
        string[] temp = { "Background Task performed, output changed" };
        bool check = false;
        public IEnumerable<string> Process(string command)
        {
            var cd = command.Trim().ToLowerInvariant();
            if (cd == "help")
            {
                if (check == true)
                {
                    return temp;
                }
                check = true;
                return new[] 
                {
                    "Commands:",
                    "ufw enable - turns on the device's firewall",
                    "show ports - lists currently open ports",
                    "show services - lists currently running services",
                    "close ports - turn off unnecessary ports",
                    "stop services - turn off unnecessary services",
                };
            }
            return cd switch
            {
                "help" => new[]
                {
                    "Commands:",
                    "ufw enable - turns on the device's firewall",
                    "show ports - lists currently open ports",
                    "show services - lists currently running services",
                    "close ports - turn off unnecessary ports",
                    "stop services - turn off unnecessary services",
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
