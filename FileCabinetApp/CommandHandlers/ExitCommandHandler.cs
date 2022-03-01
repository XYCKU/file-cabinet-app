using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class ExitCommandHandler : CommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        public ExitCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string Command { get; } = "exit";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            Console.WriteLine("Exiting an application...");
            Program.IsRunning = false;
        }
    }
}
