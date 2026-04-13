using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;



namespace Velocity.ViewModels
{
    public partial class DefCLIViewModel : ViewModelBase
    {
        public override double? SetWidth => 800;
        public override double? SetHeight => 575;

        private readonly MainWindowViewModel _main;
        public DefCLIViewModel(MainWindowViewModel main)
        {
            _main = main;
            DefQBank();
        }


        private readonly DefCommands _givenCommand = new();
        
        public ObservableCollection<string> OutputLines { get; } = new();


        public string[] fw_rule_list = ["To    Action    From"];
        public string[] q1a1;
        public string[] q2a2;
        public string[] q3a3;

        [ObservableProperty]
        string _CommandInput;

        [ObservableProperty]
        private bool _MenuState = false;

        [ObservableProperty]
        string _Q1;
        [ObservableProperty]
        string _Q1a;
        
        [ObservableProperty]
        string _Q2;
        [ObservableProperty]
        string _Q2a;

        [ObservableProperty]
        string _Q3;
        [ObservableProperty]
        string _Q3a;

        [ObservableProperty]
        bool _IsAnswer;


        [RelayCommand]
        private void Execute() 
        { 
            if (string.IsNullOrWhiteSpace(CommandInput))
            {
                return;
            }
            string userInput = "User@Defense> " + CommandInput;
            OutputLines.Add(userInput);
            foreach (var line in _givenCommand.Process(CommandInput))
            { 
                OutputLines.Add(line);
            }
            CommandInput = string.Empty;
        }

        [RelayCommand]
        private void MenuToggle()
        {
            MenuState = !MenuState;
        }

        [RelayCommand]
        private void GoBack()
        {
            _main.NavTut();
        }
        public void DefQBank()
        {
            Dictionary<string, Func<string>> Q_bank = new Dictionary<string, Func<string>>()
            {
                {"Is the firewall up?", () => string.Empty},
                {"what is the command to turn the firewall off?", () => "fw disable"},
                {"Enable at least 2 services",() =>string.Empty},
                {"Ensure only the 'ApiService' is running",() =>string.Empty},
                {"Add a firewall rule",() =>string.Empty},
                {"How do I check the currently enabled/running services?",() =>"service show"},
                {"Sample question7?",() =>"answer7"},
            };

            Random rand = new Random();
            Q1 = Q_bank.ElementAt(rand.Next(0, Q_bank.Count)).Key;
            Q2 = Q_bank.ElementAt(rand.Next(0, Q_bank.Count)).Key;
            Q3 = Q_bank.ElementAt(rand.Next(0, Q_bank.Count)).Key;
            
            Q1a = Q_bank[Q1].ToString();
            Q2a = Q_bank[Q2].ToString();
            Q3a = Q_bank[Q3].ToString();
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
                        "Commands:",
                        "    fw          [ status | enable | disable | rule ] - Configure the device's firewall",
                        "    service     [ show | start | stop ] - configure services",
                    },
                    "fw" => new[]
                    {
                        "fw options:",
                        "    status - displays firewall state",
                        "    enable - turns on the firewall",
                        "    disable - turns off the firewall",
                        "    rule - configure firewall rules",
                    },
                    "service" => new[]
                    {
                        "service options:",
                        "    show - list currently running services",
                        "    start - initiates a service",
                        "    stop - halts a service"
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
                                "fw rule [option]",
                                "        show - lists currently enabled rules",
                                "        add - inserts a sample rule",
                                "        remove - delete a sample rule"
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
                                "    WebService",
                                "    DataService",
                                "    ApiService",
                            };

                        case "stop":
                            return new[]
                            {
                                "Here is a list of example services to stop:",
                                "    WebService",
                                "    DataService",
                                "    ApiService",
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
            if (cd_ext.Length >= 3)
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
                            fw_rule_list = fw_rule_list.Append(fw_rule).ToArray();
                            return new[] { "Sample rule added, go and take a look!" };

                        case "remove":
                            fw_rule_list = new string[] { fw_rule_list[0] };
                            return new[] { "Sample rule removed, go and take a look!" };

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
                                service_list = service_list.Append(webService).ToArray();
                                return service_list;
                        
                            case "dataservice":
                                service_list = service_list.Append(dataService).ToArray();
                                return service_list;


                            case "apiservice":
                                service_list = service_list.Append(apiService).ToArray();
                                return service_list;


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