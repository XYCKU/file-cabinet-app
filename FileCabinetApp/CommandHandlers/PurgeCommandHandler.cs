using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string Command { get; } = "purge";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            var filesystemService = this.Service as FileCabinetFilesystemService;

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
