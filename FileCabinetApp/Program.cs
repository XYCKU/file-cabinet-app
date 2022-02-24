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
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private const int ArgsAmount = 6;

        private static bool isRunning = true;

        private static IFileCabinetService fileCabinetService = new FileCabinetService(new DefaultValidator());

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "create", "creates new record", "The 'create' command creates new record." },
            new string[] { "edit", "edits exsisting record", "The 'edit' command edits exsisting record." },
            new string[] { "stat", "shows records statistics", "The 'stat' command shows records statistics." },
            new string[] { "list", "lists all records", "The 'list' command lists all records." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Console arguments.</param>
        public static void Main(string[] args)
        {
            ProcessArguments(args);
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine($"Using {fileCabinetService.ToString()} validation rules.");
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

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.OrdinalIgnoreCase));
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
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.OrdinalIgnoreCase));
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
            string[] args;
            bool isValid = true;

            do
            {
                if (!isValid)
                {
                    Console.WriteLine("Input arguments or q to exit");
                    var line = Console.ReadLine();

                    if (string.Equals(line, "q", StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    parameters = line != null ? line : string.Empty;
                }

                isValid = true;

                if (parameters is null)
                {
                    Console.WriteLine("Invalid arguments");
                    isValid = false;
                    continue;
                }

                args = parameters.Split(' ', ArgsAmount);

                if (args.Length < ArgsAmount)
                {
                    Console.WriteLine("Not enough arguments");
                    isValid = false;
                    continue;
                }

                for (int i = 0; i < ArgsAmount; ++i)
                {
                    if (string.IsNullOrWhiteSpace(args[i]))
                    {
                        Console.WriteLine($"args[{i}] is null or whitespace");
                        isValid = false;
                        break;
                    }
                }

                if (!isValid)
                {
                    continue;
                }

                args = parameters.Split(' ', ArgsAmount);
                int recordId;
                try
                {
                    DateTime dt = DateTime.Parse(args[2], new CultureInfo("en-US"));
                    short carAmount = short.Parse(args[3], CultureInfo.InvariantCulture);
                    decimal money = decimal.Parse(args[4], CultureInfo.InvariantCulture);
                    char favoriteChar = char.ToUpperInvariant(char.Parse(args[5]));

                    var data = new FileCabinetData(args[0], args[1], dt, carAmount, money, favoriteChar);

                    recordId = fileCabinetService.CreateRecord(data);

                    Console.WriteLine(LongFormatRecord(fileCabinetService.GetRecords()[recordId - 1]));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid arguments");
                    Console.WriteLine(e.Message);
                    isValid = false;
                }
            }
            while (!isValid);
        }

        private static void Edit(string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            try
            {
                int id = int.Parse(parameters, CultureInfo.InvariantCulture);

                if (fileCabinetService.GetStat() <= id)
                {
                    Console.WriteLine($"#{id} record is not found.");
                    return;
                }

                Console.Write("First name: ");
                string firstName = Console.ReadLine() ?? string.Empty;

                Console.Write("Last name: ");
                string lastName = Console.ReadLine() ?? string.Empty;

                Console.Write("Date of birth: ");
                DateTime dt = DateTime.Parse(Console.ReadLine() ?? string.Empty, new CultureInfo("en-US"));

                Console.Write("Car amount: ");
                short carAmount = short.Parse(Console.ReadLine() ?? string.Empty, CultureInfo.InvariantCulture);

                Console.Write("Money: ");
                decimal money = decimal.Parse(Console.ReadLine() ?? string.Empty, CultureInfo.InvariantCulture);

                Console.Write("Favorite char: ");
                char favoriteChar = char.ToUpperInvariant(char.Parse(Console.ReadLine() ?? string.Empty));

                var data = new FileCabinetData(firstName, lastName, dt, carAmount, money, favoriteChar);

                fileCabinetService.EditRecord(id, data);

                Console.WriteLine($"First name: {firstName}{Environment.NewLine} " +
                                                       $"Last name: {lastName}{Environment.NewLine}" +
                                                       $"Date of birth: {dt.ToString("MM/dd/YYYY", CultureInfo.InvariantCulture)}{Environment.NewLine}" +
                                                       $"Car amount: {carAmount}{Environment.NewLine}" +
                                                       $"Money: {money}{Environment.NewLine}" +
                                                       $"Favorite char: {favoriteChar}{Environment.NewLine}" +
                                                       $"Record #{id} is updated.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Find(string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            string[] args = parameters.Split(' ', 2);

            ReadOnlyCollection<FileCabinetRecord> result;

            if (string.IsNullOrWhiteSpace(args[1]))
            {
                Console.WriteLine("Invalid search argument");
                return;
            }

            string searchText;
            try
            {
                searchText = args[1].Split(new char[] { '"', '"' })[1];
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Invalid search argument");
                return;
            }

            switch (args[0].ToLowerInvariant())
            {
                case "firstname":
                    result = fileCabinetService.FindByFirstName(searchText);
                    break;
                case "lastname":
                    result = fileCabinetService.FindByLastName(searchText);
                    break;
                case "dateofbirth":
                    DateTime dt;
                    if (DateTime.TryParseExact(searchText, "yyyy-MMM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        result = fileCabinetService.FindByDateOfBirth(dt);
                    }
                    else
                    {
                        Console.WriteLine("Invalid date");
                        return;
                    }

                    break;
                default:
                    Console.WriteLine("Unknown search property");
                    return;
            }

            for (int i = 0; i < result.Count; ++i)
            {
                Console.WriteLine(FormatRecord(result[i]));
            }
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void List(string parameters)
        {
            var record = fileCabinetService.GetRecords();

            for (int i = 0; i < record.Count; ++i)
            {
                Console.WriteLine(FormatRecord(record[i]));
            }
        }

        private static string FormatRecord(FileCabinetRecord record) => $"#{record.Id}, " +
            $"{record.FirstName}, " +
            $"{record.LastName}, " +
            $"{record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, " +
            $"{record.CarAmount}, " +
            $"{record.Money}, " +
            $"{record.FavoriteChar}";

        private static string LongFormatRecord(FileCabinetRecord record) => $"First name: {record.FirstName}{Environment.NewLine}" +
            $"Last name: {record.LastName}{Environment.NewLine}" +
            $"Date of birth: {record.DateOfBirth.ToString("MM/dd/YYYY", CultureInfo.InvariantCulture)}{Environment.NewLine}" +
            $"Car amount: {record.CarAmount}{Environment.NewLine}" +
            $"Money: {record.Money}{Environment.NewLine}" +
            $"Favorite char: {record.Money}{Environment.NewLine}" +
            $"Record #{record.Id} is created.";

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static FileCabinetService GetCabinetService(string name) => name switch
        {
            "custom" => new FileCabinetService(new CustomValidator()),
            _ => new FileCabinetService(new DefaultValidator()),
        };

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
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

        private static void ProcessArguments(string[] args)
        {
            if (args == null)
            {
                fileCabinetService = GetCabinetService("default");
                return;
            }

            Dictionary<string, Action> arguments = new Dictionary<string, Action>()
            {
                { "--validation-rules=custom", () => { fileCabinetService = GetCabinetService("custom"); } },
            };

            Dictionary<string, Action<string>> valueArguments = new Dictionary<string, Action<string>>()
            {
                { "-v", (string value) => { fileCabinetService = GetCabinetService(value); } },
            };

            for (int i = 0; i < args.Length; ++i)
            {
                string lowerCaseArgument = args[i].ToLowerInvariant();

                if (arguments.ContainsKey(lowerCaseArgument))
                {
                    arguments[lowerCaseArgument]();
                }
                else if (valueArguments.ContainsKey(lowerCaseArgument))
                {
                    valueArguments[lowerCaseArgument](args[++i]);
                }
                else
                {
                    Console.WriteLine($"Invalid argument {args[i]}");
                }
            }

            if (fileCabinetService is null)
            {
                fileCabinetService = GetCabinetService("default");
            }
        }
    }
}