using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Velocity.ViewModels
{
    public partial class VelCLIViewModel : ViewModelBase
    {
        private readonly VelCommands _givenCommand = new();
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
            string userInput = "User@Tutorial> " + CommandInput;
            OutputLines.Add(userInput);
            foreach (var line in _givenCommand.Process(CommandInput))
            { 
                OutputLines.Add(line);
            }
            CommandInput = string.Empty;
        }
    }
    public class VelCommands
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
                        "Command1 - Description",
                        "Command2 - Description",
                        "\nGo ahead and run 'command1'",
                    },
                    "command1" => new[]
                    {
                        "Good! In most cases, it isn't just 'command1' and you're done, there are typically options like below:\n",
                        "Options:",
                        "option1 - this will perform an action",
                        "option2 - this will perform a different action\n",
                        "I promise the descriptions are much more helpful in the other labs!",
                        "For now, have a look around and try out different commands and options.",
                    },
                    "command2" => new[]
                    {
                        "Good! Curiosity is key to good learning.\n",
                        "Options:",
                        "option1 - return the state of 'security procedures'",
                        "up - change the state to 'on'",
                        "down - change the state to 'off'",
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
                if (cd_ext[1] == "command1")
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

                if (cd_ext[1] == "command2")
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
