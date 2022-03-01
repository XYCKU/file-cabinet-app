using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class PurgeCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        protected override string Command { get; } = "purge";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            var filesystemService = Program.FileCabinetService as FileCabinetFilesystemService;

            if (filesystemService is null)
            {
                return;
            }

            var stat = filesystemService.GetStat();

            filesystemService.PurgeRecords();

            Console.WriteLine($"Data file processing is completed: {stat.Item2} of {stat.Item1} records were purged.");
        }
    }
}
