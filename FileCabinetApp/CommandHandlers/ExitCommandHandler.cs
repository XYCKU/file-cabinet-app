using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class ExitCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            Console.WriteLine("Exiting an application...");
            Program.IsRunning = false;
        }
    }
}
