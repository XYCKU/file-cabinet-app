using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class ExitCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        protected override string Command { get; } = "exit";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            Console.WriteLine("Exiting an application...");
            Program.isRunning = false;
        }
    }
}
