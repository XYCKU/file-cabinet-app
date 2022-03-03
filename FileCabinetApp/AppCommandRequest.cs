using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Holds command and parameters.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Gets or sets a command.
        /// </summary>
        /// <value>A command.</value>
        public string Command { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets parameters.
        /// </summary>
        /// <value>Parameters.</value>
        public string Parameters { get; set; } = string.Empty;
    }
}
