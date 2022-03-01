using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class ListCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            var records = Program.FileCabinetService.GetRecords();

            for (int i = 0; i < records.Count; ++i)
            {
                Console.WriteLine(records[i]);
            }
        }
    }
}
