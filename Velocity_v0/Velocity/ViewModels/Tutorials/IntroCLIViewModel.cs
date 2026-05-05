using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Velocity.ViewModels
{
    public partial class IntroCLIViewModel : ViewModelBase
    {
        public override double? SetWidth => 800;
        public override double? SetHeight => 575;

        private readonly MainWindowViewModel _main;
        public IntroCLIViewModel(MainWindowViewModel main)
        {
            _main = main;
        }

        private readonly IntroCommands _givenCommand = new();
        
        public ObservableCollection<string> OutputLines { get; } = new();


        [ObservableProperty]
        string _CommandInput;
        
        [ObservableProperty]
        private bool _MenuState = false;

        [ObservableProperty]
        string _Result1;
        [ObservableProperty]
        string _Result2;

        [RelayCommand]
        private void Execute() 
        { 
            if (string.IsNullOrWhiteSpace(CommandInput))
            {
                return;
            }
            string userInput = "User@Introduction> " + CommandInput;
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

        [RelayCommand]
        private void CoinToss1() 
        {
            string Option1 = "Checks executed, correct! (Maybe)";
            string Option2 = "Checks executed, incorrect. (Maybe)";

            Random rand = new Random();
            bool result = rand.Next(2) == 0;
            if (result == true)
            {
                Result1 = Option1;
            }
            else
            {
                Result1 = Option2;
            }
        }

        [RelayCommand]
        private void CoinToss2() 
        {

            string Option1 = "You gave an answer...I hope";
            string Option2 = "No right answer here, but good curiosity!";

            Random rand = new Random();
            bool result = rand.Next(2) == 0;
            if (result == true)
            {
                Result2 = Option1;
            }
            else
            {
                Result2 = Option2;
            }
        }
    }
    public class IntroCommands
    {
        bool state = false;


        public IEnumerable<string> Process(string command)
        {
            var cd = command.Trim().ToLowerInvariant();
            string[] cd_ext = cd.Split(' ');

            // Command
            if (cd_ext.Length == 1)
            {
                return cd switch
                {
                    "help" => new[]
                    {
                        "Every lab will have 'help' as its starting point,",
                        "this is so you can understand what the lab is about and what commmands are used within.",
                        "Here is what a typical 'help' command will look like:\n",
                        "Commands:",
                        "    command1 [ options ] - Description",
                        "    command2 [ options ] - Description",
                        "\nGo ahead and run 'command1'",
                    },
                    "command1" => new[]
                    {
                        "Good! In most cases, it isn't just 'command1' and you're done, there are typically options like below:\n",
                        "Options:",
                        "    option1 - this will perform an action",
                        "    option2 - this will perform a different action\n",
                        "I promise the descriptions are much more helpful in the other labs!",
                        "For now, have a look around and try out different commands and options.",
                    },
                    "command2" => new[]
                    {
                        "Good! Curiosity is key to good learning.\n",
                        "Options:",
                        "    state - return the state of 'security procedures'",
                        "    up - change the state to 'on'",
                        "    down - change the state to 'off'",
                    },
                    "easteregg" => new[]
                    {
                        "Sorry, I haven't coded a response for this one..."
                    },
                    _ => new[]
                    {
                        $"Unkown command: {command}"
                    }
                };
            }
            if (cd_ext.Length==2)
            {
                if (cd_ext[0] == "command1")
                {
                    switch (cd_ext[1])
                    {
                        case "option1":
                            return new[]
                            {
                                "You have performed an action"
                            };
                        case "option2":
                            return new[]
                            {
                                "You have performed a different action"
                            };
                        default:
                            break;
                    }
                }

                if (cd_ext[0] == "command2")
                {
                    switch (cd_ext[1])
                    {
                        case "state":
                            return new[] 
                            { 
                                $"{state}",
                                "This might be a clue for another lab...",
                            };
                        case "up":
                            state = true;
                            return new[] 
                            { 
                                "Security procedures enabled",
                                "This might be a clue for another lab..."
                            };
                        case "down":
                            state = false;
                            return new[] 
                            { 
                                "Security procedures disabled",
                                "This might be a clue for another lab..."
                            };
                            
                    }
                }
            }
            return new[]
            {
                "Invalid command. This message appears if you've mistyped a command, or tried a command not available in this lab"
            };
        }
    }
}
