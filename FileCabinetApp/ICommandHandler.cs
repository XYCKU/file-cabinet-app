using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Handles a command.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Sets next <see cref="ICommandHandler"/> in chain.
        /// </summary>
        /// <param name="commandHandler">Next <see cref="ICommandHandler"/>.</param>
        public void SetNext(ICommandHandler commandHandler);

        /// <summary>
        /// Handles a <see cref="AppCommandRequest"/>.
        /// </summary>
        /// <param name="commandRequest"><see cref="AppCommandRequest"/> data.</param>
        public void Handle(AppCommandRequest commandRequest);
    }
}
