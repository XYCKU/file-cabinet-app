using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler? nextHandler;

        /// <summary>
        /// Gets command.
        /// </summary>
        /// <value>Command.</value>
        protected virtual string Command { get; } = string.Empty;

        /// <inheritdoc/>
        public virtual void Handle(AppCommandRequest commandRequest)
        {
            if (string.Equals(commandRequest.Command, this.Command, StringComparison.OrdinalIgnoreCase))
            {
                this.Action(commandRequest);
            }
            else
            {
                if (this.nextHandler != null)
                {
                    this.nextHandler.Handle(commandRequest);
                }
            }
        }

        /// <inheritdoc/>
        public void SetNext(ICommandHandler commandHandler)
        {
            if (commandHandler is null)
            {
                throw new ArgumentNullException(nameof(commandHandler));
            }

            this.nextHandler = commandHandler;
        }

        /// <summary>
        /// Action to do.
        /// </summary>
        /// <param name="commandRequest">Command info.</param>
        protected abstract void Action(AppCommandRequest commandRequest);
    }
}
