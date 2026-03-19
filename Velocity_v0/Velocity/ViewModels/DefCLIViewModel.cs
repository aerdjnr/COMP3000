using CommunityToolkit.Mvvm.Input;
using DynamicData;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;



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
        // Command "fw" variables
        bool fw_state = false;
        string[] fw_rule_list = ["To    Action    From"];
        string fw_rule = "22/tcp    Allow   192.168.0.10";

        // Command "service" variables
        string[] service_list = ["Service       Port"];
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

            // Command + Option + Object
            if (cd_ext.Length == 3)
            {
                if (cd_ext[0] == "fw")
                {
                    switch (cd_ext[2])
                    {
                        case "show":
                            if (fw_rule_list.Length == 1)
                            {
                                return new[]
                                {
                                    "No rules configured"
                                };
                            }
                            return fw_rule_list;

                        case "add":
                            return fw_rule_list.Append(fw_rule);

                        case "remove":
                            fw_rule_list = new string[] { fw_rule_list[0] };
                            return fw_rule_list;

                        default:
                            return new[]
                            {
                              "Invalid option"
                            };
                    }
                }

                if (cd_ext[0] == "service")
                {
                    if (cd_ext[1]=="start")
                    {
                        switch (cd_ext[2])
                        {
                            case "webservice":
                                return service_list.Append(webService);
                        
                            case "dataservice":
                                return service_list.Append(dataService); 
                        
                            case "apiservice":
                                return service_list.Append(apiService);

                            default:
                                return new[] 
                                {
                                    "Invalid service. Try 'service start' for a list of example services"
                                };
                        }
                    }
                    if (cd_ext[1] == "stop")
                    {
                        List<string> list_conv = service_list.ToList();
                        switch (cd_ext[2]) 
                        {
                            case "webservice":
                                list_conv.Remove(webService);
                                return list_conv.ToArray();

                            case "dataservice":
                                list_conv.Remove(dataService);
                                return list_conv.ToArray();

                            case "apiservice":
                                list_conv.Remove(apiService);
                                return list_conv.ToArray();

                            default:
                                return new[]
                                {
                                    "Invalid service. Try 'service start' for a list of example services"
                                };
                        }
                    }
                }
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
