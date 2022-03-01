using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class MissedCommandHandler : CommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissedCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        public MissedCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

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
