using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class ExportCommandHandler : CommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        public ExportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string Command { get; } = "export";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            if (string.IsNullOrWhiteSpace(commandRequest.Parameters))
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            const int ArgumentsAmount = 2;
            string[] args = commandRequest.Parameters.Split(' ', ArgumentsAmount, StringSplitOptions.RemoveEmptyEntries);

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
                    FileCabinetServiceSnapshot snapshot = this.Service.MakeSnapshot();

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
    }
}
