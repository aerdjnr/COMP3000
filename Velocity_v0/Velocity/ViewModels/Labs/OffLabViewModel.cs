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
    public partial class OffLabViewModel : ViewModelBase
    {
        public override double? SetWidth => 800;
        public override double? SetHeight => 575;

        private readonly MainWindowViewModel _main;
        public OffLabViewModel(MainWindowViewModel main)
        {
            _main = main;
            OffLabQBank();
        }

        private readonly OffLabCommands _givenCommand = new();

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
            string userInput;
            if (_givenCommand.connected == true) { userInput = "\nShell1> " + CommandInput; }
            else { userInput = "\nUser@Offense> " + CommandInput; };

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
            if (userInput.ToLower() == answer)
            {
                return true;
            }
            return false;
        }

        // Assign the matching answer check based on the question generated for the user
        // last case is to catch all else, but "all else" is already handled so is left with a lambda function of false
        // This lab has no 'Checker' questions, but I am leaving this here in case of future question creation
        public Func<bool> Checker_Assign(string question)
        {
            return question switch
            {
                _ => () => false
            };
        }

        // Generates the relevant answer for the question, as well as if the input box needs to be hidden
        // if the question checks the state of variables,
        // no input is required and so the box is made invisible through axaml
        public (string, bool) Q_Gen(Dictionary<string, string> bank, string question, string answer, bool InputShown)
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
        public void OffLabQBank()
        {
            Dictionary<string, string> Q_bank = new Dictionary<string, string>()
            {
                { "How many total IP addresses are there in a network with '/25'?", "128" },
                { "How many total IP addresses are there in a network with '/24'?","256" },
                { "What host has port 445 open?", "192.168.32.133"},
                { "What port is open on the machine identified first in the network?","4500" },
                { "What protocol is running on the machine identified last in the network?","http" },
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
            (Q1a, HasInput1) = Q_Gen(Q_bank, Q1, Q1a, HasInput1);
            (Q2a, HasInput2) = Q_Gen(Q_bank, Q2, Q2a, HasInput2);
            (Q3a, HasInput3) = Q_Gen(Q_bank, Q3, Q3a, HasInput3);
        }
    }
    public class OffLabCommands
    {
        string[] sh_Exit()
        {
            connected = false;
            string[] text = { "Exiting Shell" };
            return text;
        }
        public string[] Discovered = [];
        string Shell_ID;
        string target;
        string payload;
        public bool connected;
        string[] host_port;
        public IEnumerable<string> Process(string command)
        {
            string[][] Network1 =
                [
                    ["192.168.32.128/25", "192.168.32.129", "192.168.32.133", "192.168.32.134"],
                    ["192.168.32.129","4500"],
                    ["192.168.32.133","445"],
                    ["192.168.32.134","80"]
                ];

            string[] NetworkFinder(string network)
            {
                if ((Network1[0][0] == network) || ("192.168.32.128" == network))
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
                return ["No network with that address in this lab"];
            }

            string[] PortFinder(string host)
            {
                if (Discovered.ToArray().Contains(host))
                {
                    if (Network1[0].ToArray().Contains(host))
                    {
                        // Stores and returns the matching host and port 'object'
                        string[] Found = Network1[Network1[0].ToArray().IndexOf(host)];
                        return Found;
                    }
                };
                return new[] { "No Scan" };
            }


            // Command splicing to specify command and necessary process required
            string cd = command.Trim().ToLowerInvariant();
            string[] cd_ext = cd.Split(' ');

            // Command
            if (cd_ext.Length == 1)
            {
                if (connected == true)
                {
                    return cd switch
                    {
                        "help" => new[]
                        {
                            "Commands:",
                            "   exit          - Disconnects from the current shell",
                            "   directory     - Lists files in the target machine",
                        },
                        "exit" => sh_Exit(),
                        "directory" => new[] 
                        { 
                            "secrets.txt     funnyCat.mp4     podcast.mp3     passwords.txt",
                            "folder1     avingersMovie.mp4     flag_1a2b3c"
                        },
                        _ => new[]
                        {
                            "Invalid command"
                        }
                    };
                }
                return cd switch
                {
                    "help" => new[]
                    {
                        "Commands:",
                        "   scan          [ network | host ] - Perform a scan on a given network",
                        "   exploit       [ target | payload | run ] - Configure exploit settings",
                        "   connect        - Access any active shells"
                    },
                    "scan" => new[]
                    {
                        "scan options:",
                        "    network - Scan a given network to find active machines",
                        "    host  - Scan discovered hosts for open ports"
                    },
                    "exploit" => new[]
                    {
                        "exploit options:",
                        "    target       [ host ] - Select the target machine to run the exploit on",
                        "    payload      [ payload ] - Set the payload for the exploit",
                        "    run - start the exploit with the given parameters"
                    },
                    "connect" => new[]
                    {
                        "Active Shells:",
                        Shell_ID
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
                if (connected == true) 
                { 
                
                }
                if (cd_ext[0] == "scan")
                {
                    switch (cd_ext[1])
                    {
                        case "network":
                            return new[]
                            {
                                "Here is the target network:",
                                "192.168.32.128/25",
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

                if (cd_ext[0] == "exploit")
                {
                    if (Discovered.Length == 0)
                    {
                        return new[] { "Very eager! Go find some hosts first" };
                    }
                    switch (cd_ext[1])
                    {
                        case "target":
                            return new[] { "Please provide a host" };
                        case "payload":
                            return new[] 
                            { 
                                "Please provide a payload from below (more in future builds):",
                                "EternalBlue" 
                            };
                        case "run":
                            if (target == String.Empty)
                            {
                                return new[] { "No target set" };
                            }

                            if (payload == String.Empty)
                            {
                                return new[] { "No payload set" };
                            }
                            Shell_ID = "Shell1";
                            return new[] { $"{Shell_ID} Created" };
                    
                        default:
                            return new[] { "No options given. Please try again" };
                    }
                }
                if (cd_ext[0] == "connect")
                {
                    if (Shell_ID == String.Empty) 
                    {
                        return new[] { "No active shells found" };
                    }
                    if (Shell_ID != String.Empty && cd_ext[1] == Shell_ID.ToLower())
                    {
                        connected = true;
                        return new[] 
                        { 
                            $"Connected to {host_port[0]}",
                            "Type 'help' for a list of available commands",
                        };
                    }
                    return new[]
                    {
                        "No shell provided. Please try again",
                        "Active Shells:",
                        Shell_ID
                    };
                }
                
                return cd switch
                {
                     _ => new[]
                    {
                        "Invalid command or option"
                     }
                };
            }

            // Command + Option + Target
            if (cd_ext.Length == 3)
            {
                if (cd_ext[0] == "scan") 
                { 
                    switch (cd_ext[1])
                    {
                        case "network":
                            if (cd_ext[2] == "192.168.32.128/25" || cd_ext[2] == "192.168.32.128")
                            {
                                if (Discovered.Length == 3)
                                {
                                    return new[] { "Discovered all possible networks (in this tutorial at least)" };
                                }
                                NetworkFinder(cd_ext[2]);
                                if (Discovered.Length == 3)
                                {
                                    return new[]
                                    {
                                        "Hosts Discovered:",
                                        Discovered[0],
                                        Discovered[1],
                                        Discovered[2],
                                    };
                                }
                                return new[]
                                {
                                    "Scanning error has occurred, not entirely sure what you did to get here."
                                };
                            }
                            return new[] { "That network isn't in this lab" };
                        
                        case "host":
                            host_port = PortFinder(cd_ext[2]);
                            if (host_port[0] == "No Scan")
                            {
                                return new[]
                                {
                                "No network associated with that host" 
                                };
                            }
                            return new[]
                            {
                                $"The following ports were found open at {host_port[0]} :",
                                $"{host_port[1]}"
                            };
                        default:
                            return new[] { $"Unknown Option '{cd_ext[1]}'" };
                    }
                }
                if (cd_ext[0] == "exploit")
                {
                    switch (cd_ext[1])
                    {
                        case "target":
                            if (cd_ext[2] == "192.168.32.133")
                            {
                                target = cd_ext[2];
                                return new[] { $"Target set: {target}" };
                            }
                            return new[] { "Invalid target" };

                        case "payload":
                            if (cd_ext[2] == "eternalblue")
                            {
                                payload = cd_ext[2];
                                return new[] { $"Payload set: {payload}" };
                            }
                            return new[] { "Invalid payload" };
                        default:
                            break;
                    }
                }
                if (cd_ext[0] == "connect")
                {

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