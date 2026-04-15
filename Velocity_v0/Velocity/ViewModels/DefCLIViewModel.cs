using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public string[] taken = ["", "", ""];
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
        bool _HasInput1;
        [ObservableProperty]
        string _UserAnswer1;
        
        [ObservableProperty]
        string _Q2;
        [ObservableProperty]
        string _Q2a;
        [ObservableProperty]
        bool _HasInput2;
        [ObservableProperty]
        string _UserAnswer2;

        [ObservableProperty]
        string _Q3;
        [ObservableProperty]
        string _Q3a;
        [ObservableProperty]
        bool _HasInput3;
        [ObservableProperty]
        string _UserAnswer3;


        [RelayCommand]
        public void Checker1()
        {
            if (HasInput1 == true)
            {
                Debug.WriteLine(Q1a, UserAnswer1);
                Answer_Check(Q1a, UserAnswer1);
            }
            else
            {
                Debug.WriteLine(Q1, HasInput1.ToString());
                Checker_Assign(Q1).Invoke();
            }
        }

        [RelayCommand]
        public void Checker2()
        {
            Checker_Assign(Q2).Invoke();
            
        }

        [RelayCommand]
        public void Checker3()
        {
            Checker_Assign(Q3).Invoke();
        }

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


        public bool Answer_Check(string answer, string userInput)
        {
            if (userInput == answer)
            {
                return true;
            }
            return false;
        }
        public bool FW_Up()
        {
            Debug.WriteLine("Code executed");
            DefCommands def = new DefCommands();
            bool fw_state = def.fw_state;
            if (fw_state==true)
            {
                return true;
            }
            Debug.WriteLine("You didnt do it");
            return false;
        }

        public bool Two_Services()
        {
            Debug.WriteLine("Code executed");
            DefCommands def = new DefCommands();
            string[] services = def.service_list;
            if (services.Length >= 3)
            {
                return true;
            }
            return false;
        }

        public bool Only_API()
        {
            Debug.WriteLine("Code executed");
            DefCommands def = new DefCommands();
            string api = def.apiService;
            string[] services = def.service_list;
            if (services.Length == 2)
            {
                if (fw_rule_list.Contains(api))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool One_Rule()
        {
            Debug.WriteLine("Code executed");
            if (fw_rule_list.Length == 2)
            {
                return true;
            }
            return false;
        }

        public Func<bool> Checker_Assign(string question)
        {
            return question switch
            {
                "Is the firewall up?" => FW_Up,
                "Enable at least 2 services" => Two_Services,
                "Ensure only the 'ApiService' is running" => Only_API,
                "Add a firewall rule" => One_Rule,
                _ => () => false
            };
        }
        public (string, bool) Q_Gen(Dictionary<string,string> bank, string question,string answer, bool InputShown)
        {
            if (bank[question].ToString() == String.Empty)
            {
                InputShown = false;
            }
            else
            {
                InputShown = true;
                answer = bank[question].ToString();
            }
            return (answer, InputShown);
        }

        public void DefQBank()
        {
            Dictionary<string,string> Q_bank = new Dictionary<string,string>()
            {
                {"Is the firewall up?", String.Empty },
                {"what is the command to turn the firewall off?","fw disable" },
                {"Enable at least 2 services", String.Empty},
                {"Ensure only the 'ApiService' is running",String.Empty},
                {"Add a firewall rule",String.Empty},
                {"How do I check the currently enabled/running services?","service show" },
                {"What is the port shown in the sample rule?","22" },
            };
            
            (string, string, string) No_Dupe() 
            {                 
                Random rand = new Random();
                string a = Q_bank.ElementAt(rand.Next(0, Q_bank.Count)).Key;
                string b = Q_bank.ElementAt(rand.Next(0, Q_bank.Count)).Key;
                string c = Q_bank.ElementAt(rand.Next(0, Q_bank.Count)).Key;
                if (a == b || a == b || b == c)
                {
                    return No_Dupe();
                }
                else
                {
                    return (a, b, c);
                }
            }
            (Q1, Q2, Q3) = No_Dupe();

            (Q1a, HasInput1)  = Q_Gen(Q_bank, Q1, Q1a, HasInput1);
            (Q2a, HasInput2) = Q_Gen(Q_bank, Q2, Q2a, HasInput2);
            (Q3a, HasInput3) = Q_Gen(Q_bank, Q3, Q3a, HasInput3);            
            Debug.WriteLine(Q1a);
            Debug.WriteLine(Q2a);
            Debug.WriteLine(Q3a);
            Debug.WriteLine(HasInput1.ToString());
            Debug.WriteLine(HasInput2.ToString());
            Debug.WriteLine(HasInput3.ToString());
        }
    }

    public class DefCommands
    {
        // Command "fw" variables
        public bool fw_state = false;
        string[] fw_rule_list = ["To    Action    From"];
        string fw_rule = "22/tcp    Allow   192.168.0.10";

        // Command "service" variables
        public string[] service_list = ["Service       Port"];
        string webService = "WebService    8080";
        string dataService = "DataService   5600";
        public string apiService = "ApiService    3000";
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