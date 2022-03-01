using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class StatCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        protected override string Command { get; } = "stat";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            var stat = Program.FileCabinetService.GetStat();
            Console.WriteLine($"{stat.Item1} record(s).");
            Console.WriteLine($"{stat.Item2} record(s) are deleted.");
        }
    }
}
