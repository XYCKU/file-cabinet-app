using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public abstract class PrintServiceCommandHandlerBase : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        /// <param name="printer">Info printer.</param>
        protected PrintServiceCommandHandlerBase(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            this.Printer = printer ?? throw new ArgumentNullException(nameof(printer));
        }

        /// <summary>
        /// Gets printer.
        /// </summary>
        /// <value>Printer.</value>
        protected IRecordPrinter Printer { get; }

        /// <inheritdoc/>
        protected abstract override void Action(AppCommandRequest commandRequest);
    }
}
