using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class ListCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        protected override string Command { get; } = "list";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            var records = Program.FileCabinetService.GetRecords();

            for (int i = 0; i < records.Count; ++i)
            {
                Console.WriteLine(records[i]);
            }
        }
    }
}
