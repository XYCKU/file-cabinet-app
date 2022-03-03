using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public abstract class PrintServiceCommandHandlerBase : ServiceCommandHandlerBase
    {
        private readonly IRecordPrinter printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        /// <param name="printer">Info printer.</param>
        protected PrintServiceCommandHandlerBase(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            if (printer is null)
            {
                throw new ArgumentNullException(nameof(printer));
            }

            this.printer = printer;
        }

        /// <summary>
        /// Gets printer.
        /// </summary>
        /// <value>Printer.</value>
        protected IRecordPrinter Printer => this.printer;

        /// <inheritdoc/>
        protected abstract override void Action(AppCommandRequest commandRequest);
    }
}
