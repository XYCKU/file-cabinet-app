﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class CommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private const string DateTimeFormat = "MM/dd/yyyy";

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
            new Tuple<string, Action<string>>("remove", Remove),
            new Tuple<string, Action<string>>("purge", Purge),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "create", "creates new record", "The 'create' command creates new record." },
            new string[] { "edit", "edits exsisting record", "The 'edit' command edits exsisting record." },
            new string[] { "stat", "shows records statistics", "The 'stat' command shows records statistics." },
            new string[] { "list", "lists all records", "The 'list' command lists all records." },
            new string[] { "export", "exports all records", "The 'export' command exports all records." },
            new string[] { "import", "imports records from file", "The 'import' command imports records from file." },
            new string[] { "remove", "removes record", "The 'remove' command removes record." },
            new string[] { "purge", "defragments FileCabinetFilesystemService file", "The 'purge' command defragments FileCabinetFilesystemService file." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(commandRequest.Command, StringComparison.OrdinalIgnoreCase));

            if (index >= 0)
            {
                commands[index].Item2(commandRequest.Parameters);
            }
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
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Create(string parameters)
        {
            int recordId;

            Console.Write("First name: ");
            string firstName = ReadInput(StringConverter, FirstNameValidator);

            Console.Write("Last name: ");
            string lastName = ReadInput(StringConverter, LastNameValidator);

            Console.Write("Date of birth: ");
            DateTime dt = ReadInput(DateConverter, DateOfBirthValidator);

            Console.Write("Car amount: ");
            short carAmount = ReadInput(ShortConverter, CarAmountValidator);

            Console.Write("Money: ");
            decimal money = ReadInput(DecimalConverter, MoneyValidator);

            Console.Write("Favorite char: ");
            char favoriteChar = ReadInput(CharConverter, FavoriteCharValidator);

            var data = new FileCabinetData(firstName, lastName, dt, carAmount, money, favoriteChar);

            recordId = Program.FileCabinetService.CreateRecord(data);

            Console.WriteLine(LongFormatRecord(data, recordId, "created"));
        }

        private static void Edit(string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            int id;
            if (!int.TryParse(parameters, out id))
            {
                Console.WriteLine("Invalid id format");
                return;
            }

            if (id < 0)
            {
                Console.WriteLine($"{id} id is invalid.");
                return;
            }

            if (id >= Program.FileCabinetService.GetStat().Item1)
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            Console.Write("First name: ");
            string firstName = ReadInput(StringConverter, FirstNameValidator);

            Console.Write("Last name: ");
            string lastName = ReadInput(StringConverter, LastNameValidator);

            Console.Write("Date of birth: ");
            DateTime dt = ReadInput(DateConverter, DateOfBirthValidator);

            Console.Write("Car amount: ");
            short carAmount = ReadInput(ShortConverter, CarAmountValidator);

            Console.Write("Money: ");
            decimal money = ReadInput(DecimalConverter, MoneyValidator);

            Console.Write("Favorite char: ");
            char favoriteChar = ReadInput(CharConverter, FavoriteCharValidator);

            var data = new FileCabinetData(firstName, lastName, dt, carAmount, money, favoriteChar);

            Program.FileCabinetService.EditRecord(id, data);

            Console.WriteLine(LongFormatRecord(data, id, "updated"));
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
                    result = Program.FileCabinetService.FindByFirstName(searchText);
                    break;
                case "lastname":
                    result = Program.FileCabinetService.FindByLastName(searchText);
                    break;
                case "dateofbirth":
                    DateTime dt;
                    if (DateTime.TryParseExact(searchText, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        result = Program.FileCabinetService.FindByDateOfBirth(dt);
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
                Console.WriteLine(result[i]);
            }
        }

        private static void Stat(string parameters)
        {
            var stat = Program.FileCabinetService.GetStat();
            Console.WriteLine($"{stat.Item1} record(s).");
            Console.WriteLine($"{stat.Item2} record(s) are deleted.");
        }

        private static void List(string parameters)
        {
            var records = Program.FileCabinetService.GetRecords();

            for (int i = 0; i < records.Count; ++i)
            {
                Console.WriteLine(records[i]);
            }
        }

        private static void Export(string parameters)
        {
            var fileCabinet = Program.FileCabinetService as FileCabinetMemoryService;

            if (fileCabinet is null)
            {
                Console.WriteLine("File cabinet is not memory type");
                return;
            }

            if (string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            const int ArgumentsAmount = 2;
            string[] args = parameters.Split(' ', ArgumentsAmount, StringSplitOptions.RemoveEmptyEntries);

            if (args is null)
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            if (args.Length != ArgumentsAmount)
            {
                Console.WriteLine("Invalid amount of arguments");
                return;
            }

            string exportType = args[0].ToLowerInvariant();
            string path = args[1];

            if (File.Exists(path))
            {
                Console.Write($"File is exist - rewrite {path}? [Y/n] ");
                string answer = Console.ReadLine() ?? string.Empty;

                const string positiveAnswer = "y";

                if (!string.Equals(answer, positiveAnswer, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    FileCabinetServiceSnapshot snapshot = fileCabinet.MakeSnapshot();

                    switch (exportType)
                    {
                        case "csv":
                            snapshot.SaveToCsv(writer);
                            break;
                        case "xml":
                            snapshot.SaveToXml(writer);
                            break;
                        default:
                            Console.WriteLine("Unknown export format");
                            return;
                    }
                }
            }
            catch
            {
                Console.WriteLine($"Export failed: can't open file {path}.");
                return;
            }

            Console.WriteLine($"All records are exported to file {Path.GetFileName(path)}.");
        }

        private static void Import(string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            const int ArgumentsAmount = 2;
            string[] args = parameters.Split(' ', ArgumentsAmount, StringSplitOptions.RemoveEmptyEntries);

            if (args is null)
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            if (args.Length != ArgumentsAmount)
            {
                Console.WriteLine("Invalid amount of arguments");
                return;
            }

            string importType = args[0].ToLowerInvariant();
            string path = args[1];

            try
            {
                using (var reader = new StreamReader(path))
                {
                    FileCabinetServiceSnapshot snapshot = Program.FileCabinetService.MakeSnapshot();

                    switch (importType)
                    {
                        case "csv":
                            snapshot.LoadFromCsv(reader);
                            break;
                        case "xml":
                            snapshot.LoadFromXml(reader);
                            break;
                        default:
                            Console.WriteLine("Unknown export format");
                            return;
                    }

                    Program.FileCabinetService.Restore(snapshot);
                }
            }
            catch
            {
                Console.WriteLine($"Import failed: can't open file {path}.");
                return;
            }

            Console.WriteLine($"All records are imported from file {Path.GetFileName(path)}.");
        }

        private static void Remove(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                Console.WriteLine($"Id is null or whitespace.");
                return;
            }

            int id;

            if (!int.TryParse(parameter, out id))
            {
                Console.WriteLine($"Invalid id: {parameter}");
                return;
            }

            if (id < 0)
            {
                Console.WriteLine("Id is less than zero.");
                return;
            }

            try
            {
                Program.FileCabinetService.RemoveRecord(id);
                Console.WriteLine($"Record #{id} is removed.");
            }
            catch
            {
                Console.WriteLine($"Record #{id} doesn't exist.");
            }
        }

        private static void Purge(string parameters)
        {
            var filesystemService = Program.FileCabinetService as FileCabinetFilesystemService;

            if (filesystemService is null)
            {
                return;
            }

            var stat = filesystemService.GetStat();

            filesystemService.PurgeRecords();

            Console.WriteLine($"Data file processing is completed: {stat.Item2} of {stat.Item1} records were purged.");
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            Program.IsRunning = false;
        }

        private static string FormatRecord(FileCabinetData record, int id) => $"#{id}, " +
            $"{record.FirstName}, " +
            $"{record.LastName}, " +
            $"{record.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture)}, " +
            $"{record.CarAmount}, " +
            $"{record.Money}, " +
            $"{record.FavoriteChar}";

        private static string LongFormatRecord(FileCabinetData record, int id, string pastAction) => $"First name: {record.FirstName}{Environment.NewLine}" +
            $"Last name: {record.LastName}{Environment.NewLine}" +
            $"Date of birth: {record.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture)}{Environment.NewLine}" +
            $"Car amount: {record.CarAmount}{Environment.NewLine}" +
            $"Money: {record.Money}{Environment.NewLine}" +
            $"Favorite char: {record.Money}{Environment.NewLine}" +
            $"Record #{id} is {pastAction}.";

        private static Tuple<bool, string, string> StringConverter(string input)
        {
            return new Tuple<bool, string, string>(true, string.Empty, input);
        }

        private static Tuple<bool, string, DateTime> DateConverter(string input)
        {
            return new Tuple<bool, string, DateTime>(
                DateTime.TryParseExact(input, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result),
                "Invalid date of birth.",
                result);
        }

        private static Tuple<bool, string, char> CharConverter(string input)
        {
            return new Tuple<bool, string, char>(char.TryParse(input, out char result), "Invalid char.", char.ToUpperInvariant(result));
        }

        private static Tuple<bool, string, short> ShortConverter(string input)
        {
            return new Tuple<bool, string, short>(short.TryParse(input, out short result), input, result);
        }

        private static Tuple<bool, string, decimal> DecimalConverter(string input)
        {
            return new Tuple<bool, string, decimal>(decimal.TryParse(input, out decimal result), input, result);
        }

        private static Tuple<bool, string> FirstNameValidator(string firstName)
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

        private static Tuple<bool, string> LastNameValidator(string lastName)
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

        private static Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
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

        private static Tuple<bool, string> CarAmountValidator(short carAmount)
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

        private static Tuple<bool, string> MoneyValidator(decimal money)
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

        private static Tuple<bool, string> FavoriteCharValidator(char favoriteChar)
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
    }
}
