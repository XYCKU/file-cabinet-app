using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class StatCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            var stat = Program.FileCabinetService.GetStat();
            Console.WriteLine($"{stat.Item1} record(s).");
            Console.WriteLine($"{stat.Item2} record(s) are deleted.");
        }
    }
}
