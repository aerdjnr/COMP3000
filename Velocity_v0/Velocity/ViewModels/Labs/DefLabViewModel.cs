using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;



namespace Velocity.ViewModels
{
    public partial class DefLabViewModel : ViewModelBase
    {
        public override double? SetWidth => 800;
        public override double? SetHeight => 575;

        private readonly MainWindowViewModel _main;
        public DefLabViewModel(MainWindowViewModel main)
        {
            _main = main;
            DefLabQBank();
        }


        private readonly DefCommands _givenCommand = new();
        
        public ObservableCollection<string> OutputLines { get; } = new();

        public string[] fw_rule_list = ["To    Action    From"];


        [ObservableProperty]
        string _CommandInput;

        // Sidebar state, default off
        [ObservableProperty]
        private bool _MenuState = false;

        // Question 1 variables 
        [ObservableProperty]
        string _Q1;
        [ObservableProperty]
        string _Q1a;
        [ObservableProperty]
        bool _Q1Correct;
        [ObservableProperty]
        bool _Q1Incorrect;
        [ObservableProperty]
        bool _HasInput1;
        [ObservableProperty]
        string _UserAnswer1;

        // Question 2 variables 
        [ObservableProperty]
        string _Q2;
        [ObservableProperty]
        string _Q2a;
        [ObservableProperty]
        bool _Q2Correct;
        [ObservableProperty]
        bool _Q2Incorrect;
        [ObservableProperty]
        bool _HasInput2;
        [ObservableProperty]
        string _UserAnswer2;

        // Question 3 variables 
        [ObservableProperty]
        string _Q3;
        [ObservableProperty]
        string _Q3a;
        [ObservableProperty]
        bool _Q3Correct;
        [ObservableProperty]
        bool _Q3Incorrect;
        [ObservableProperty]
        bool _HasInput3;
        [ObservableProperty]
        string _UserAnswer3;

        // Checks answer for question 1
        [RelayCommand]
        public void Checker1()
        {
            if (HasInput1 == true)
            {
                Q1Correct = Answer_Check(Q1a, UserAnswer1);
                Q1Incorrect = !Q1Correct;
            }
            else
            {
                Q1Correct = Checker_Assign(Q1).Invoke();
                Q1Incorrect = !Q1Correct;
            }
        }

        // Checks answer for question 2
        [RelayCommand]
        public void Checker2()
        {
            if (HasInput2 == true)
            {
                Q2Correct = Answer_Check(Q2a, UserAnswer2);
                Q1Incorrect = !Q1Correct;
            }
            else
            {
                Q2Correct = Checker_Assign(Q2).Invoke();
                Q2Incorrect = !Q2Correct;
            }
        }

        // Checks answer for question 3
        [RelayCommand]
        public void Checker3()
        {
            if (HasInput3 == true)
            {
                Q3Correct = Answer_Check(Q3a, UserAnswer3);
                Q3Incorrect = !Q3Correct;
            }
            else
            {
                Q3Correct = Checker_Assign(Q3).Invoke();
                Q3Incorrect = !Q3Correct;
            }
        }

        // Where commands are executed
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

        // Sidebar toggle
        [RelayCommand]
        private void MenuToggle()
        {
            MenuState = !MenuState;
        }
        
        // Back to home page button
        [RelayCommand]
        private void GoBack()
        {
            _main.NavRem();
        }


        // Answer checker for questions with input boxes
        public bool Answer_Check(string answer, string userInput)
        {
            if (userInput == answer)
            {
                return true;
            }
            return false;
        }


        // All answer checks for questions with no input boxes
        public bool FW_Up()
        {
            DefCommands def = _givenCommand;
            bool fw_state = def.fw_state;
            if (fw_state==true)
            {
                return true;
            }
            return false;
        }

        public bool Two_Services()
        {
            DefCommands def = _givenCommand;
            string[] services = def.service_list;
            if (services.Length >= 3)
            {
                return true;
            }
            return false;
        }

        public bool Only_API()
        {
            DefCommands def = _givenCommand;
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
            if (fw_rule_list.Length == 2)
            {
                return true;
            }
            return false;
        }

        // Assign the matching answer check based on the question generated for the user
        // last case is to catch all else, but "all else" is already handled so is left with a lambda function of false
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

        // Generates the relevant answer for the question, as well as if the input box needs to be hidden
        // if the question checks the state of variables,
        // no input is required and so the box is made invisible through axaml
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

        // Question bank for all the questions in this tutorial lab
        public void DefLabQBank()
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
            
            // Recursively assigning questions to variables using a tuple, ensuring that they are all unique
            (string, string, string) No_Dupe() 
            {


                Random rand = new Random();
                string a = Q_bank.ElementAt(rand.Next(0, Q_bank.Count)).Key;
                string b = Q_bank.ElementAt(rand.Next(0, Q_bank.Count)).Key;
                string c = Q_bank.ElementAt(rand.Next(0, Q_bank.Count)).Key;
                if ((a == b) || (a == c) || (b == c))
                {
                    return No_Dupe();
                }
                else
                {
                    return (a, b, c);
                }
                
            }

            // Assigning all 3 questions at once
            (Q1, Q2, Q3) = No_Dupe();

            // Similar process for the question answers
            (Q1a, HasInput1)  = Q_Gen(Q_bank, Q1, Q1a, HasInput1);
            (Q2a, HasInput2) = Q_Gen(Q_bank, Q2, Q2a, HasInput2);
            (Q3a, HasInput3) = Q_Gen(Q_bank, Q3, Q3a, HasInput3);            
        }
    }

    public class DefLabCommands
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
                    $"Unkown command: {command}"
                }
            };
        }
    }
}