using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private readonly Action<bool> setProgramState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="action">Action for interaction with program state.</param>
        public ExitCommandHandler(Action<bool> action)
        {
            this.setProgramState = action;
        }

        /// <inheritdoc/>
        protected override string Command { get; } = "exit";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            Console.WriteLine("Exiting an application...");
            this.setProgramState(false);
        }
    }
}
