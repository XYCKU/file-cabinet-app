using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class CommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(commandRequest.Command, StringComparison.OrdinalIgnoreCase));

            if (index >= 0)
            {
                commands[index].Item2(commandRequest.Parameters);
            }
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}
