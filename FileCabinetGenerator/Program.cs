using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Console application class.
    /// </summary>
    public static class Program
    {
        private static readonly Dictionary<string, Action<string>> ParameterArguments = new Dictionary<string, Action<string>>()
        {
            { "--output-type", (string value) => { SetExportType(value); } },
            { "-t", (string value) => { SetExportType(value); } },
            { "--output", (string value) => { SetFilePath(value); } },
            { "-o", (string value) => { SetFilePath(value); } },
            { "--records-amount", (string value) => { SetRecordsAmount(value); } },
            { "-a", (string value) => { SetRecordsAmount(value); } },
            { "--start-id", (string value) => { SetStartId(value); } },
            { "-i", (string value) => { SetStartId(value); } },
        };

        private static readonly string DefaultFileName = "defaultFile.txt";
        private static string exportType = string.Empty;
        private static string path = string.Empty;
        private static int recordAmount;
        private static int startId;

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Console arguments.</param>
        public static void Main(string[] args)
        {
            ProcessArguments(args);

            Console.WriteLine($"{exportType}{Environment.NewLine}{path}{Environment.NewLine}{recordAmount}{Environment.NewLine}{startId}");

            var generator = new FileCabinetRecordGenerator();

            FileCabinetRecord[] records = generator.Generate(recordAmount, 45);

            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    var snapshot = new FileCabinetServiceSnapshot(records);

                    switch (exportType)
                    {
                        case "csv":
                            snapshot.SaveToCsv(writer);
                            break;
                        case "xml":
                            snapshot.SaveToXml(writer);
                            break;
                        default:
                            Console.WriteLine($"Invalid export type: {exportType}");
                            return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine($"{recordAmount} were written to {path}");
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
                string argument;

                const string fullParameter = "--";
                const string shortParameter = "--";

                if (lowerCaseArgument.StartsWith(fullParameter, StringComparison.OrdinalIgnoreCase))
                {
                    string[] splitted = args[i].Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
                    lowerCaseArgument = splitted[0];
                    argument = splitted[1];
                }
                else if (lowerCaseArgument.StartsWith(shortParameter, StringComparison.OrdinalIgnoreCase))
                {
                    argument = args[++i];
                }
                else
                {
                    Console.WriteLine($"Invalid argument: {lowerCaseArgument}");
                    continue;
                }

                if (ParameterArguments.ContainsKey(lowerCaseArgument))
                {
                    ParameterArguments[lowerCaseArgument](argument);
                }
                else
                {
                    Console.WriteLine($"Invalid argument {args[i]}");
                }
            }
        }

        private static void SetExportType(string value)
        {
            exportType = value.ToLowerInvariant();
        }

        private static void SetStartId(string value)
        {
            if (int.TryParse(value, out int result))
            {
                if (result < 0)
                {
                    Console.WriteLine($"Start id is less than zero");
                    return;
                }

                startId = result;
            }
        }

        private static void SetRecordsAmount(string value)
        {
            if (int.TryParse(value, out int result))
            {
                if (result < 0)
                {
                    Console.WriteLine($"Record amount is less than zero");
                    return;
                }

                recordAmount = result;
            }
        }

        private static void SetFilePath(string value)
        {
            path = string.IsNullOrWhiteSpace(value) ? DefaultFileName : value;
        }
    }
}