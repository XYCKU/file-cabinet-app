using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        public ListCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string Command { get; } = "list";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            var records = this.Service.GetRecords();

            for (int i = 0; i < records.Count; ++i)
            {
                Console.WriteLine(records[i]);
            }
        }
    }
}
