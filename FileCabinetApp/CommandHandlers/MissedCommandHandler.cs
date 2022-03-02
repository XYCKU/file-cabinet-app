using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class MissedCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            this.Action(commandRequest);
        }

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            Console.WriteLine($"There is no '{commandRequest.Command}' command.");
            Console.WriteLine();
        }
    }
}
