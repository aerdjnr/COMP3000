using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;


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
        
        //Layer 2 variables
        bool fw_state = false;
        public IEnumerable<string> Process(string command)
        {
            //Command splicing to specify command and necessary process required
            string cd = command.Trim().ToLowerInvariant();
            string[] cd_ext = cd.Split(' ');
            
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
                        "rule - configure firewall rules",
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
                        "Invalid command"
                    }
                };
            }
            if (cd_ext.Length == 2)
            {
                if (cd_ext[0] == "fw")
                {
                    switch(cd_ext[1])
                    {
                        case "status":
                            if (fw_state==true)
                            {
                                return new[] { "State: On" };
                            }
                            else
                            {
                                return new[] { "State: Off" };
                            }

                        case "enable":
                            fw_state = true;
                            return new[] { "Firewall enabled" };
                        
                        case "disable":
                            fw_state = false;
                            return new[] { "Firewall disabled" };
                        
                        case "rule":
                            return new[] 
                            { 
                                "show - lists currently enabled rules",
                                "add - adds an example rule",
                                "remove - removes an example rule"
                            };

                    }
                    Debug.WriteLine("fw command detected");
                }
                if (cd_ext[0] == "show")
                {
                    
                }
                return cd switch
                {
                    _ => new[]
                    {
                        "Invalid command or option"
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
