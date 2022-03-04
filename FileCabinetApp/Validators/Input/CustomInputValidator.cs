using FileCabinetApp.Validators.Config;

namespace FileCabinetApp.Validators.Input
{
    /// <inheritdoc/>
    public class CustomInputValidator : InputValidatorBase
    {
        private static readonly ConfigurationData CustomConfig = new ConfigurationData()
        {
            MinFirstNameLength = 2,
            MaxFirstNameLength = 60,
            MinLastNameLength = 2,
            MaxLastNameLength = 60,
            MinChar = '0',
            MaxChar = '9',
            MinCarAmount = 0,
            MaxCarAmount = 60,
            MinMoney = 0,
            MaxMoney = 9999999999,
            MinDate = new DateTime(1920, 01, 01),
            MaxDate = new DateTime(2000, 01, 01),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomInputValidator"/> class.
        /// </summary>
        public CustomInputValidator()
            : base(CustomConfig)
        {
        }
    }
}
