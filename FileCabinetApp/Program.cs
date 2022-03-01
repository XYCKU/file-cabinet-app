using System.Collections.ObjectModel;
using System.Globalization;
using FileCabinetApp.CommandHandlers;

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

            ICommandHandler commandHandler = GetCommandHandlers();

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

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Tuple with result.</returns>
        public static Tuple<bool, string, string> StringConverter(string input)
        {
            return new Tuple<bool, string, string>(true, string.Empty, input);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Tuple with result.</returns>
        public static Tuple<bool, string, DateTime> DateConverter(string input)
        {
            return new Tuple<bool, string, DateTime>(
                DateTime.TryParseExact(input, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result),
                "Invalid date of birth.",
                result);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Tuple with result.</returns>
        public static Tuple<bool, string, char> CharConverter(string input)
        {
            return new Tuple<bool, string, char>(char.TryParse(input, out char result), "Invalid char.", char.ToUpperInvariant(result));
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Tuple with result.</returns>
        public static Tuple<bool, string, short> ShortConverter(string input)
        {
            return new Tuple<bool, string, short>(short.TryParse(input, out short result), input, result);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Tuple with result.</returns>
        public static Tuple<bool, string, decimal> DecimalConverter(string input)
        {
            return new Tuple<bool, string, decimal>(decimal.TryParse(input, out decimal result), input, result);
        }

        /// <summary>
        /// Validates given parameter.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Returns is input valid.</returns>
        public static Tuple<bool, string> FirstNameValidator(string firstName)
        {
            try
            {
                Program.Validator.ValidateFirstName(firstName);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>
        /// Validates given parameter.
        /// </summary>
        /// <param name="lastName">Last name.</param>
        /// <returns>Returns is input valid.</returns>
        public static Tuple<bool, string> LastNameValidator(string lastName)
        {
            try
            {
                Program.Validator.ValidateLastName(lastName);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>
        /// Validates given parameter.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns>Returns is input valid.</returns>
        public static Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            try
            {
                Program.Validator.ValidateDateOfBirth(dateOfBirth);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>
        /// Validates given parameter.
        /// </summary>
        /// <param name="carAmount">Car amount.</param>
        /// <returns>Returns is input valid.</returns>
        public static Tuple<bool, string> CarAmountValidator(short carAmount)
        {
            try
            {
                Program.Validator.ValidateCarAmount(carAmount);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>
        /// Validates given parameter.
        /// </summary>
        /// <param name="money">Money.</param>
        /// <returns>Returns is input valid.</returns>
        public static Tuple<bool, string> MoneyValidator(decimal money)
        {
            try
            {
                Program.Validator.ValidateMoney(money);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>
        /// Validates given parameter.
        /// </summary>
        /// <param name="favoriteChar">Favorite char.</param>
        /// <returns>Returns is input valid.</returns>
        public static Tuple<bool, string> FavoriteCharValidator(char favoriteChar)
        {
            try
            {
                Program.Validator.ValidateFavoriteChar(favoriteChar);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
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

        private static ICommandHandler GetCommandHandlers()
        {
            var handlers = new ICommandHandler[]
            {
                new HelpCommandHandler(),
                new CreateCommandHandler(),
                new EditCommandHandler(),
                new FindCommandHandler(),
                new ListCommandHandler(),
                new StatCommandHandler(),
                new ExportCommandHandler(),
                new ImportCommandHandler(),
                new PurgeCommandHandler(),
                new RemoveCommandHandler(),
                new ExitCommandHandler(),
                new MissedCommandHandler(),
            };

            for (int i = 0; i < handlers.Length - 1; ++i)
            {
                handlers[i].SetNext(handlers[i + 1]);
            }

            return handlers[0];
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