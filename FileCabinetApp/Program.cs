using System.Globalization;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.Config;
using FileCabinetApp.Validators.Input;

namespace FileCabinetApp
{
    /// <summary>
    /// Console application class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Dafault culture format.
        /// </summary>
        public static readonly CultureInfo DefaultCulture = new CultureInfo("en-US");

        private const string DeveloperName = "Vladislav Sharaev";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const string FileSystemPath = "cabinet-records.db";
        private const string ConfigFilePath = "validation-rules.json";
        private const string LogFileNameFormat = @"logs\cabinet-log-{0}.log";

        private static readonly Dictionary<string, Action> ConsoleVoidArguments = new Dictionary<string, Action>()
        {
            { "--validation-rules=custom", () => { validatorType = GetValidatorStringType("custom"); } },
            { "use-stopwatch", () => { useStopwatch = true; } },
            { "use-logger", () => { useLogger = true; } },
        };

        private static readonly Dictionary<string, Action<string>> ConsoleStringArguments = new Dictionary<string, Action<string>>()
        {
            { "-v", (string value) => { validatorType = GetValidatorStringType(value); } },
            { "--storage", (string value) => { cabinetType = GetCabinetStringType(value); } },
        };

        private static bool isRunning = true;

        private static string cabinetType = "memory";
        private static string validatorType = "default";
        private static bool useStopwatch;
        private static bool useLogger;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private static IRecordValidator validator;
        private static IInputValidator inputValidator;
        private static Dictionary<string, ConfigurationData> configs;

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
            SetupConfigs(ConfigFilePath);
            ProcessArguments(args);

            ConfigurationData config = GetValidatorConfig(validatorType);
            validator = new ValidatorBuilder().Create(config);
            inputValidator = new InputValidator(config);
            FileCabinetService = GetCabinetType(cabinetType);

            string logFileName = GetLogFileName(LogFileNameFormat);

            if (!Directory.Exists(logFileName))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logFileName) ?? @"logs\");
            }

            using TextWriter logWriter = new StreamWriter(logFileName);

            if (useStopwatch)
            {
                FileCabinetService = new ServiceMeter(FileCabinetService);
            }

            if (useLogger)
            {
                FileCabinetService = new ServiceLogger(FileCabinetService, logWriter);
            }
            else
            {
                logWriter.Close();
                File.Delete(logFileName);
            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine($"Using {validatorType} validation rules.");
            Console.WriteLine($"Using {FileCabinetService} storage method.");
            if (useStopwatch)
            {
                Console.WriteLine($"Using stopwatch.");
            }

            if (useLogger)
            {
                Console.WriteLine($"Using logger.");
            }

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
            $"{record.DateOfBirth.ToString("MM/dd/yyyy", DefaultCulture)}, " +
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
            $"Date of birth: {record.DateOfBirth.ToString("MM/dd/yyyy", DefaultCulture)}{Environment.NewLine}" +
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

        private static void SetupConfigs(string path)
        {
            try
            {
                using (var streamReader = new StreamReader(path))
                {
                    IConfigReader configReader = new JsonConfigReader(streamReader);
                    configs = configReader.ReadAllConfigs();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Config parsing error: {e.Message}");
            }
        }

        private static ConfigurationData GetValidatorConfig(string name)
        {
            if (configs.ContainsKey(name))
            {
                return configs[name];
            }

            return configs.First().Value;
        }

        private static IFileCabinetService GetCabinetType(string name) => name switch
        {
            "file" => new FileCabinetFilesystemService(new FileStream(FileSystemPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite), validator, inputValidator),
            _ => new FileCabinetMemoryService(validator, inputValidator),
        };

        private static string GetLogFileName(string format)
        {
            return string.Format(DefaultCulture, format, DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss", DefaultCulture));
        }

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