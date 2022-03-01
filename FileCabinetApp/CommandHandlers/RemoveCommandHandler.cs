using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class RemoveCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
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
                Program.FileCabinetService.RemoveRecord(id);
                Console.WriteLine($"Record #{id} is removed.");
            }
            catch
            {
                Console.WriteLine($"Record #{id} doesn't exist.");
            }
        }
    }
}
