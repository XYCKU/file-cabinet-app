using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        public HelpCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string Command { get; } = "help";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            if (!string.IsNullOrEmpty(commandRequest.Parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], commandRequest.Parameters, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{commandRequest.Parameters}' command.");
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
    }
}
