using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class ListCommandHandler : PrintServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        /// <param name="printer">Printer.</param>
        public ListCommandHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service, printer)
        {
        }

        /// <inheritdoc/>
        protected override string Command { get; } = "list";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            var records = this.Service.GetRecords();

            Console.WriteLine($"File cabinet has {records.Count} records.");
            this.Printer.Print(records);
        }
    }
}
