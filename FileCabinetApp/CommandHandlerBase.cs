using System;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler? nextHandler;

        /// <inheritdoc/>
        public abstract void Handle(AppCommandRequest commandRequest);

        /// <inheritdoc/>
        public void SetNext(ICommandHandler commandHandler)
        {
            if (commandHandler is null)
            {
                throw new ArgumentNullException(nameof(commandHandler));
            }

            this.nextHandler = commandHandler;
        }
    }
}
