using FileCabinetApp.Validators.Config;

namespace FileCabinetApp.Validators.Input
{
    /// <inheritdoc/>
    public class DefaultInputValidator : InputValidatorBase
    {
        private static readonly ConfigurationData DefaultConfig = new ConfigurationData()
        {
            MinFirstNameLength = 2,
            MaxFirstNameLength = 60,
            MinLastNameLength = 2,
            MaxLastNameLength = 60,
            MinChar = 'A',
            MaxChar = 'Z',
            MinCarAmount = 0,
            MaxCarAmount = 60,
            MinMoney = 0,
            MaxMoney = 9999999999,
            MinDate = new DateTime(1950, 01, 01),
            MaxDate = DateTime.Today,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultInputValidator"/> class.
        /// </summary>
        public DefaultInputValidator()
            : base(DefaultConfig)
        {
        }
    }
}
