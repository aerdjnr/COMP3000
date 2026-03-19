using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;



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
        // Command variables
        bool fw_state = false;

        string[] service_list = ["Service       Port\n"];
        string webService = "WebService    8080";
        string dataService = "DataService   5600";
        string apiService = "ApiService    3000";
        public IEnumerable<string> Process(string command)
        {
            // Command splicing to specify command and necessary process required
            string cd = command.Trim().ToLowerInvariant();
            string[] cd_ext = cd.Split(' ');

            // Command
            if (cd_ext.Length == 1)
            {
                return cd switch
                {
                    "help" => new[]
                    {
                        "Enter the command to list available options",
                        "Commands:",
                        "fw - Configure the device's firewall",
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
                    "service" => new[]
                    {
                        "show - list currently running services",
                        "start - initiates a service",
                        "stop - halts a service"
                    },
                    _ => new[]
                    {
                        "Invalid command"
                    }
                };
            }

            // Command + option
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
                                "add - inserts a given rule",
                                "remove - delete a given rule"
                            };

                        default:
                            return new[]
                            {
                              "Invalid option"
                            };
                    }
                }
                if (cd_ext[0] == "service")
                {
                    switch (cd_ext[1])
                    {
                        case "show":
                            if (service_list.Length == 1)
                            {
                                return new[]
                                {
                                    "No services currently running"
                                };
                            }
                            return service_list;
                        case "start":
                            return new[]
                            {
                                "Here is a list of example services to start:",
                                "WebService",
                                "DataService",
                                "ApiService",
                            };
                        case "stop":
                            return new[]
                            {
                                "Here is a list of example services to stop:",
                                "WebService",
                                "DataService",
                                "ApiService",
                            };
                        default:
                            return new[]
                            {
                              "Invalid option"
                            };
                    }
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
                    "Potential escape located"
                }
            };
        }
    }
}
