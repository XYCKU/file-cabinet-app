using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class StatCommandHandler : CommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        public StatCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string Command { get; } = "stat";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            var stat = this.Service.GetStat();
            Console.WriteLine($"{stat.Item1} record(s).");
            Console.WriteLine($"{stat.Item2} record(s) are deleted.");
        }
    }
}
