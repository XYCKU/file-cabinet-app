using FileCabinetApp.Validators.Config;

namespace FileCabinetApp.Validators.Input
{
    /// <inheritdoc/>
    public class InputValidator : InputValidatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputValidator"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public InputValidator(ConfigurationData config)
            : base(config)
        {
        }
    }
}
