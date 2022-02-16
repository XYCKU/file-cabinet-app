﻿using System.Globalization;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Vladislav Sharaev";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;

        private static FileCabinetService fileCabinetService = new FileCabinetService();

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "create", "creates new record", "The 'stat' command creates new record." },
            new string[] { "stat", "shows records statistics", "The 'stat' command shows records statistics." },
            new string[] { "list", "lists all records", "The 'stat' command lists all records." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Create(string parameters)
        {
            string[] args = parameters.Split(' ', 3);

            if (args is null)
            {
                Console.WriteLine("Arguments are null");
                return;
            }

            if (string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("FirstName is null");
                return;
            }

            if (string.IsNullOrEmpty(args[1]))
            {
                Console.WriteLine("LastName is null");
                return;
            }

            if (string.IsNullOrEmpty(args[2]))
            {
                Console.WriteLine("DateOfBirth is null");
                return;
            }

            DateTime dt;

            try
            {
                dt = DateTime.ParseExact(args[2], "MM/dd/YYYY", CultureInfo.CurrentCulture);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if (!DateTime.TryParse(args[2], out dt))
            {
                Console.WriteLine("DateOfBirth has invalid format");
                return;
            }

            int recordId = fileCabinetService.CreateRecord(args[0], args[1], dt);

            Console.WriteLine($"First name: {args[0]}{Environment.NewLine}" +
                                $"Last name: {args[1]}{Environment.NewLine}" +
                                $"Date of birth: {args[2]}{Environment.NewLine}" +
                                $"Record #{recordId} is created.");
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void List(string parameters)
        {
            var record = fileCabinetService.GetRecords();

            for (int i = 0; i < record.Length; ++i)
            {
                Console.WriteLine(FormatRecord(record[i]));
            }
        }

        private static string FormatRecord(FileCabinetRecord record) => $"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd")}";

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}