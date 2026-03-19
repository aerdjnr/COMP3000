using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
            string cd = command.Trim().ToLowerInvariant();
            string[] cd_ext = cd.Split(' ');
            foreach(var item in cd_ext)
            {
                Debug.WriteLine(item);
            }
            if (cd_ext.Length == 1)
            {
                return cd switch
                {
                    "help" => new[]
                    {
                        "Input the command to list available options",
                        "Commands:",
                        "fw - Configure the device's firewall",
                        "show - lists available resources based on the option given",
                        "port - configure ports",
                        "service - configure services",
                    },
                    "fw" => new[]
                    {
                        "Options:",
                        "status - displays firewall state",
                        "enable - turns on the firewall",
                        "disable - turns off the firewall",
                        "rules - list currently enabled rules",
                    },
                    "show" => new[]
                    {
                        "ports - list currently open ports",
                        "services - list currently running services",
                    },
                    "port" => new[]
                    {
                        "open [ports] - opens the given ports. E.g. open 22,25,53,80",
                        "close [ports] - closes the given ports. E.g. close 443,445,3389"
                    },
                    "service" => new[]
                    {
                        "start [service] - initiates a service",
                        "stop [service] - halts a service"
                    },
                    _ => new[]
                    {
                        "1 word command given, but is not valid"
                    }
                };
            }
            return cd switch
            {
                _ => new[]
                {
                    "Potential bypass located, no catch made"
                }
            };
        }
    }
}
