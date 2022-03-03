using System.Globalization;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Console application class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Dafault date format.
        /// </summary>
        public const string DateTimeFormat = "MM/dd/yyyy";

        private const string DeveloperName = "Vladislav Sharaev";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const string FileSystemPath = "cabinet-records.db";

        private static readonly Dictionary<string, Action> ConsoleVoidArguments = new Dictionary<string, Action>()
        {
            { "--validation-rules=custom", () => { validatorType = GetValidatorStringType("custom"); } },
        };

        private static readonly Dictionary<string, Action<string>> ConsoleStringArguments = new Dictionary<string, Action<string>>()
        {
            { "-v", (string value) => { validatorType = GetValidatorStringType(value); } },
            { "--storage", (string value) => { cabinetType = GetCabinetStringType(value); } },
        };

        private static bool isRunning = true;

        private static string cabinetType = "memory";
        private static string validatorType = "default";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private static IRecordValidator validator;
        private static IInputValidator inputValidator;

        /// <summary>
        /// Gets or sets fileCabinetService.
        /// </summary>
        /// <value>fileCabinetService.</value>
        public static IFileCabinetService FileCabinetService { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Console arguments.</param>
        public static void Main(string[] args)
        {
            ProcessArguments(args);

            validator = GetValidatorType(validatorType);
            inputValidator = GetInputValidatorType(validatorType);
            FileCabinetService = GetCabinetType(cabinetType);

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine($"Using {validatorType} validation rules.");
            Console.WriteLine($"Using {FileCabinetService} storage method.");
            Console.WriteLine();

            ICommandHandler commandHandler = GetCommandHandlers(FileCabinetService);

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
            }
            while (isRunning);
        }

        /// <summary>
        /// Reads input, converts and validates it.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="converter">Converter.</param>
        /// <param name="validator">Validator.</param>
        /// <returns>Valid result.</returns>
        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine() ?? string.Empty;
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        /// <summary>
        /// Format record to short variant.
        /// </summary>
        /// <param name="record">Record.</param>
        /// <param name="id">Id of a record.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatRecord(FileCabinetData record, int id) => $"#{id}, " +
            $"{record.FirstName}, " +
            $"{record.LastName}, " +
            $"{record.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture)}, " +
            $"{record.CarAmount}, " +
            $"{record.Money}, " +
            $"{record.FavoriteChar}";

        /// <summary>
        /// Format record to short variant.
        /// </summary>
        /// <param name="record">Record.</param>
        /// <param name="id">Id of a record.</param>
        /// <param name="pastAction">Action done to record.</param>
        /// <returns>Formatted string.</returns>
        public static string LongFormatRecord(FileCabinetData record, int id, string pastAction) => $"First name: {record.FirstName}{Environment.NewLine}" +
            $"Last name: {record.LastName}{Environment.NewLine}" +
            $"Date of birth: {record.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture)}{Environment.NewLine}" +
            $"Car amount: {record.CarAmount}{Environment.NewLine}" +
            $"Money: {record.Money}{Environment.NewLine}" +
            $"Favorite char: {record.Money}{Environment.NewLine}" +
            $"Record #{id} is {pastAction}.";

        private static ICommandHandler GetCommandHandlers(IFileCabinetService service)
        {
            var printer = new DefaultRecordPrinter();

            var handlers = new ICommandHandler[]
            {
                new HelpCommandHandler(),
                new CreateCommandHandler(service),
                new EditCommandHandler(service),
                new FindCommandHandler(service, printer),
                new ListCommandHandler(service, printer),
                new StatCommandHandler(service),
                new ExportCommandHandler(service),
                new ImportCommandHandler(service),
                new PurgeCommandHandler(service),
                new RemoveCommandHandler(service),
                new ExitCommandHandler((bool state) => isRunning = state),
                new MissedCommandHandler(),
            };

            for (int i = 0; i < handlers.Length - 1; ++i)
            {
                handlers[i].SetNext(handlers[i + 1]);
            }

            return handlers[0];
        }

        private static string GetValidatorStringType(string name) => name.ToLowerInvariant() switch
        {
            "custom" => "custom",
            _ => "default",
        };

        private static string GetCabinetStringType(string name) => name.ToLowerInvariant() switch
        {
            "file" => "file",
            _ => "memory",
        };

        private static IRecordValidator GetValidatorType(string name) => name switch
        {
            "custom" => new ValidatorBuilder().CreateCustom(),
            _ => new ValidatorBuilder().CreateDefault(),
        };

        private static IInputValidator GetInputValidatorType(string name) => name switch
        {
            "custom" => new CustomInputValidator(),
            _ => new DefaultInputValidator(),
        };

        private static IFileCabinetService GetCabinetType(string name) => name switch
        {
            "file" => new FileCabinetFilesystemService(new FileStream(FileSystemPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite), validator, inputValidator),
            _ => new FileCabinetMemoryService(validator, inputValidator),
        };

        private static void ProcessArguments(string[] args)
        {
            if (args == null)
            {
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
        }
    }
}