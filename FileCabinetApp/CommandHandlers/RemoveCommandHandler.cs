using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        public RemoveCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string Command { get; } = "remove";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            if (string.IsNullOrWhiteSpace(commandRequest.Parameters))
            {
                Console.WriteLine($"Id is null or whitespace.");
                return;
            }

            int id;

            if (!int.TryParse(commandRequest.Parameters, out id))
            {
                Console.WriteLine($"Invalid id: {commandRequest.Parameters}");
                return;
            }

            if (id < 0)
            {
                Console.WriteLine("Id is less than zero.");
                return;
            }

            try
            {
                this.Service.RemoveRecord(id);
                Console.WriteLine($"Record #{id} is removed.");
            }
            catch
            {
                Console.WriteLine($"Record #{id} doesn't exist.");
            }
        }
    }
}
