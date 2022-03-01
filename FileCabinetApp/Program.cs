using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Console application class.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Vladislav Sharaev";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const string FileSystemPath = "cabinet-records.db";

        private static readonly Dictionary<string, Action> ConsoleVoidArguments = new Dictionary<string, Action>()
        {
            { "--validation-rules=custom", () => { Validator = GetCabinetServiceValidator("custom"); } },
        };

        private static readonly Dictionary<string, Action<string>> ConsoleStringArguments = new Dictionary<string, Action<string>>()
        {
            { "-v", (string value) => { Validator = GetCabinetServiceValidator(value); } },
            { "--storage", (string value) => { FileCabinetService = GetCabinetService(value); } },
        };

        /// <summary>
        /// Gets or sets a value indicating whether program is running.
        /// </summary>
        /// <value>Indicating whether program is running.</value>
        public static bool IsRunning { get; set; } = true;

        /// <summary>
        /// Gets or sets validator.
        /// </summary>
        /// <value>Validator.</value>
        public static IRecordValidator Validator { get; set; } = new DefaultValidator();

        /// <summary>
        /// Gets or sets fileCabinetService.
        /// </summary>
        /// <value>fileCabinetService.</value>
        public static IFileCabinetService FileCabinetService { get; set; } = new FileCabinetMemoryService(Validator);

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Console arguments.</param>
        public static void Main(string[] args)
        {
            ProcessArguments(args);
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine($"Using {Validator} validation rules.");
            Console.WriteLine($"Using {FileCabinetService} storage method.");
            Console.WriteLine();

            ICommandHandler commandHandler = CreateCommandHandler();

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

                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                commandHandler.Handle(new AppCommandRequest
                {
                    Command = command,
                    Parameters = parameters,
                });

                // PrintMissedCommandInfo(command);
            }
            while (IsRunning);
        }

        private static ICommandHandler CreateCommandHandler()
        {
            return new CommandHandler();
        }

        private static IRecordValidator GetCabinetServiceValidator(string name) => name switch
        {
            "custom" => new CustomValidator(),
            _ => new DefaultValidator(),
        };

        private static IFileCabinetService GetCabinetService(string name) => name switch
        {
            "file" => new FileCabinetFilesystemService(new FileStream(FileSystemPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite), Validator),
            _ => new FileCabinetMemoryService(Validator),
        };

        private static void ProcessArguments(string[] args)
        {
            if (args == null)
            {
                FileCabinetService = GetCabinetService("default");
                return;
            }

            for (int i = 0; i < args.Length; ++i)
            {
                string lowerCaseArgument = args[i].ToLowerInvariant();

                if (ConsoleVoidArguments.ContainsKey(lowerCaseArgument))
                {
                    ConsoleVoidArguments[lowerCaseArgument]();
                }
                else if (ConsoleStringArguments.ContainsKey(lowerCaseArgument))
                {
                    ConsoleStringArguments[lowerCaseArgument](args[++i]);
                }
                else
                {
                    Console.WriteLine($"Invalid argument {args[i]}");
                }
            }

            if (FileCabinetService is null)
            {
                FileCabinetService = GetCabinetService("default");
            }
        }
    }
}