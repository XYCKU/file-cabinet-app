namespace FileCabinetGenerator
{
    /// <summary>
    /// Console application class.
    /// </summary>
    public static class Program
    {
        private static readonly Dictionary<string, Action<string>> ParameterArguments = new Dictionary<string, Action<string>>()
        {
            { "--output-type", (string value) => { exporter = GetExporter(value); } },
            { "-t", (string value) => { exporter = GetExporter(value); } },
            { "--output", (string value) => { SetFilePath(value); } },
            { "-o", (string value) => { SetFilePath(value); } },
            { "--records-amount", (string value) => { SetRecordsAmount(value); } },
            { "-a", (string value) => { SetRecordsAmount(value); } },
            { "--start-id", (string value) => { SetStartId(value); } },
            { "-i", (string value) => { SetStartId(value); } },
        };

        private static readonly string DefaultFileName = "defaultFile.txt";
        private static IExporter exporter = new CsvExporter();
        private static string path = string.Empty;
        private static string fileName = string.Empty;
        private static int recordAmount;
        private static int startId;

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Console arguments.</param>
        public static void Main(string[] args)
        {
            ProcessArguments(args);

            Console.WriteLine($"{exporter}{Environment.NewLine}{path}{Environment.NewLine}{fileName}{Environment.NewLine}{recordAmount}{Environment.NewLine}{startId}");

            Console.WriteLine($"{recordAmount} were written to {path}\\{fileName}");
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

        private static IExporter GetExporter(string value) => value.ToLowerInvariant() switch
        {
            "csv" => new CsvExporter(),
            "xml" => new XmlExporter(),
            _ => new CsvExporter(),
        };

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
            try
            {
                path = Path.GetDirectoryName(value) ?? string.Empty;
                fileName = Path.GetFileName(value);

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = DefaultFileName;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Path is invalid: {e.Message}");
            }
        }
    }
}