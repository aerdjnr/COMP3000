using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
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
        public bool One_Net()
        {
            InfoCommands info = _givenCommand;
            if (info.Discovered.Length >= 3)
            {
                return true;
            }
            return false;
        }

        public bool Four_Hosts()
        {
            InfoCommands info = _givenCommand;
            if (info.Scan_Count >= 4)
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
                "Scan at least 1 network" => One_Net,
                "Scan at least 4 hosts" => Four_Hosts,
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
                { "Scan at least 1 network", String.Empty },
                { "What host has port 22 open?","192.168.10.5" },
                { "What host has port 445 open?", "10.10.10.15"},
                { "How many hosts are in any of the networks?", "3"},
                { "Scan at least 4 hosts",String.Empty},
                { "What would the command be to scan the network 1.2.3.4?","scan network 1.2.3.4" },
                { "What protocol typically runs over port 22?","ssh" },
                { "What protocol typically runs over port 443?","https" },
                { "What port is typically used for the 'Syslog' protocol?","514" },
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
        public string[] Discovered = [];
        public int Scan_Count;
        public IEnumerable<string> Process(string command)
        {
            string[][] Network1 =
                [
                    ["192.168.10.0/24","192.168.10.5","192.168.10.6","192.168.10.8"],
                    ["192.168.10.5","22"],
                    ["192.168.10.6","443"],
                    ["192.168.10.8","389"]
                ];

            string[][] Network2 =
                [
                    ["10.10.10.0/24", "10.10.10.2", "10.10.10.15", "10.10.10.32"],
                    ["10.10.10.2","80"],
                    ["10.10.10.15","445"],
                    ["10.10.10.32","514"]
                ];


            string[] NetworkFinder(string network)
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

            string[] PortFinder(string host)
            {
                if (Discovered.ToArray().Contains(host))
                {
                    if (Network1[0].ToArray().Contains(host))
                    {
                        Scan_Count += 1;
                        // Stores and returns the matching host and port 'object'
                        string[] Found = Network1[Network1[0].ToArray().IndexOf(host)];
                        return Found;
                    }
                    if (Network2[0].ToArray().Contains(host))
                    {
                        Scan_Count += 1;
                        string[] Found = Network2[Network2[0].ToArray().IndexOf(host)];
                        return Found;
                    }
                };
                return new[] {"No Scan"}; 
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
                                "Use the format 'scan network [ ip address subnet ]'"
                            };

                        case "host":
                            return new[] 
                            { 
                                "scan host [ ip address ] - This will look for any open ports!",
                                "If you haven't found any yet, go ahead and investigate the 'scan network' command"
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
                            NetworkFinder(cd_ext[2]);
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
                            NetworkFinder(cd_ext[2]);
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
                if (cd_ext[1]== "host")
                {
                    string[] host_port = PortFinder(cd_ext[2]);
                    if (host_port[0] == "No scan") 
                    {
                        return new[]
                        {
                            "No network associated with that host"
                        };
                    }
                    return new[] 
                    { 
                        "The following ports were found open:",
                        $"{host_port[1]}" 
                    };
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