using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;



namespace Velocity.ViewModels
{
    public partial class InformCLIViewModel : ViewModelBase
    {
        public override double? SetWidth => 800;
        public override double? SetHeight => 575;

        private readonly MainWindowViewModel _main;
        public InformCLIViewModel(MainWindowViewModel main)
        {
            _main = main;
            InfoQBank();
        }

        private readonly InfoCommands _givenCommand = new();
        
        public ObservableCollection<string> OutputLines { get; } = new();


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
            string userInput = "User@Information> " + CommandInput;
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
            _main.NavTut();
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
            InfoCommands info = _givenCommand;
            return false;
        }

        public bool Two_Services()
        {
            InfoCommands info = _givenCommand;
            return false;
        }

        public bool Only_API()
        {
            InfoCommands info = _givenCommand;
            return false;
        }

        public bool One_Rule()
        {
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
        public void InfoQBank()
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

    public class InfoCommands
    {
        string[] Discovered = [];

        public IEnumerable<string> Process(string command)
        {
            string[][] Network1 =
                [
                    ["192.168.10.0/24","192.168.10.5","192.168.10.6","192.168.10.8"],
                    ["192.168.10.5","22"],["192.168.10.6","443"],["192.168.10.8","20","21"]
                ];

            string[][] Network2 =
                [
                    ["10.10.10.0/24", "10.10.10.2", "10.10.10.15", "10.10.10.32"],
                    ["10.10.10.2","80"],["10.10.10.15","445"],["10.10.10.32","514"]
                ];


            string[] FoundNetwork(string network)
            {
                if ((Network1[0][0]==network) || ("192.168.10.0" == network))
                {
                    if (Discovered.ToArray().Contains(Network1[0][1]))
                    {
                        return Discovered;
                    }
                    Discovered = Discovered.Append(Network1[0][1]).ToArray();
                    Discovered = Discovered.Append(Network1[0][2]).ToArray();
                    Discovered = Discovered.Append(Network1[0][3]).ToArray();
                    return Discovered;
                }

                if (Network2[0][0] == network || ("10.10.10.0" == network))
                {
                    if (Discovered.ToArray().Contains(Network2[0][1]))
                    {
                        return Discovered;
                    }
                    Discovered = Discovered.Append(Network2[0][1]).ToArray();
                    Discovered = Discovered.Append(Network2[0][2]).ToArray();
                    Discovered = Discovered.Append(Network2[0][3]).ToArray();
                    return Discovered;
                } 
                return ["No network with that address in this lab"];
            }
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
                        "   scan          [ network | host ] - Perform a scan on a given network",
                    },
                    "scan" => new[]
                    {
                        "scan options:",
                        "    network - Scan a given network to find active machines",
                        "    host  - Scan discovered hosts for open ports"
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
                if (cd_ext[0] == "scan")
                {
                    switch(cd_ext[1])
                    {
                        case "network":
                            return new[] 
                            {
                                "Here are a selection of simulated networks:",
                                "192.168.10.0/24",
                                "10.10.10.0/24",
                                "",
                                "Use the format 'scan network [ ip address ]'"
                            };

                        case "host":
                            return new[] { "filler text" };
                        
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
            if (cd_ext.Length == 3) 
            {
                if (cd_ext[1] == "network")
                {
                    switch (cd_ext[2])
                    {
                        case "192.168.10.0/24" or "192.168.10.0":
                            if (Discovered.Length == 6)
                            {
                                return new[] { "Discovered all possible networks (in the tutorial at least)" };
                            }
                            FoundNetwork(cd_ext[2]);
                            if (Discovered.Length == 3)
                            {
                                return new[]
                                {
                                    "Hosts Discovered",
                                    Discovered[0],
                                    Discovered[1],
                                    Discovered[2],
                                }; 
                            }
                            return new[]
                            {
                                "Hosts Discovered",
                                Discovered[0],
                                Discovered[1],
                                Discovered[2],
                                Discovered[3],
                                Discovered[4],
                                Discovered[5],
                            };
                        
                        case "10.10.10.0/24" or "10.10.10.0":
                            if (Discovered.Length == 6)
                            {
                                return new[] { "Discovered all possible networks (in the tutorial at least)" };
                            }
                            FoundNetwork(cd_ext[2]);
                            Debug.WriteLine(Discovered.Length.ToString());
                            if (Discovered.Length == 3)
                            {
                                return new[]
                                {
                                    "Hosts Discovered",
                                    Discovered[0],
                                    Discovered[1],
                                    Discovered[2],
                                };
                            }
                            return new[]
                            {
                                "Hosts Discovered",
                                Discovered[0],
                                Discovered[1],
                                Discovered[2],
                                Discovered[3],
                                Discovered[4],
                                Discovered[5],
                            };

                        default:
                            return new[] { "Invalid Selection" };
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