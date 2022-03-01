using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class ImportCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        protected override string Command { get; } = "import";

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
    }
}
