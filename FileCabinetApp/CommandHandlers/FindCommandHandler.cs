using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class FindCommandHandler : PrintServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        /// <param name="printer">Printer.</param>
        public FindCommandHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service, printer)
        {
        }

        /// <inheritdoc/>
        protected override string Command { get; } = "find";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            if (string.IsNullOrWhiteSpace(commandRequest.Parameters))
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            string[] args = commandRequest.Parameters.Split(' ', 2);

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
                    result = this.Service.FindByFirstName(searchText);
                    break;
                case "lastname":
                    result = this.Service.FindByLastName(searchText);
                    break;
                case "dateofbirth":
                    DateTime dt;
                    if (DateTime.TryParseExact(searchText, Program.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        result = this.Service.FindByDateOfBirth(dt);
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

            Console.WriteLine($"Found {result.Count} results.");

            this.Printer.Print(result);
        }
    }
}
